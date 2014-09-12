/// 
/// The MIT License (MIT)
/// 
/// Copyright (c) 2014 João Francisco Biondo Trinca <wolfulus@gmail.com>
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in
/// all copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
/// THE SOFTWARE.
/// 

namespace Almirante.Engine.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;
    using Almirante.Engine.Audio;
    using Almirante.Engine.Core.Windows;
    using Almirante.Engine.Input;
    using Almirante.Engine.Resources;
    using Almirante.Engine.Scenes;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Game application class.
    /// </summary>
    public sealed class AlmiranteEngine
    {
        /// <summary>
        /// The windows
        /// </summary>
        private bool windows;

        /// <summary>
        /// The initialized
        /// </summary>
        private bool initialized;

        /// <summary>
        /// Stores a list of arguments passed to the command-line.
        /// </summary>
        private string[] arguments;

        /// <summary>
        /// The services
        /// </summary>
        private GameServiceContainer services;

        /// <summary>
        /// Gets a value indicating whether the engine is running on winforms.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [win forms]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsWinForms
        {
            get
            {
                return Instance.windows;
            }
        }

        /// <summary>
        /// Gets the instance to the game engine.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        internal static AlmiranteEngine Instance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the application.
        /// </summary>
        /// <value>
        /// The application.
        /// </value>
        internal static GameApplication Application
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        public static GameServiceContainer Services
        {
            get
            {
                if (Application != null)
                {
                    return Application.Services;
                }

                return Instance.services;
            }
        }

        /// <summary>
        /// Gets the sprite batch instance.
        /// </summary>
        /// <value>
        /// The audio.
        /// </value>
        public static AudioManager Audio
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the camera instance.
        /// </summary>
        /// <value>
        /// The camera.
        /// </value>
        public static CameraManager Camera
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the configurator.
        /// </summary>
        /// <value>
        /// The configurator instance.
        /// </value>
        public static Bootstrapper Bootstrap
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the sprite batch instance.
        /// </summary>
        /// <value>
        /// The batch.
        /// </value>
        public static SpriteBatch Batch
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the input manager.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public static InputManager Input
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the resource manager.
        /// </summary>
        /// <value>
        /// The resources.
        /// </value>
        public static ResourceManager Resources
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the scene manager.
        /// </summary>
        /// <value>
        /// The scenes.
        /// </value>
        public static SceneManager Scenes
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the engine settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public static Settings Settings
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        /// <value>
        /// The device.
        /// </value>
        public static GraphicsDevice Device
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the time manager.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public static TimeManager Time
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the graphics device manager.
        /// </summary>
        /// <value>
        /// The device manager.
        /// </value>
        public static GraphicsDeviceManager DeviceManager
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the content manager.
        /// </summary>
        /// <value>
        /// The resource contents.
        /// </value>
        internal static ResourceContentManager ResourceContents
        {
            get;
            set;
        }

        /// <summary>
        /// Starts the engine.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        public static void Start(string[] arguments)
        {
            AlmiranteEngine.Instance = new AlmiranteEngine(arguments);
            using (AlmiranteEngine.Application)
            {
                AlmiranteEngine.Instance.InitializeServices(AlmiranteEngine.Services);
                AlmiranteEngine.Application.Content = AlmiranteEngine.ResourceContents;
                AlmiranteEngine.Application.Run();
            }
        }

        /// <summary>
        /// Starts the engine for winforms.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        public static void StartWindows()
        {
            AlmiranteEngine.Instance = new AlmiranteEngine();
            AlmiranteEngine.Instance.InitializeServices(AlmiranteEngine.Services);
        }

        /// <summary>
        /// Exits the game.
        /// </summary>
        public static void Stop()
        {
            AlmiranteEngine.Application.Exit();
        }

        /// <summary>
        /// Initializes the exported types.
        /// </summary>
        private void InitializeExports()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                this.ProcessAssembly(assembly);
            }
        }

        /// <summary>
        /// Register scenes from the assemblies.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="assembly">The assembly.</param>
        /// <exception cref="System.Exception">You cannot have more than one startup class.</exception>
        private void ProcessAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes();

            var scenes = AlmiranteEngine.Scenes;
            foreach (var type in types)
            {
                if (typeof(Scene).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    var startup = type.GetCustomAttributes(typeof(StartupAttribute), false).Any();
                    scenes.Register(type, startup);
                }
                else if (typeof(Transition).IsAssignableFrom(type))
                {
                    var attrs = type.GetCustomAttributes(typeof(TransitionAttribute), false);
                    foreach (var attr in attrs)
                    {
                        var auto = attr as TransitionAttribute;
                        if (auto != null)
                        {
                            scenes.RegisterTransition(type, auto.Name);
                            break;
                        }
                    }
                }
                else if (typeof(Bootstrapper).IsAssignableFrom(type) && type != typeof(Bootstrapper) && !this.windows)
                {
                    if (AlmiranteEngine.Bootstrap != null)
                    {
                        throw new InvalidOperationException("You cannot have more than one startup class.");
                    }

                    var constructor = type.GetConstructor(new Type[0]);
                    if (constructor != null)
                    {
                        AlmiranteEngine.Bootstrap = (Bootstrapper)constructor.Invoke(new object[0]);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the services.
        /// </summary>
        private void InitializeServices(GameServiceContainer services)
        {
            AlmiranteEngine.ResourceContents = new ResourceContentManager(services, "Content");
            services.AddService(typeof(ResourceContentManager), AlmiranteEngine.ResourceContents);

            AlmiranteEngine.Time = new TimeManager();
            services.AddService(typeof(TimeManager), AlmiranteEngine.Time);

            AlmiranteEngine.Camera = new CameraManager();
            services.AddService(typeof(CameraManager), AlmiranteEngine.Camera);

            AlmiranteEngine.Audio = new AudioManager();
            services.AddService(typeof(AudioManager), AlmiranteEngine.Audio);

            if (!windows)
            {
                AlmiranteEngine.DeviceManager = new GraphicsDeviceManager(Application);
                services.AddService(typeof(GraphicsDeviceManager), AlmiranteEngine.DeviceManager);
            }

            AlmiranteEngine.Settings = new Settings();
            services.AddService(typeof(Settings), AlmiranteEngine.Settings);

            AlmiranteEngine.Resources = new ResourceManager(AlmiranteEngine.ResourceContents);
            services.AddService(typeof(ResourceManager), AlmiranteEngine.Resources);

            AlmiranteEngine.Input = new InputManager();
            services.AddService(typeof(InputManager), AlmiranteEngine.Input);

            AlmiranteEngine.Scenes = new SceneManager();
            services.AddService(typeof(SceneManager), AlmiranteEngine.Scenes);

            this.InitializeExports();
        }

        /// <summary>
        /// Initializes the windows services.
        /// </summary>
        internal void InitializeWindowsServices()
        {
            if (!initialized)
            {
                AlmiranteEngine.Time.Initialize();
                AlmiranteEngine.Camera.Initialize();
                AlmiranteEngine.Scenes.Initialize();
                this.initialized = true;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="AlmiranteEngine" /> class.
        /// <param name="arguments">Arguments.</param>
        /// </summary>
        /// <param name="windows">if set to <c>true</c> WinForms will be used.</param>
        /// <param name="arguments">The arguments.</param>
        internal AlmiranteEngine(string[] arguments)
        {
            this.windows = false;
            this.arguments = arguments;

            AlmiranteEngine.Bootstrap = null;
            AlmiranteEngine.Instance = this;
            AlmiranteEngine.Application = new GameApplication(arguments);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="AlmiranteEngine" /> class.
        /// <param name="arguments">Arguments.</param>
        /// </summary>
        /// <param name="windows">if set to <c>true</c> WinForms will be used.</param>
        /// <param name="arguments">The arguments.</param>
        internal AlmiranteEngine()
        {
            this.windows = true;
            this.services = new GameServiceContainer();

            this.arguments = new string[0];

            AlmiranteEngine.Bootstrap = null;
            AlmiranteEngine.Instance = this;
        }
    }
}