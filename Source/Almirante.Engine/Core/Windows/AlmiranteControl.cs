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

namespace Almirante.Engine.Core.Windows
{
    using System;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;
    using Almirante.Engine.Scenes;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Design;
    using Microsoft.Xna.Framework.Graphics;
    using Color = System.Drawing.Color;
    using Rectangle = Microsoft.Xna.Framework.Rectangle;

    /// <summary>
    /// Custom control to use Scenes inside a WinForms project.
    /// </summary>
    public class AlmiranteControl<T> : Control
        where T : SceneControl
    {
        /// <summary>
        /// The stop watch
        /// </summary>
        private Stopwatch stopWatch = Stopwatch.StartNew();

        /// <summary>
        /// The target elapsed time
        /// </summary>
        private readonly TimeSpan TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 60);

        /// <summary>
        /// The max elapsed time
        /// </summary>
        private readonly TimeSpan MaxElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 10);

        /// <summary>
        /// The total game time
        /// </summary>
        private TimeSpan accumulatedTime;

        /// <summary>
        /// The elapsed game time
        /// </summary>
        private TimeSpan lastTime;

        /// <summary>
        /// The graphics device service
        /// </summary>
        private AlmiranteGraphicsService graphicsDeviceService;

        /// <summary>
        /// The scene instance.
        /// </summary>
        private T scene;

        /// <summary>
        /// Gets the scene.
        /// </summary>
        /// <value>
        /// The scene.
        /// </value>
        public T Scene
        {
            get
            {
                return this.scene;
            }
        }

        /// <summary>
        /// Gets a GraphicsDevice that can be used to draw onto this control.
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
            get
            {
                return graphicsDeviceService.GraphicsDevice;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlmiranteControl{T}"/> class.
        /// </summary>
        public AlmiranteControl()
        {
            this.Dock = DockStyle.Fill;
            this.scene = AlmiranteEngine.Scenes.Get<T>();
            this.scene.Parent = this;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            this.scene.Activate();
            base.OnGotFocus(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            this.scene.Deactivate();
            base.OnLostFocus(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void OnCreateControl()
        {
            if (!DesignMode)
            {
                this.graphicsDeviceService = AlmiranteGraphicsService.AddRef(Handle, ClientSize.Width, ClientSize.Height);
                AlmiranteEngine.Instance.InitializeWindowsServices();
                this.OnInitialize();
            }

            base.OnCreateControl();
        }

        /// <summary>
        /// Disposes the control.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            this.OnUninitialize();

            if (graphicsDeviceService != null)
            {
                graphicsDeviceService.Release(disposing);
                graphicsDeviceService.Dispose();
                graphicsDeviceService = null;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Redraws the control in response to a WinForms paint message.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            string beginDrawError = BeginDraw();
            if (string.IsNullOrEmpty(beginDrawError))
            {
                this.OnDraw();
                this.EndDraw();
            }
            else
            {
                // If BeginDraw failed, show an error message using System.Drawing.
                PaintUsingSystemDrawing(e.Graphics, beginDrawError);
            }
        }

        /// <summary>
        /// Begins drawing the control contents.
        /// </summary>
        private string BeginDraw()
        {
            // If we have no graphics device, we must be running in the designer.
            if (graphicsDeviceService == null)
            {
                return Text + "\n\n" + GetType();
            }

            // Make sure the graphics device is big enough, and is not lost.
            string deviceResetError = HandleDeviceReset();

            if (!string.IsNullOrEmpty(deviceResetError))
            {
                return deviceResetError;
            }

            Viewport viewport = new Viewport();

            viewport.X = 0;
            viewport.Y = 0;

            viewport.Width = ClientSize.Width;
            viewport.Height = ClientSize.Height;

            viewport.MinDepth = 0;
            viewport.MaxDepth = 1;

            GraphicsDevice.Viewport = viewport;

            return null;
        }

        /// <summary>
        /// Presents the content to the screen.
        /// </summary>
        private void EndDraw()
        {
            try
            {
                Rectangle sourceRectangle = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
                GraphicsDevice.Present(sourceRectangle, null, this.Handle);
            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        /// Checks the device status and resets it if necessary.
        /// </summary>
        private string HandleDeviceReset()
        {
            bool deviceNeedsReset = false;

            switch (GraphicsDevice.GraphicsDeviceStatus)
            {
                case GraphicsDeviceStatus.Lost:
                    return "Graphics device lost";

                case GraphicsDeviceStatus.NotReset:
                    deviceNeedsReset = true;
                    break;

                default:
                    PresentationParameters pp = GraphicsDevice.PresentationParameters;
                    deviceNeedsReset = (ClientSize.Width > pp.BackBufferWidth) || (ClientSize.Height > pp.BackBufferHeight);
                    AlmiranteEngine.Camera.UpdateOrigin(ClientSize.Width, ClientSize.Height);
                    break;
            }

            if (deviceNeedsReset)
            {
                try
                {
                    graphicsDeviceService.ResetDevice(ClientSize.Width, ClientSize.Height);
                }
                catch (Exception e)
                {
                    return "Graphics device reset failed\n\n" + e;
                }
            }

            return null;
        }

        /// <summary>
        /// Renders an error message to the form using GDI.
        /// </summary>
        protected virtual void PaintUsingSystemDrawing(Graphics graphics, string text)
        {
            graphics.Clear(Color.CornflowerBlue);

            using (Brush brush = new SolidBrush(Color.Black))
            {
                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    graphics.DrawString(text, Font, brush, ClientRectangle, format);
                }
            }
        }

        /// <summary>
        /// Ignores original winforms messages.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        /// <summary>
        /// Derived classes override this to initialize their drawing code.
        /// </summary>
        protected void OnInitialize()
        {
            Application.Idle += new EventHandler(OnTick);
            this.scene.Enter();
        }

        /// <summary>
        /// Ticks the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnTick(object sender, EventArgs e)
        {
            TimeSpan currentTime = stopWatch.Elapsed;
            TimeSpan elapsedTime = currentTime - lastTime;
            lastTime = currentTime;

            if (elapsedTime > MaxElapsedTime)
            {
                elapsedTime = MaxElapsedTime;
            }

            accumulatedTime += elapsedTime;

            bool updated = false;
            while (elapsedTime >= TargetElapsedTime)
            {
                OnUpdate();

                elapsedTime -= TargetElapsedTime;
                updated = true;
            }

            //if (updated)
            //{
            Invalidate();
            //}
        }

        /// <summary>
        /// Uninitializes this instance.
        /// </summary>
        protected void OnUninitialize()
        {
            this.scene.Deactivate();
            this.scene.Leave();
        }

        /// <summary>
        /// The game time
        /// </summary>
        private GameTime gameTime = new GameTime();

        /// <summary>
        /// Called when [timer].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnUpdate()
        {
            lock (this.scene)
            {
                MethodInfo method = null;

                method = gameTime.GetType().GetProperty("ElapsedGameTime").GetSetMethod(true);
                method.Invoke(this.gameTime, new object[] { lastTime });

                method = gameTime.GetType().GetProperty("TotalGameTime").GetSetMethod(true);
                method.Invoke(this.gameTime, new object[] { accumulatedTime });

                AlmiranteEngine.Time.Update(gameTime);
                AlmiranteEngine.Input.Update(this.Handle);

                this.scene.Update();
            }
        }

        /// <summary>
        /// Derived classes override this to draw themselves using the GraphicsDevice.
        /// </summary>
        protected void OnDraw()
        {
            lock (this.scene)
            {
                AlmiranteEngine.Device.Clear(this.scene.BackgroundColor);
                this.scene.Draw(AlmiranteEngine.Batch);
            }
        }
    }
}