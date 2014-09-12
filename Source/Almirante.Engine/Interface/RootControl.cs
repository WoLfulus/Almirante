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

namespace Almirante.Engine.Interface
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Root interface control.
    /// </summary>
    public sealed class RootControl : Control
    {
        /// <summary>
        /// Stores all hud objects.
        /// </summary>
        private readonly List<Display> huds = new List<Display>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public RootControl()
            : base()
        {
        }

        /// <summary>
        /// Creates a HUD into the scene.
        /// </summary>
        /// <typeparam name="T">Type of the display.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// Same instance passed as argument.
        /// </returns>
        public T AddDisplay<T>(T instance)
            where T : Display
        {
            instance.Parent = this.Parent;
            instance.Visible = true;
            instance.Initialize();
            this.huds.Add(instance);
            return instance;
        }

        /// <summary>
        /// Control update.
        /// </summary>
        internal override void Update()
        {
            foreach (var hud in this.huds)
            {
                hud.Update();
            }

            base.Update();
        }

        /// <summary>
        /// Control draw.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="position"></param>
        internal override void Draw(SpriteBatch batch, Vector2 position)
        {
            batch.Start(false, SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            foreach (var hud in this.huds)
            {
                if (hud.Visible)
                {
                    hud.Draw(batch);
                }
            }
            batch.End();

            batch.Start(false, SpriteSortMode.Deferred, BlendState.AlphaBlend);
            base.Draw(batch, position);
            batch.End();
        }
    }
}