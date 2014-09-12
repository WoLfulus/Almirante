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
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.Xna.Framework.Content;

    /// <summary>
    /// Content management.
    /// </summary>
    public class ResourceContentManager : ContentManager
    {
        /// <summary>
        /// Stores all loaded resources.
        /// </summary>
        private readonly Dictionary<string, ResourceTracker> resources = new Dictionary<string, ResourceTracker>();

        /// <summary>
        /// Creates a new instance of the <see cref="ResourceContentManager"/> class.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        public ResourceContentManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ResourceContentManager"/> class.
        /// </summary>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="rootDirectory">Root content directory.</param>
        public ResourceContentManager(IServiceProvider serviceProvider, string rootDirectory)
            : base(serviceProvider, rootDirectory)
        {
        }

        /// <summary>
        /// Check whether the specified asset is loaded.
        /// </summary>
        /// <param name="assetPath">Path to the asset.</param>
        /// <returns>Value indicating whether the requested asset is loaded.</returns>
        public bool IsLoaded(string assetPath)
        {
            lock (this)
            {
                return this.resources.ContainsKey(this.Normalize(assetPath));
            }
        }

        /// <summary>
        /// Requests a resource to the content manager.
        /// </summary>
        /// <typeparam name="T">Type of the resource.</typeparam>
        /// <param name="resourcePath">The resource path.</param>
        /// <returns>
        /// The loaded resource.
        /// </returns>
        /// <exception cref="System.Exception">Trying to load the same asset with different types.</exception>
        public override T Load<T>(string resourcePath)
        {
            lock (this)
            {
                ResourceTracker tracker = null;
                string path = this.Normalize(resourcePath);

                if (this.resources.TryGetValue(path, out tracker))
                {
                    if (!(tracker.Content is T))
                    {
                        throw new InvalidOperationException("Trying to load the same asset with different types.");
                    }
                }
                else
                {
                    tracker = new ResourceTracker()
                    {
                        Count = 1,
                        Content = this.ReadAsset<T>(path, null)
                    };

                    try
                    {
                        this.resources.Add(path, tracker);
                    }
                    catch (Exception e)
                    {
                    }
                }

                return (T)tracker.Content;
            }
        }

        /// <summary>
        /// Unloads a single asset from the content manager.
        /// </summary>
        /// <param name="resourcePath">The resource path.</param>
        public void Unload(string resourcePath)
        {
            lock (this)
            {
                ResourceTracker tracker = null;
                string path = this.Normalize(resourcePath);
                if (this.resources.TryGetValue(path, out tracker))
                {
                    tracker.Count--;
                    if (tracker.Count == 0)
                    {
                        IDisposable disposable = tracker.Content as IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                        tracker = null;
                        this.resources.Remove(path);
                    }
                }
            }
        }

        /// <summary>
        /// Unloads all loaded resources and disposes them when possible.
        /// </summary>
        public override void Unload()
        {
            lock (this)
            {
                Dictionary<string, ResourceTracker>.Enumerator enumer = this.resources.GetEnumerator();
                while (enumer.MoveNext())
                {
                    ResourceTracker obj = enumer.Current.Value;
                    IDisposable disposable = obj.Content as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                    obj = null;
                }
                this.resources.Clear();
            }
        }

        /// <summary>
        /// Noramlizes the path to the specified resource.
        /// </summary>
        /// <param name="resourcePath">Resource path.</param>
        /// <returns>Normalized path.</returns>
        private string Normalize(string resourcePath)
        {
            return resourcePath.Replace('/', '\\').ToLower(CultureInfo.CurrentCulture);
        }
    }
}