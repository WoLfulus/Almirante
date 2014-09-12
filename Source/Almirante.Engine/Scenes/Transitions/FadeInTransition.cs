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

namespace Almirante.Engine.Scenes.Transitions
{
    using Almirante.Engine.Scenes;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Scene transition.
    /// </summary>
    [Transition(Name)]
    public class FadeInTransition : Transition
    {
        /// <summary>
        /// Transition name.
        /// </summary>
        public const string Name = "fade-in";

        /// <summary>
        /// Stores the fade alpha color.
        /// </summary>
        private Color colorIn;

        /// <summary>
        /// Initializes the resources.
        /// </summary>
        protected override void OnInitialize()
        {
        }

        /// <summary>
        /// Updates the fade effect.
        /// </summary>
        /// <param name="progress">Transition progress.</param>
        protected override void OnUpdate(float progress)
        {
            var alpha = progress;
            this.colorIn = Color.FromNonPremultiplied(255, 255, 255, (int)(255.0f * alpha));
        }

        /// <summary>
        /// Draws the transition
        /// </summary>
        /// <param name="batch">Sprite batch.</param>
        /// <param name="bottom">The bottom.</param>
        /// <param name="top">The top.</param>
        protected override void OnDraw(SpriteBatch batch, Texture2D bottom, Texture2D top)
        {
            batch.Start();
            batch.Draw(bottom, Vector2.Zero, Color.White);
            batch.Draw(top, Vector2.Zero, this.colorIn);
            batch.End();
        }
    }
}