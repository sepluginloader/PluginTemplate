using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRageMath;

namespace ClientPlugin.Settings.Layouts
{
    internal class None : Layout
    {
        public override Vector2 ScreenSize => new Vector2(0.5f, 0.5f);

        public None(Func<List<List<MyGuiControlBase>>> getControls) : base(getControls) { }

        public override List<MyGuiControlBase> RecreateControls()
        {
            return GetControls().SelectMany(x => x).ToList();
        }

        public override void LayoutControls()
        {
            foreach (var group in GetControls())
            {
                foreach (var control in group)
                {
                    control.Position = Vector2.Zero;
                }
            }
        }
    }
}
