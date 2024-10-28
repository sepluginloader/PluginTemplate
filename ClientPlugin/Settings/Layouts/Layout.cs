﻿using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRageMath;

namespace ClientPlugin.Settings.Layouts
{
    internal abstract class Layout
    {
        /// <summary>
        /// Size of the UI screen this layout is responsible for.
        /// </summary>
        public abstract Vector2 ScreenSize { get; }

        /// <summary>
        /// Call this to recieve a list of rows of controls.
        /// </summary>
        protected readonly Func<List<List<MyGuiControlBase>>> GetControls;

        public Layout(Func<List<List<MyGuiControlBase>>> getControls)
        {
            GetControls = getControls;
        }

        /// <summary>
        /// Recreates all of the layout-specific controls.
        /// Includes setting up parents.
        /// </summary>
        /// <returns>Any controls to be parented to the screen.</returns>
        public abstract List<MyGuiControlBase> RecreateControls();

        /// <summary>
        /// Layout all the exiting controls. Do not create controls in here.
        /// </summary>
        public abstract void LayoutControls();
    }
}
