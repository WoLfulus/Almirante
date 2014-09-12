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

namespace Almirante.Entities.Components
{
    using Almirante.Entities.Types;

    /// <summary>
    /// Component class.
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// Gets the component info information.
        /// </summary>
        public TypeInfo Type
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the owner of this component instance.
        /// </summary>
        public Entity Owner
        {
            get;
            internal set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Component"/> class.
        /// </summary>
        public Component()
        {
            this.Type = ComponentHelper.GetInfo(this.GetType()).Value;
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        internal void Create()
        {
            this.OnCreate();
        }

        /// <summary>
        /// Destroys the component.
        /// </summary>
        internal void Destroy()
        {
            this.OnDestroy();
        }

        /// <summary>
        /// Called when component is being created.
        /// </summary>
        public virtual void OnCreate()
        {
        }

        /// <summary>
        /// Called when component gets destroyed.
        /// </summary>
        public virtual void OnDestroy()
        {
        }
    }
}