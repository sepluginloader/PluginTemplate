using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using ClientPlugin.Settings.Elements;
using VRage.Utils;
using VRageMath;

namespace ClientPlugin.Settings.Layouts
{
    internal class Simple : Layout
    {
        private MyGuiControlParent Parent;
        private MyGuiControlScrollablePanel ScrollPanel;

        public override Vector2 SettingsPanelSize => new Vector2(0.5f, 0.7f);
        private const float ElementPadding = 0.01f;

        public Simple(Func<List<List<Control>>> getControls) : base(getControls) { }

        public override List<MyGuiControlBase> RecreateControls()
        {
            Parent = new MyGuiControlParent()
            {
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
                Position = Vector2.Zero, 
                Size = new Vector2(SettingsPanelSize.X-0.01f, SettingsPanelSize.Y-0.09f),
            };

            ScrollPanel = new MyGuiControlScrollablePanel(Parent)
            {
                BackgroundTexture = null,
                BorderHighlightEnabled = false,
                BorderEnabled = false,
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
                Position = new Vector2(0f, 0.03f), // Do not overlap the dialog's title
                Size = Parent.Size,
                ScrollbarVEnabled = true,
                CanFocusChildren = true,
                ScrolledAreaPadding = new MyGuiBorderThickness(0.005f),
                DrawScrollBarSeparator = true,
            };

            foreach (var row in GetControls())
            {
                foreach (var control in row)
                {
                    Parent.Controls.Add(control.GuiControl);
                }
            }

            return new List<MyGuiControlBase> { ScrollPanel };
        }

        public override void LayoutControls()
        {
            var totalWidth = ScrollPanel.ScrolledAreaSize.X - 2 * ElementPadding;
            
            var controls = GetControls();
            var totalHeight = ElementPadding + controls.Select(row => row.Max(c => c.GuiControl.Size.Y) + ElementPadding).Sum();
            Parent.Size = new Vector2(Parent.Size.X, totalHeight);
            
            var rowY = -0.5f * totalHeight + ElementPadding;
            foreach (var row in controls)
            {
                // Vertical
                
                var rowHeight = row.Max(c => c.GuiControl.Size.Y);
                var controlY = rowY + 0.5f * rowHeight;
                
                rowY += rowHeight + ElementPadding;
                
                // Horizontal
                
                var totalMinWidth = row.Select(c => (c.FixedWidth ?? c.MinWidth) + c.RightMargin).Sum();
                var remainingWidth = Math.Max(0f, totalWidth - totalMinWidth);
                var sumFillFactors = row.Select(c => c.FixedWidth.HasValue ? 0f : c.FillFactor ?? 0f).Sum();
                var unitWidth = sumFillFactors > 0f ? remainingWidth / sumFillFactors : 0f;

                var controlX = -0.5f * Parent.Size.X + ElementPadding;
                foreach (var control in row)
                {
                    var guiControl = control.GuiControl;
                    guiControl.Position = new Vector2(controlX, controlY) + control.Offset;
                    guiControl.OriginAlign = control.OriginAlign;

                    var sizeY = guiControl.Size.Y;
                    if (control.FixedWidth.HasValue)
                    {
                        guiControl.Size = new Vector2(control.FixedWidth.Value, sizeY);
                        guiControl.SetMaxWidth(control.FixedWidth.Value);
                    }
                    else if (control.FillFactor.HasValue)
                    {
                        guiControl.Size = new Vector2(Math.Max(control.MinWidth, unitWidth * control.FillFactor.Value), sizeY);
                    } 
                    else
                    {
                        guiControl.Size = new Vector2(Math.Max(guiControl.Size.X, control.MinWidth), sizeY);
                    }

                    controlX += guiControl.Size.X + control.RightMargin;
                }
            }
            
            ScrollPanel.RefreshInternals();
        }
    }
}
