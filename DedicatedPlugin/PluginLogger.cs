using Sandbox.Graphics.GUI;
using System;
using System.Text;
using System.Windows.Forms;
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
                string severityText;
                switch (severity)
                {
                    case MyLogSeverity.Debug:
                        severityText = "DEBUG: ";
                        break;
                    case MyLogSeverity.Info:
                        severityText = "INFO: ";
                        break;
                    case MyLogSeverity.Warning:
                        severityText = "WARNING: ";
                        break;
                    case MyLogSeverity.Error:
                        severityText = "ERROR: ";
                        break;
                    case MyLogSeverity.Critical:
                        severityText = "CRITICAL: ";
                        break;
                        default:
                        severityText = "";
                            break;
                }

                if (ex.Message != null)
                {
                    MyLog.Default.Log(severity, "[" + PluginName + "]: Exception Occurred: " + ex.Message);
                    MyLog.Default.WriteLineToConsole(severityText + "[" + PluginName + "]: Exception Occurred: " + ex.Message);
                }
                else
                {
                    MyLog.Default.Log(severity, "[" + PluginName + "]: Exception Occurred: No exception message.");
                    MyLog.Default.WriteLineToConsole(severityText + "[" + PluginName + "]: Exception Occurred: No exception message.");

                if (ex.TargetSite != null)
                {
                    MyLog.Default.Log(severity, "[" + PluginName + "]: Target Site: " + ex.TargetSite);
                    MyLog.Default.WriteLineToConsole(severityText + "[" + PluginName + "]: Target Site: " + ex.TargetSite);
                }               

                if (ex.StackTrace != null)
                {
                    MyLog.Default.Log(severity, "[" + PluginName + "]: Stack Trace: " + ex.StackTrace);
                    MyLog.Default.WriteLineToConsole(severityText + "[" + PluginName + "]: Stack Trace: " + ex.StackTrace);
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
                    MyLog.Default.WriteLineToConsole(severityText + "[" + PluginName + "]: Exception Occurred: " + stringBuilder.ToString());
                }

                if (ex?.InnerException != null)
                {
                    MyLog.Default.Log(severity, "[" + PluginName + "]: InnerException: ");
                    MyLog.Default.WriteLineToConsole(severityText + "[" + PluginName + "]: InnerException: ");
                    WriteException(severity, ex.InnerException);
                }
            }
        }
    }
}
