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

namespace Almirante.Engine.Scenes
{
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Scene transition class.
    /// </summary>
    public abstract class Transition
    {
        /// <summary>
        /// Initializes the transition resources.
        /// </summary>
        internal void Initialize()
        {
            this.OnInitialize();
        }

        /// <summary>
        /// Initializes the transition resources.
        /// </summary>
        protected virtual void OnInitialize()
        {
        }

        /// <summary>
        /// Uninitializes the transition resources.
        /// </summary>
        internal void Uninitialize()
        {
            this.OnUninitialize();
        }

        /// <summary>
        /// Uninitializes the transition resources.
        /// </summary>
        protected virtual void OnUninitialize()
        {
        }

        /// <summary>
        /// Starts the transition.
        /// </summary>
        internal void Start()
        {
            this.OnStart();
        }

        /// <summary>
        /// Starts the transition state.
        /// </summary>
        protected virtual void OnStart()
        {
        }

        /// <summary>
        /// Completes the transition.
        /// </summary>
        internal void Complete()
        {
            this.OnComplete();
        }

        /// <summary>
        /// Completes the transition state.
        /// </summary>
        protected virtual void OnComplete()
        {
        }

        /// <summary>
        /// Updates the transition.
        /// </summary>
        /// <param name="progress">Transition progress (ranges from 0.0f to 1.0f)</param>
        internal void Update(float progress)
        {
            this.OnUpdate(progress);
        }

        /// <summary>
        /// Updates the transition state.
        /// </summary>
        /// <param name="progress">Transition progress (ranges from 0.0f to 1.0f)</param>
        protected abstract void OnUpdate(float progress);

        /// <summary>
        /// Draws the transition.
        /// </summary>
        /// <param name="batch">Sprite batch.</param>
        /// <param name="bottom">The bottom.</param>
        /// <param name="top">The top.</param>
        internal void Draw(SpriteBatch batch, Texture2D bottom, Texture2D top)
        {
            this.OnDraw(batch, bottom, top);
        }

        /// <summary>
        /// Draws the transition state.
        /// </summary>
        /// <param name="batch">Sprite batch.</param>
        /// <param name="bottom">The bottom.</param>
        /// <param name="top">The top.</param>
        protected virtual void OnDraw(SpriteBatch batch, Texture2D bottom, Texture2D top)
        {
        }
    }
}