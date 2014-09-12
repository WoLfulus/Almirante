using System;
using Almirante.Engine.Core;
using Almirante.Engine.Scenes;
using Almirante.Entities;

namespace Tests.Entites
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
            // EntityManager.OnInitialize();
            AlmiranteEngine.Start(args);
        }
    }

    /// <summary>
    /// Application configuration class.
    /// </summary>
    public class ApplicationConfigurator : Bootstrapper
    {
        /// <summary>
        /// Sets all game settings.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        protected override Type OnStartup(string[] arguments)
        {
            return null;
        }

        /// <summary>
        /// Sets all game settings.
        /// </summary>
        /// <param name="settings">The settings instance.</param>
        protected override void OnConfigure(Settings settings)
        {
            //settings.Resolution.SetBaseResolution(800, 600);
            settings.Resolution.SetResolution(1280, 720, false);
            settings.VerticalSync = false;
            settings.UseFixedTimestep = false;
            settings.IsCursorVisible = true;
        }
    }

#endif
}

