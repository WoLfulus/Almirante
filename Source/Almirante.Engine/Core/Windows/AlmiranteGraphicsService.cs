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
    using System.Threading;
    using Microsoft.Xna.Framework.Graphics;

    // The IGraphicsDeviceService interface requires a DeviceCreated event, but we
    // always just create the device inside our constructor, so we have no place to
    // raise that event. The C# compiler warns us that the event is never used, but
    // we don't care so we just disable this warning.
#pragma warning disable 67

    /// <summary>
    /// Helper class responsible for creating and managing the GraphicsDevice.
    /// All AlmiranteControl instances share the same WindowsGraphicsDevice,
    /// so even though there can be many controls, there will only ever be a single
    /// underlying GraphicsDevice. This implements the standard IGraphicsDeviceService
    /// interface, which provides notification events for when the device is reset
    /// or disposed.
    /// </summary>
    internal class AlmiranteGraphicsService : IGraphicsDeviceService, IDisposable
    {
        /// <summary>
        /// The reference count
        /// </summary>
        private static int referenceCount;

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static AlmiranteGraphicsService singletonInstance;

        /// <summary>
        /// The graphics device
        /// </summary>
        private GraphicsDevice graphicsDevice;

        /// <summary>
        /// The parameters
        /// </summary>
        private PresentationParameters parameters;

        /// <summary>
        /// Occurs when [device created].
        /// </summary>
        public event EventHandler<EventArgs> DeviceCreated;

        /// <summary>
        /// Occurs when [device disposing].
        /// </summary>
        public event EventHandler<EventArgs> DeviceDisposing;

        /// <summary>
        /// Occurs when [device reset].
        /// </summary>
        public event EventHandler<EventArgs> DeviceReset;

        /// <summary>
        /// Occurs when [device resetting].
        /// </summary>
        public event EventHandler<EventArgs> DeviceResetting;

        /// <summary>
        /// Retrieves a graphcs device.
        /// </summary>
        GraphicsDevice IGraphicsDeviceService.GraphicsDevice
        {
            get
            {
                return this.graphicsDevice;
            }
        }

        /// <summary>
        /// Gets the current graphics device.
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
            get
            {
                return graphicsDevice;
            }
        }

        /// <summary>
        /// Initializes a new instance of the WindowsGraphicsDevice class.
        /// Constructor is private, because this is a singleton class:
        /// client controls should use the public AddRef method instead.
        /// </summary>
        /// <param name="windowHandle">The window handle.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        private AlmiranteGraphicsService(IntPtr windowHandle, int width, int height)
        {
            this.parameters = new PresentationParameters();

            this.parameters.BackBufferWidth = Math.Max(width, 1);
            this.parameters.BackBufferHeight = Math.Max(height, 1);
            this.parameters.BackBufferFormat = SurfaceFormat.Color;

            this.parameters.DepthStencilFormat = DepthFormat.Depth24;
            this.parameters.DeviceWindowHandle = windowHandle;
            this.parameters.IsFullScreen = false;

            try
            {
                graphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.HiDef, parameters);
                AlmiranteEngine.Device = graphicsDevice;
                AlmiranteEngine.Batch = new SpriteBatch(graphicsDevice);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Gets a reference to the singleton instance.
        /// </summary>
        /// <param name="windowHandle">The window handle.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static AlmiranteGraphicsService AddRef(IntPtr windowHandle, int width, int height)
        {
            if (Interlocked.Increment(ref referenceCount) == 1)
            {
                singletonInstance = new AlmiranteGraphicsService(windowHandle, width, height);
            }

            return singletonInstance;
        }

        /// <summary>
        /// Releases a reference to the singleton instance.
        /// </summary>
        /// <param name="disposing">if set to <c>true</c> [disposing].</param>
        public void Release(bool disposing)
        {
            if (Interlocked.Decrement(ref referenceCount) == 0)
            {
                if (disposing)
                {
                    if (this.DeviceDisposing != null)
                    {
                        this.DeviceDisposing(this, EventArgs.Empty);
                    }

                    if (this.graphicsDevice != null)
                    {
                        this.graphicsDevice.Dispose();
                    }
                }

                AlmiranteEngine.Device = null;
                this.graphicsDevice = null;
            }
        }

        /// <summary>
        /// Resets the graphics device to whichever is bigger out of the specified
        /// resolution or its current size. This behavior means the device will
        /// demand-grow to the largest of all its AlmiranteControl clients.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void ResetDevice(int width, int height)
        {
            if (this.DeviceResetting != null)
            {
                this.DeviceResetting(this, EventArgs.Empty);
            }

            this.parameters.BackBufferWidth = Math.Max(parameters.BackBufferWidth, width);
            this.parameters.BackBufferHeight = Math.Max(parameters.BackBufferHeight, height);

            this.graphicsDevice.Reset(parameters);

            if (DeviceReset != null)
            {
                DeviceReset(this, EventArgs.Empty);
            }
        }

        #region IDisposable Implementation

        /// <summary>
        /// The disposed
        /// </summary>
        protected bool disposed/* = false*/;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                // Do nothing if the object has already been disposed of.
                if (disposed)
                    return;

                if (disposing)
                {
                    singletonInstance = null;

                    if (graphicsDevice != null)
                        graphicsDevice.Dispose();
                }

                // Release unmanaged resources here. Don't access reference type fields.

                // Remember that the object has been disposed of.
                disposed = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Implementation
    }
}