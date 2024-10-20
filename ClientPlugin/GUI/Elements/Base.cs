using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientPlugin.GUI.Elements
{
    internal class Base
    {
        public string Identifier { get; set; }
        public string DisplayName { get; set; }
        public string ToolTip { get; set; }

        public virtual MyGuiControlBase GetLeftElement()
        {
            return new MyGuiControlLabel(text: DisplayName);
        }

        public virtual MyGuiControlBase GetRightElement()
        {
            return new MyGuiControlLabel();
        }
    }
}
