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

namespace Microsoft.Xna.Framework.Graphics
{
    using System;
    using System.Collections.Generic;
    using Almirante.Engine.Core;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// SpriteBatch extension methods
    /// </summary>
    public static partial class BatchExtensions
    {
        /// <summary>
        /// Starts the specified batch.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <param name="useCamera">if set to <c>true</c> camera matrix will be applied.</param>
        /// <param name="sortMode">The sort mode.</param>
        /// <param name="blendState">State of the blend.</param>
        /// <param name="samplerState">State of the sampler.</param>
        /// <param name="depthStencilState">State of the depth stencil.</param>
        /// <param name="rasterizerState">State of the rasterizer.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="transform">The transformation matrix.</param>
        public static void Start(this SpriteBatch batch,
            bool useCamera = false,
            SpriteSortMode sortMode = SpriteSortMode.Deferred,
            BlendState blendState = null,
            SamplerState samplerState = null,
            DepthStencilState depthStencilState = null,
            RasterizerState rasterizerState = null,
            Effect effect = null,
            Matrix? transform = null)
        {
            Matrix matrix = AlmiranteEngine.IsWinForms ? Matrix.Identity : AlmiranteEngine.Settings.Resolution.Matrix;

            if (useCamera)
            {
                matrix = AlmiranteEngine.Camera.Matrix * matrix;
            }

            if (transform.HasValue)
            {
                matrix = transform.Value * matrix;
            }

            batch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, matrix);
        }
    }
}