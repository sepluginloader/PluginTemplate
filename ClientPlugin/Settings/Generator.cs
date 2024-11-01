using ClientPlugin.Settings.Elements;
using ClientPlugin.Settings.Layouts;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
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

    internal class Generator
    {
        public readonly string Name;

        private readonly List<AttributeInfo> Attributes;
        private List<List<MyGuiControlBase>> Controls;
        public Screen Dialog { get; private set; }
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

        public Generator()
        {
            Attributes = ExtractAttributes();
            Name = Config.Current.Title;
            ActiveLayout = new Layouts.None(()=>Controls);
            Dialog = new Screen(Name, OnRecreateControls, size: ActiveLayout.ScreenSize);
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
            ActiveLayout = (T)Activator.CreateInstance(typeof(T), (Func<List<List<MyGuiControlBase>>>)(() => Controls));
            Dialog.UpdateSize(ActiveLayout.ScreenSize);
        }

        public void RefreshLayout()
        {
            ActiveLayout.LayoutControls();
        }

        private void CreateConfigControls()
        {
            Controls = new List<List<MyGuiControlBase>>();

            foreach (AttributeInfo info in Attributes)
            {
                Controls.Add(info.ElementType.GetElements(info.Name, info.Getter, info.Setter));
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
                Delegate method = methodInfo.CreateDelegate(typeof(Config), Config.Current);

                foreach (var attribute in methodInfo.GetCustomAttributes())
                {
                    if (attribute is IElement element)
                    {
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
