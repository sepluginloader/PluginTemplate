using ClientPlugin.Settings.Elements;
using ClientPlugin.Settings.Layouts;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;


namespace ClientPlugin.Settings
{
    internal class AttributeInfo
    {
        public IElement ElementType;
        public string Name;
        public Func<object> Getter;
        public Action<object> Setter;
    }

    internal class SettingsGenerator
    {
        public readonly string Name;

        private readonly List<AttributeInfo> Attributes;
        private List<List<Control>> Controls;
        public SettingsScreen Dialog { get; private set; }
        public Layout ActiveLayout { get; private set; }

        private static string UnCamelCase(string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }

        private static bool validateType(Type type, List<Type> typesList)
        {
            return typesList.Any(t => t.IsAssignableFrom(type));
        }

        private static Delegate getDelegate(MethodInfo methodInfo)
        {
            // Reconstruct the type
            Type[] methodArgs = methodInfo.GetParameters().Select(p => p.ParameterType).ToArray();
            Type type = Expression.GetDelegateType(methodArgs.Concat(new[] { methodInfo.ReturnType }).ToArray());

            // Create a delegate
            return Delegate.CreateDelegate(type, null, methodInfo);
        }

        public SettingsGenerator()
        {
            Attributes = ExtractAttributes();
            Name = Config.Current.Title;
            ActiveLayout = new Layouts.None(()=>Controls);
            Dialog = new SettingsScreen(Name, OnRecreateControls, size: ActiveLayout.SettingsPanelSize);
        }

        private List<MyGuiControlBase> OnRecreateControls()
        {
            CreateConfigControls();
            List<MyGuiControlBase> controls = ActiveLayout.RecreateControls();
            ActiveLayout.LayoutControls();
            return controls;
        }

        public void SetLayout<T>() where T : Layout
        {
            ActiveLayout = (T)Activator.CreateInstance(typeof(T), (Func<List<List<Control>>>)(() => Controls));
            Dialog.UpdateSize(ActiveLayout.SettingsPanelSize);
        }

        public void RefreshLayout()
        {
            ActiveLayout.LayoutControls();
        }

        private void CreateConfigControls()
        {
            Controls = new List<List<Control>>();

            foreach (AttributeInfo info in Attributes)
            {
                Controls.Add(info.ElementType.GetControls(info.Name, info.Getter, info.Setter));
            }
        }

        private static List<AttributeInfo> ExtractAttributes()
        {
            var config = new List<AttributeInfo>();

            foreach (var propertyInfo in typeof(Config).GetProperties())
            {
                string name = propertyInfo.Name;
                
                object getter() => propertyInfo.GetValue(Config.Current);
                void setter(object value) => propertyInfo.SetValue(Config.Current, value);

                foreach (var attribute in propertyInfo.GetCustomAttributes())
                {
                    if (attribute is IElement element)
                    {
                        if (!validateType(propertyInfo.PropertyType, element.SupportedTypes))
                        {
                            throw new Exception(
                                $"Element {element.GetType().Name} for {name} expects "
                                + $"{string.Join("/", element.SupportedTypes)} but "
                                + $"recieved {propertyInfo.PropertyType.FullName}");
                        }

                        var info = new AttributeInfo()
                        {
                            ElementType = element,
                            Name = name,
                            Getter = getter,
                            Setter = setter
                        };
                        config.Add(info);
                    }
                }
            }

            foreach (var methodInfo in typeof(Config).GetMethods())
            {
                string name = methodInfo.Name;
                Delegate method = getDelegate(methodInfo);

                foreach (var attribute in methodInfo.GetCustomAttributes())
                {
                    if (attribute is IElement element)
                    {
                        if (!validateType(typeof(Delegate), element.SupportedTypes))
                        {
                            throw new Exception(
                                $"Element {element.GetType().Name} for {name} expects "
                                + $"{string.Join("/", element.SupportedTypes)} but "
                                + $"recieved {typeof(Delegate).FullName}");
                        }

                        var info = new AttributeInfo()
                        {
                            ElementType = element,
                            Name = name,
                            Getter = () => method,
                            Setter = null
                        };
                        config.Add(info);
                    }
                }
            }

            return config;
        }
    }
}
