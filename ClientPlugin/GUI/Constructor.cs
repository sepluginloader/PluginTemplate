using ClientPlugin.GUI.Elements;
using Sandbox;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRageMath;
using VRage.Render11;
using VRage.Utils;
using DirectShowLib.DES;

namespace ClientPlugin.GUI
{
    internal class Constructor : MyGuiScreenBase
    {
        public override string GetFriendlyName() => Interface.FriendlyName;
        public const string Caption = "The test caption";
        private const float ElementHeight = 0.03f;
        private const float ElementPadding = 0.015f;

        public Constructor() : base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.9f, 0.9f), false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
        {
            EnabledBackgroundFade = true;
            m_closeOnEsc = true;
            m_drawEvenWithoutFocus = true;
            CanHideOthers = true;
            CanBeHidden = true;
            CloseButtonEnabled = true;

            RecreateControls(true);
        }

        private float GetIndexOffset(int index)
        {
            return index * (ElementHeight + ElementPadding);
        }

        public override void RecreateControls(bool constructor)
        {
            base.RecreateControls(constructor);

            AddCaption(Caption);

            MyGuiControlParent guiControlParent = new MyGuiControlParent();
            guiControlParent.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
            guiControlParent.Position = new Vector2(0f, 0.5f*Size.Value.Y);
            guiControlParent.Size = new Vector2(0.8f, 0.8f);

            var controlPanel = new MyGuiControlScrollablePanel(guiControlParent)
            {
                BackgroundTexture = null,
                BorderHighlightEnabled = false,
                BorderEnabled = false,
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM,
                Position = new Vector2(0f, 0.5f * Size.Value.Y),
                Size = new Vector2(0.8f, 0.8f),
                ScrollbarVEnabled = true,
                CanFocusChildren = true,
                ScrolledAreaPadding = new MyGuiBorderThickness(0.005f),
                DrawScrollBarSeparator = true,
            };

            for (int i = 0; i < Interface.Elements.Count; i++)
            {
                var element = Interface.Elements[i];

                var leftElement = element.GetLeftElement();
                var rightElement = element.GetRightElement();

                leftElement.OriginAlign = VRage.Utils.MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
                rightElement.OriginAlign = VRage.Utils.MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;

                leftElement.Position = new Vector2(-0.5f * controlPanel.Size.X, -0.5f * controlPanel.Size.Y + GetIndexOffset(i));
                rightElement.Position = new Vector2(controlPanel.ScrolledAreaSize.X - 0.5f * controlPanel.Size.X, -0.5f * controlPanel.Size.Y + GetIndexOffset(i));

                guiControlParent.Controls.Add(leftElement);
                guiControlParent.Controls.Add(rightElement);
            }

            Controls.Add(controlPanel);
        }
    }
}
