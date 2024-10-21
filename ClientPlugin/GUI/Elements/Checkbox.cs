using Sandbox.Graphics.GUI;
using ClientPlugin.GUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRageMath;

namespace ClientPlugin.GUI.Builders
{
    internal class Checkbox : Base
    {
        public event Action<bool> OnCheck;

        public Checkbox(string identifier, string displayName, string toolTip, Action<bool> onCheck = null)
        {
            Identifier = identifier; 
            DisplayName = displayName;
            ToolTip = toolTip;
            OnCheck += onCheck;
        }

        public override MyGuiControlBase GetLeftElement()
        {
            return base.GetLeftElement();
        }

        public override MyGuiControlBase GetRightElement()
        {
            MyGuiControlCheckbox checkBox = new MyGuiControlCheckbox(toolTip: ToolTip);
            new MyGuiControlCheckbox(toolTip: ToolTip)
            {
                Size = new Vector2(0.5f, 0.5f),
                IsChecked = false,
                IsCheckedChanged = (x) => OnCheck(x.IsChecked),
                Name = Identifier,
            };

            return checkBox;
        }
    }
}
