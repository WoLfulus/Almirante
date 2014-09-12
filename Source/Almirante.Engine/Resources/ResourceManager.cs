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

namespace Almirante.Engine.Resources
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Almirante.Engine.Core;
    using Almirante.Engine.Fonts;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Resource management.
    /// </summary>
    public class ResourceManager : IDisposable
    {
        /// <summary>
        /// Resource load result.
        /// </summary>
        /// <typeparam name="T">Type of resource.</typeparam>
        /// <param name="resource">The resource instance.</param>
        /// <param name="success">if set to <c>true</c> the resource loaded successfully.</param>
        /// <param name="tag">The tag object.</param>
        public delegate void LoadResult<T>(Resource<T> resource, bool success, object tag);

        /// <summary>
        /// Stores a reference to the content manager.
        /// </summary>
        private readonly ResourceContentManager contents;

        /// <summary>
        /// Stores the default font.
        /// </summary>
        private Resource<BitmapFont> font = null;

        /// <summary>
        /// Stores the default texture.
        /// </summary>
        private Resource<Texture2D> background = null;

        /// <summary>
        /// Creates a new instance of the <see cref="ResourceManager"/> class.
        /// </summary>
        /// <param name="contents"></param>
        internal ResourceManager(ResourceContentManager contents)
        {
            this.PendingResources = 0;
            this.contents = contents;
            this.Delay = 0;
        }

        /// <summary>
        /// Gets the default spritefont.
        /// </summary>
        public BitmapFont DefaultFont
        {
            get
            {
                if (this.font == null)
                {
                    this.font = this.LoadSync<BitmapFont>("Common\\Fonts\\Default");
                }
                return this.font.Content;
            }
        }

        /// <summary>
        /// Gets the default texture.
        /// </summary>
        public Texture2D DefaultBackground
        {
            get
            {
                if (this.background == null)
                {
                    this.background = this.LoadSync<Texture2D>("Common\\Textures\\Background");
                }
                return this.background.Content;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the manager is loading contents.
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return this.PendingResources > 0;
            }
        }

        /// <summary>
        /// Gets how many resource are pending to load.
        /// </summary>
        public int PendingResources
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a delay for each asset being loaded in milliseconds.
        /// Useful for debugging load screens.
        /// </summary>
        public int Delay
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a texture.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="color">The color.</param>
        /// <returns>Newly created texture.</returns>
        public Texture2D CreateTexture(int width, int height, Color color)
        {
            var texture = new Texture2D(AlmiranteEngine.Device, width, height);
            texture.SetData(Enumerable.Repeat<Color>(color, width * height).ToArray());
            return texture;
        }

        /// <summary>
        /// Loads a raw texture from the disk.
        /// </summary>
        /// <param name="file">The file path.</param>
        /// <param name="colorKey">The color key. Used only in winforms control.</param>
        /// <returns>
        /// Texture instance.
        /// </returns>
        public Texture2D LoadTexture(string file, Color? colorKey = null)
        {
            try
            {
                using (var stream = File.Open(file, FileMode.Open))
                {
                    var texture = Texture2D.FromStream(AlmiranteEngine.Device, stream);
                    if (AlmiranteEngine.IsWinForms)
                    {
                        texture.PremultiplyAlpha(colorKey);
                    }
                    return texture;
                }
            }
            catch (System.Exception)
            {
            }
            return null;
        }

        /// <summary>
        /// Loads a resource asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the resource.</typeparam>
        /// <param name="path">Path to the resource.</param>
        /// <returns>
        /// Resource tracker.
        /// </returns>
        public Resource<T> LoadAsync<T>(string path)
        {
            return this.Load<T>(path, null, null, true);
        }

        /// <summary>
        /// Loads a resource asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the resource.</typeparam>
        /// <param name="path">Path to the resource.</param>
        /// <param name="change">The change.</param>
        /// <returns>
        /// Resource tracker.
        /// </returns>
        public Resource<T> LoadAsync<T>(string path, LoadResult<T> change)
        {
            return this.Load<T>(path, null, change, true);
        }

        /// <summary>
        /// Loads a resource asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the resource.</typeparam>
        /// <param name="path">Path to the resource.</param>
        /// <param name="tag">User object.</param>
        /// <returns>
        /// Resource tracker.
        /// </returns>
        public Resource<T> LoadAsync<T>(string path, object tag)
        {
            return this.Load<T>(path, tag, null, true);
        }

        /// <summary>
        /// Loads a resource asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the resource.</typeparam>
        /// <param name="path">Path to the resource.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="change">The change.</param>
        /// <returns>
        /// Resource tracker.
        /// </returns>
        public Resource<T> LoadAsync<T>(string path, object tag, LoadResult<T> change)
        {
            return this.Load<T>(path, tag, change, true);
        }

        /// <summary>
        /// Requests a new resource to the resource manager.
        /// </summary>
        /// <typeparam name="T">Type of the resource.</typeparam>
        /// <param name="path">The path.</param>
        /// <returns>
        /// Resource tracker
        /// </returns>
        public Resource<T> LoadSync<T>(string path)
        {
            return this.Load<T>(path, null, null, false);
        }

        /// <summary>
        /// Loads a resource asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of the resource to load.</typeparam>
        /// <param name="path">Path to the resource.</param>
        /// <param name="tag">The tag (can be anything).</param>
        /// <param name="change">The event callback.</param>
        /// <param name="async">If set to <c>true</c> the load will occur asynchronously.</param>
        /// <returns>
        /// Resource tracker.
        /// </returns>
        private Resource<T> Load<T>(string path, object tag, LoadResult<T> change, bool async)
        {
            Resource<T> resource = new Resource<T>(this, path);
            if (async)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback((res) =>
                {
                    lock (this)
                    {
                        Resource<T> r = res as Resource<T>;

                        try
                        {
                            var content = this.contents.Load<T>(r.Path);
                            if (this.Delay != 0)
                            {
                                Thread.Sleep(this.Delay);
                            }

                            r.OnChange(true, tag, content);

                            if (change != null)
                            {
                                change(r, true, tag);
                            }
                        }
                        catch (System.Exception)
                        {
                            r.OnChange(false, tag, default(T));

                            if (change != null)
                            {
                                change(r, false, tag);
                            }
                        }

                        this.PendingResources--;
                    }
                }), resource);

                lock (this)
                {
                    this.PendingResources++;
                }
            }
            else
            {
                try
                {
                    var content = this.contents.Load<T>(resource.Path);
                    resource.OnChange(true, tag, content);

                    if (change != null)
                    {
                        change(resource, true, tag);
                    }
                }
                catch (System.Exception)
                {
                    resource.OnChange(false, tag, default(T));

                    if (change != null)
                    {
                        change(resource, false, tag);
                    }

                    throw;
                }
            }

            return resource;
        }

        /// <summary>
        /// Requests the content manager to unload a asset.
        /// </summary>
        /// <param name="path">Path to the resource that must be unloaded.</param>
        internal void Unload(string path)
        {
            this.contents.Unload(path);
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
                    // Release disposable objects used by this instance here.

                    if (contents != null)
                        contents.Dispose();
                    if (font != null)
                        font.Dispose();
                    if (background != null)
                        background.Dispose();
                }

                // Release unmanaged resources here. Don't access reference type fields.

                // Remember that the object has been disposed of.
                disposed = true;
            }
        }

        public virtual void Dispose()
        {
            Dispose(true);
            // Unregister object for finalization.
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Implementation
    }
}