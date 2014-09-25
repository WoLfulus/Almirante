using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Almirante.Engine.Core;

namespace Tests.NetworkClient.System
{
    /// <summary>
    /// </summary>
    public class Bootstrap : Bootstrapper
    {
        /// <summary>
        /// Startup method
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns>The type of the main game class.</returns>
        protected override Type OnStartup(string[] arguments)
        {
            return null;
        }

        /// <summary>
        /// Setup your game configuration here.
        /// </summary>
        /// <param name="settings">Settings</param>
        protected override void OnConfigure(Settings settings)
        {
            settings.WindowTitle = "Network";
            settings.IsCursorVisible = true;
            settings.Resolution.SetResolution(1280, 720, false); // Try messing with this values
            settings.Resolution.SetBaseResolution(1280, 720); // DONT CHANGE
            settings.VerticalSync = false;
        }
    }
}
