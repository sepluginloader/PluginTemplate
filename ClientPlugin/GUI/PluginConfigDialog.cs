using System;
using System.Text;
using Sandbox;
using Sandbox.Graphics.GUI;
using Shared.Plugin;
using VRage;
using VRage.Utils;
using VRageMath;

namespace ClientPlugin.GUI
{
    public class PluginConfigDialog : MyGuiScreenBase
    {
        private const string Caption = "PluginTemplate Configuration";
        public override string GetFriendlyName() => "PluginTemplateConfigDialog";

        private MyLayoutTable layoutTable;

        private MyGuiControlLabel enabledLabel;
        private MyGuiControlCheckbox enabledCheckbox;

        // TODO: Add member variables for your UI controls here

        private MyGuiControlMultilineText infoText;
        private MyGuiControlButton closeButton;

        public PluginConfigDialog() : base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.9f, 0.9f), false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
        {
            EnabledBackgroundFade = true;
            m_closeOnEsc = true;
            m_drawEvenWithoutFocus = true;
            CanHideOthers = true;
            CanBeHidden = true;
            CloseButtonEnabled = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            RecreateControls(true);
        }

        public override void RecreateControls(bool constructor)
        {
            base.RecreateControls(constructor);

            CreateControls();
            LayoutControls();
        }

        private void CreateControls()
        {
            AddCaption(Caption);

            var config = Common.Config;
            CreateCheckbox(out enabledLabel, out enabledCheckbox, config.Enabled, value => config.Enabled = value, "Enabled", "Enable the plugin");

            OnEnableChanged();

            enabledCheckbox.IsCheckedChanged += OnEnableChanged;

            infoText = new MyGuiControlMultilineText
            {
                Name = "InfoText",
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
                TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
                TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
                // TODO: Add 2 short lines of text here if the player needs to know something. Ask for feedback here. Etc.
                Text = new StringBuilder("\r\nTODO")
            };

            closeButton = new MyGuiControlButton(originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: OnOk);
        }

        private void OnOk(MyGuiControlButton _) => CloseScreen();

        private void CreateCheckbox(out MyGuiControlLabel labelControl, out MyGuiControlCheckbox checkboxControl, bool value, Action<bool> store, string label, string tooltip, bool enabled = true)
        {
            labelControl = new MyGuiControlLabel
            {
                Text = label,
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
                Enabled = enabled,
            };

            checkboxControl = new MyGuiControlCheckbox(toolTip: tooltip)
            {
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
                IsChecked = value,
                Enabled = enabled,
                CanHaveFocus = enabled
            };
            if (enabled)
            {
                checkboxControl.IsCheckedChanged += cb => store(cb.IsChecked);
            }
            else
            {
                checkboxControl.IsCheckedChanged += cb => { cb.IsChecked = value; };
            }
        }

        private void OnEnableChanged(MyGuiControlCheckbox _ = null)
        {
            var enabled = enabledCheckbox.IsChecked;
            // TODO: Set the Enabled property of all other controls here
            // For example:
            // someFeatureCheckbox.Enabled = enabled;
        }

        private void LayoutControls()
        {
            layoutTable = new MyLayoutTable(this, new Vector2(-0.4f, -0.4f), new Vector2(0.8f, 0.8f));
            layoutTable.SetColumnWidths(120f, 880f);
            layoutTable.SetRowHeights(150f, 60f, /* TODO: Add an item for each new row */ 150f);

            layoutTable.Add(enabledCheckbox, MyAlignH.Left, MyAlignV.Center, 0, 0);
            layoutTable.Add(enabledLabel, MyAlignH.Left, MyAlignV.Center, 0, 1);

            var row = 1;

            // TODO: Add all your labels and controls to layoutTable, change the layout if required
            // For example:
            // layoutTable.Add(someFeatureCheckbox, MyAlignH.Left, MyAlignV.Center, row, 0);
            // layoutTable.Add(someFeatureLabel, MyAlignH.Left, MyAlignV.Center, row, 1);
            // row++;


            layoutTable.Add(infoText, MyAlignH.Left, MyAlignV.Top, row++, 0, colSpan: 2);
            layoutTable.Add(closeButton, MyAlignH.Center, MyAlignV.Center, row, 1, colSpan: 2);
        }
    }
}