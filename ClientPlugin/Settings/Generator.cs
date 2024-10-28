using ClientPlugin.Settings.Elements;
using ClientPlugin.Settings.Layouts;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;


namespace ClientPlugin.Settings
{
    internal class Generator
    {
        public readonly string Name;

        private readonly List<Tuple<IElement, string, Func<object>, Action<object>>> Attributes;
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

            foreach (var item in Attributes)
            {
                Controls.Add(item.Item1.GetElements(item.Item2, item.Item3, item.Item4));
            }
        }

        private static List<Tuple<IElement, string, Func<object>, Action<object>>> ExtractAttributes()
        {
            var config = new List<Tuple<IElement, string, Func<object>, Action<object>>>();

            foreach (var propertyInfo in typeof(Config).GetProperties())
            {
                string name = propertyInfo.Name;
                
                object getter() => propertyInfo.GetValue(Config.Current);
                void setter(object value) => propertyInfo.SetValue(Config.Current, value);

                foreach (var attribute in propertyInfo.GetCustomAttributes())
                {
                    if (attribute is IElement element)
                    {
                        config.Add(Tuple.Create(element, UnCamelCase(name), (Func<object>)getter, (Action<object>)setter));
                    }
                }
            }

            return config;
        }
    }
}
