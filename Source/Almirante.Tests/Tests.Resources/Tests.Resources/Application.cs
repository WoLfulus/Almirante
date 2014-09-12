using Almirante.Engine.Core;
using Almirante.Engine.Scenes;
using System;

namespace Tests.Resources
{
#if WINDOWS || XBOX

    /// <summary>
    /// Application class.
    /// </summary>
    internal static class Application
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            AlmiranteEngine.Start(args);
        }
    }

    /// <summary>
    /// Application configuration class.
    /// </summary>
    public class ApplicationConfigurator : Bootstrapper
    {
        /// <summary>
        /// Setups all game settings.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        protected override Type OnStartup(string[] arguments)
        {
            return null;
        }

        /// <summary>
        /// Setups all game settings.
        /// </summary>
        /// <param name="settings">The settings instance.</param>
        protected override void OnConfigure(Settings settings)
        {
            settings.Resolution.SetResolution(1280, 720, false);
            settings.VerticalSync = true;
            settings.UseFixedTimestep = false;
            settings.IsCursorVisible = true;
        }
    }

#endif
}