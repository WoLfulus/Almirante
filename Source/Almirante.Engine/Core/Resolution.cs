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
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Resolution management class.
    /// </summary>
    public sealed class Resolution
    {
        /// <summary>
        /// Current width.
        /// </summary>
        private int width = 1280;

        /// <summary>
        /// Current height.
        /// </summary>
        private int height = 720;

        /// <summary>
        /// Virtual width.
        /// </summary>
        private int virtualWidth = 1280;

        /// <summary>
        /// Virtual height.
        /// </summary>
        private int virtualHeight = 720;

        /// <summary>
        /// Scale matrix.
        /// </summary>
        private Matrix scaleMatrix;

        /// <summary>
        /// Fullscreen.
        /// </summary>
        private bool fullscreen/* = false*/;

        /// <summary>
        /// Matrix is dirty.
        /// </summary>
        private bool dirtyMatrix = true;

        /// <summary>
        /// Gets the window width.
        /// </summary>
        public int BaseWidth
        {
            get
            {
                return this.virtualWidth;
            }
        }

        /// <summary>
        /// Gets the window height.
        /// </summary>
        public int BaseHeight
        {
            get
            {
                return this.virtualHeight;
            }
        }

        /// <summary>
        /// Gets the window width.
        /// </summary>
        public int RealWidth
        {
            get
            {
                return this.width;
            }
        }

        /// <summary>
        /// Gets the window height.
        /// </summary>
        public int RealHeight
        {
            get
            {
                return this.height;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Resolution"/> is fullscreen.
        /// </summary>
        /// <value>
        ///   <c>true</c> if fullscreen; otherwise, <c>false</c>.
        /// </value>
        public bool Fullscreen
        {
            get
            {
                return this.fullscreen;
            }
            set
            {
                this.fullscreen = value;
            }
        }

        /// <summary>
        /// Gets or sets the window title string.
        /// </summary>
        public bool Resizable
        {
            get
            {
                return AlmiranteEngine.Application.Window.AllowUserResizing;
            }
            set
            {
                AlmiranteEngine.Application.Window.AllowUserResizing = value;
            }
        }

        /// <summary>
        /// Gets the transformation matrix.
        /// </summary>
        public Matrix Matrix
        {
            get
            {
                if (this.dirtyMatrix)
                {
                    this.RecreateScaleMatrix();
                }
                return this.scaleMatrix;
            }
        }

        /// <summary>
        /// Gets the resolution viewport.
        /// </summary>
        public Vector2 ViewportSize
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Resolution"/> class.
        /// </summary>
        internal Resolution()
        {
        }

        /// <summary>
        /// Initializes the resoltuion manager.
        /// </summary>
        public void Initialize()
        {
            this.width = AlmiranteEngine.Device.Viewport.Width;
            this.height = AlmiranteEngine.Device.Viewport.Height;
            this.dirtyMatrix = true;
            this.ApplyChanges();
        }

        /// <summary>
        /// Sets the resolution.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void SetResolution(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.ApplyChanges();
        }

        /// <summary>
        /// Sets the resolution.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="fullscreen">if set to <c>true</c> [fullscreen].</param>
        public void SetResolution(int width, int height, bool fullscreen)
        {
            this.width = width;
            this.height = height;
            this.fullscreen = fullscreen;
            this.ApplyChanges();
        }

        /// <summary>
        /// Sets the virtual resolution.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void SetBaseResolution(int width, int height)
        {
            this.virtualWidth = width;
            this.virtualHeight = height;
            this.dirtyMatrix = true;
        }

        /// <summary>
        /// Applies the changes.
        /// </summary>
        private void ApplyChanges()
        {
            var dmgr = AlmiranteEngine.DeviceManager;
            if (dmgr != null)
            {
                if (this.fullscreen == false)
                {
                    if ((this.width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                        && (this.height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                    {
                        dmgr.PreferredBackBufferWidth = this.width;
                        dmgr.PreferredBackBufferHeight = this.height;
                        dmgr.IsFullScreen = this.fullscreen;
                        dmgr.ApplyChanges();
                    }
                }
                else
                {
                    foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                    {
                        if ((dm.Width == width) && (dm.Height == height))
                        {
                            dmgr.PreferredBackBufferWidth = this.width;
                            dmgr.PreferredBackBufferHeight = this.height;
                            dmgr.IsFullScreen = this.fullscreen;
                            dmgr.ApplyChanges();
                        }
                    }
                }

                this.dirtyMatrix = true;

                this.width = dmgr.PreferredBackBufferWidth;
                this.height = dmgr.PreferredBackBufferHeight;
            }
        }

        /// <summary>
        /// Recreates the scale matrix.
        /// </summary>
        private void RecreateScaleMatrix()
        {
            dirtyMatrix = false;
            scaleMatrix = Matrix.CreateScale(
                           (float)AlmiranteEngine.Device.Viewport.Width / virtualWidth,
                           (float)AlmiranteEngine.Device.Viewport.Width / virtualWidth,
                           1f);
        }

        /// <summary>
        /// Get virtual Mode Aspect Ratio
        /// </summary>
        /// <returns>aspect ratio</returns>
        private float GetVirtualAspectRatio()
        {
            return (float)virtualWidth / (float)virtualHeight;
        }

        /// <summary>
        /// Fulls the viewport.
        /// </summary>
        internal void ApplyTotalViewport()
        {
            Viewport vp = AlmiranteEngine.Device.Viewport;
            vp.X = vp.Y = 0;
            vp.Width = width;
            vp.Height = height;
            AlmiranteEngine.Device.Viewport = vp;
        }

        /// <summary>
        /// Resets the viewport.
        /// </summary>
        internal void ApplyScaledViewport()
        {
            float targetAspectRatio = GetVirtualAspectRatio();

            int preferredWidth = AlmiranteEngine.Device.Viewport.Width;
            int preferredHeight = AlmiranteEngine.Device.Viewport.Height;

            var dmgr = AlmiranteEngine.DeviceManager;
            if (dmgr != null)
            {
                preferredWidth = AlmiranteEngine.DeviceManager.PreferredBackBufferWidth;
                preferredHeight = AlmiranteEngine.DeviceManager.PreferredBackBufferHeight;
            }

            int width = preferredWidth;
            int height = (int)(width / targetAspectRatio + .5f);

            bool changed = false;

            if (height > preferredHeight)
            {
                height = preferredHeight;
                width = (int)(height * targetAspectRatio + .5f);
                changed = true;
            }

            Viewport viewport = AlmiranteEngine.Device.Viewport;

            viewport.X = (preferredWidth / 2) - (width / 2);
            viewport.Y = (preferredHeight / 2) - (height / 2);
            viewport.Width = width;
            viewport.Height = height;
            viewport.MinDepth = 0;
            viewport.MaxDepth = 1;

            Vector2 size = this.ViewportSize;
            size.X = viewport.X;
            size.Y = viewport.Y;
            this.ViewportSize = size;

            if (changed)
            {
                dirtyMatrix = true;
            }

            AlmiranteEngine.Device.Viewport = viewport;
        }
    }
}