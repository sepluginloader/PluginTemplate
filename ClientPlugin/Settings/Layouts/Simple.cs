using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Utils;
using VRageMath;

namespace ClientPlugin.Settings.Layouts
{
    internal class Simple : Layout
    {
        private MyGuiControlParent Parent;
        private MyGuiControlScrollablePanel ScrollPanel;

        public override Vector2 ScreenSize => new Vector2(0.4f, 0.7f);
        private const float ElementPadding = 0.01f;
        private static float Subdivide(int index, int total, float length)
        {
            return length / (total - 1) * index;
        }


        public Simple(Func<List<List<MyGuiControlBase>>> getControls) : base(getControls) { }

        public override List<MyGuiControlBase> RecreateControls()
        {
            Parent = new MyGuiControlParent()
            {
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM,
                Position = 0.5f * ScreenSize,
                Size = new Vector2(ScreenSize.X-0.01f, ScreenSize.Y-0.09f),
            };

            ScrollPanel = new MyGuiControlScrollablePanel(Parent)
            {
                BackgroundTexture = null,
                BorderHighlightEnabled = false,
                BorderEnabled = false,
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM,
                Position = 0.5f * ScreenSize,
                Size = Parent.Size,
                ScrollbarVEnabled = true,
                CanFocusChildren = true,
                ScrolledAreaPadding = new MyGuiBorderThickness(0.005f),
                DrawScrollBarSeparator = true,
            };

            foreach (var row in GetControls())
            {
                foreach (var element in row)
                {
                    Parent.Controls.Add(element);
                }
            }

            return new List<MyGuiControlBase> { ScrollPanel };
        }

        public override void LayoutControls()
        {
            var controls = GetControls();
            for (int rowIndex = 0; rowIndex < controls.Count; rowIndex++)
            {
                var row = controls[rowIndex];
                float rowHeight = row.Max(x => x.Size.Y);

                float rowY;
                if (rowIndex == 0)
                {
                    rowY = -0.5f * ScrollPanel.Size.Y + 0.5f * rowHeight;
                }
                else
                {
                    var oldRow = controls[rowIndex - 1];
                    float oldRowY = oldRow.First().PositionY;
                    float oldRowHeight = oldRow.Max(x => x.Size.Y);
                    float oldRowEnd = oldRowY + 0.5f * oldRowHeight;
                    rowY = oldRowEnd + ElementPadding + 0.5f * rowHeight;
                }

                for (int elementIndex = 0; elementIndex < row.Count; elementIndex++)
                {
                    var element = row[elementIndex];

                    float elementX = -0.5f * ScrollPanel.Size.X;
                    if (row.Count > 1)
                    {
                        elementX += Subdivide(elementIndex, row.Count, ScrollPanel.ScrolledAreaSize.X);
                    }

                    element.Position = new Vector2(elementX, rowY);

                    if (elementIndex == 0)
                    {
                        element.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
                    }
                    else if (elementIndex == row.Count - 1)
                    {
                        element.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
                    }
                    else
                    {
                        element.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
                    }

                    element.SetMaxWidth(ScrollPanel.ScrolledAreaSize.X / row.Count);
                }
            }
        }
    }
}
