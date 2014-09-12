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

    /// <summary>
    /// Resource holder.
    /// </summary>
    /// <typeparam name="T">Type of the resource.</typeparam>
    public class Resource<T> : IDisposable
    {
        /// <summary>
        /// Resource changed callback.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="tag">User object.</param>
        public delegate void ResourceChanged(Resource<T> sender, object tag);

        /// <summary>
        /// Resource loaded event.
        /// </summary>
        public event ResourceChanged Change;

        /// <summary>
        /// Stores a reference to the resource manager.
        /// </summary>
        private readonly ResourceManager manager;

        /// <summary>
        /// Stores if the current resource has been disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Gets the current content.
        /// </summary>
        public T Content
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the current content path.
        /// </summary>
        public string Path
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets a value indicating whether the resource is loaded.
        /// </summary>
        public bool Loaded
        {
            get;
            internal set;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Resource&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="path">The path.</param>
        public Resource(ResourceManager manager, string path)
        {
            this.manager = manager;
            this.Content = default(T);
            this.Path = path;
        }

        /*

        /// <summary>
        /// Initializes a new instance of the <see cref="Resource{T}"/> class.
        /// </summary>
        /// <param name="other">The other.</param>
        public Resource(Resource<T> other)
        {
            this.manager = other.manager;
            this.Content = other.Content;
            this.Path = other.Path;
            this.Loaded = other.Loaded;
        }
        */

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="Resource&lt;T&gt;"/> is reclaimed by garbage collection.
        /// </summary>
        ~Resource()
        {
            this.Dispose();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected void Dispose(bool disposing)
        {
            lock (this)
            {
                // Do nothing if the object has already been disposed of.
                if (disposed)
                    return;

                if (disposing)
                {
                    // Release disposable objects used by this instance here.

                    if (this.Loaded)
                    {
                        this.manager.Unload(this.Path);
                    }
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispatches the loaded event.
        /// </summary>
        internal void OnChange(bool success, object tag, T content)
        {
            this.Loaded = success;
            this.Content = content;
            if (this.Change != null)
            {
                this.Change(this, tag);
                if (this.disposed)
                {
                    this.manager.Unload(this.Path);
                }
            }
        }
    }
}