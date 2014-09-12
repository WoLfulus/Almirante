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
    using Almirante.Engine.Audio;
    using Almirante.Engine.Input;
    using Almirante.Engine.Resources;
    using Almirante.Engine.Scenes;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Main game application class.
    /// </summary>
    public sealed class GameApplication : Game
    {
        /// <summary>
        /// The arguments
        /// </summary>
        private string[] arguments = null;

        /// <summary>
        /// Creates a new instance of the <see cref="AlmiranteEngine" /> class.
        /// <param name="arguments">Arguments.</param>
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        internal GameApplication(string[] arguments)
        {
            this.arguments = arguments;
            this.Window.ClientSizeChanged += new EventHandler<EventArgs>(OnWindowResize);
        }

        /// <summary>
        /// Called when user resizes the game window.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event parameters.</param>
        private void OnWindowResize(object sender, EventArgs e)
        {
            AlmiranteEngine.Settings.Resolution.SetResolution(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
        }

        /// <summary>
        /// Performs all initialization routines.
        /// </summary>
        protected override void Initialize()
        {
            this.IsFixedTimeStep = false;
            base.Initialize();
        }

        /// <summary>
        /// Performs all initial content loading.
        /// </summary>
        protected override void LoadContent()
        {
            AlmiranteEngine.Batch = new SpriteBatch(this.GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), AlmiranteEngine.Batch);

            AlmiranteEngine.Device = this.GraphicsDevice;
            this.Services.AddService(typeof(GraphicsDevice), this.GraphicsDevice);

            AlmiranteEngine.Settings.Resolution.Initialize();

            AlmiranteEngine.Time.Initialize();
            AlmiranteEngine.Camera.Initialize();
            AlmiranteEngine.Scenes.Initialize();

            Type sceneType = null;
            if (AlmiranteEngine.Bootstrap != null)
            {
                sceneType = AlmiranteEngine.Bootstrap.Execute(this.arguments);
                if (sceneType != null)
                {
                    AlmiranteEngine.Scenes.Set(sceneType);
                }
            }

            AlmiranteEngine.Scenes.Start();

            base.LoadContent();
        }

        /// <summary>
        /// Resource unloading.
        /// </summary>
        protected override void UnloadContent()
        {
            AlmiranteEngine.Scenes.Uninitialize();
            base.UnloadContent();
        }

        /// <summary>
        /// Game logic update.
        /// </summary>
        /// <param name="gameTime">Game timing snapshot.</param>
        protected override void Update(GameTime gameTime)
        {
            AlmiranteEngine.Time.Update(gameTime);
            AlmiranteEngine.Input.Update();
            AlmiranteEngine.Scenes.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// Game rendering.
        /// </summary>
        /// <param name="gameTime">Game timing snapshot.</param>
        protected override void Draw(GameTime gameTime)
        {
            AlmiranteEngine.Settings.Resolution.ApplyTotalViewport();
            AlmiranteEngine.Device.Clear(Color.Black);
            AlmiranteEngine.Settings.Resolution.ApplyScaledViewport();
            AlmiranteEngine.Scenes.Draw(AlmiranteEngine.Batch);
            base.Draw(gameTime);
        }
    }
}