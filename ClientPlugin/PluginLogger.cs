using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage.Utils;

namespace ClientPlugin
{
    internal class PluginLogger
    {
        private readonly string PluginName;

        public PluginLogger(string pluginName)
        {
            PluginName = pluginName;
        }

        public void WriteLine(MyLogSeverity severity, string message)
        {
            if (MyLog.Default != null)
            {
                MyLog.Default.Log(severity, "[" + PluginName + "]: " + message);
            }
        }

        public void WriteException(MyLogSeverity severity, Exception ex)
        {
            if (MyLog.Default != null)
            {
                if (ex.Message != null)
                {
                    MyLog.Default.Log(severity, "[" + PluginName + "]: Exception Occurred: " + ex.Message);
                    MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder("The plugin may not work properly from now on and more errors may occur. It is recommended to disable the plugin and report the issue to the developer of the plugin. Extra info: " + "[" + PluginName + "]: Exception Occurred: " + ex.Message), new StringBuilder("An Error has Occurred:")));
                }
                else
                {
                    MyLog.Default.Log(severity, "[" + PluginName + "]: Exception Occurred: No exception message.");
                    MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder("The plugin may not work properly from now on and more errors may occur. It is recommended to disable the plugin and report the issue to the developer of the plugin. Extra info: " + "[" + PluginName + "]: Exception Occurred: No exception message."), new StringBuilder("An Error has Occurred:")));
                }

                if (ex.TargetSite != null)
                {
                    MyLog.Default.Log(severity, "[" + PluginName + "]: Target Site: " + ex.TargetSite);
                }               

                if (ex.StackTrace != null)
                {
                    MyLog.Default.Log(severity, "[" + PluginName + "]: Stack Trace: " + ex.StackTrace);
                }

                if (ex != null && ex.Data.Count > 0)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("Exception Data:");
                    foreach (object key in ex.Data.Keys)
                    {
                        stringBuilder.AppendFormat("\n\t{0}: {1}", key, ex.Data[key]);
                    }
                    MyLog.Default.Log(severity, "[" + PluginName + "]: Exception Occurred: " + stringBuilder.ToString());
                }

                if (ex?.InnerException != null)
                {
                    MyLog.Default.Log(severity, "[" + PluginName + "]: InnerException: ");
                    WriteException(severity, ex.InnerException);
                }
            }
        }
    }
}
