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
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    ///
    /// </summary>
    public static class TextureExtensions
    {
        /// <summary>
        /// Premultiplies the alpha.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="colorKey">The color key.</param>
        public static void PremultiplyAlpha(this Texture2D texture, Color? colorKey = null)
        {
            if (texture != null)
            {
                Color[] data = new Color[texture.Width * texture.Height];
                texture.GetData<Color>(data, 0, data.Length);
                if (colorKey.HasValue)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (data[i] == colorKey)
                        {
                            data[i] = Color.Transparent;
                        }
                        else
                        {
                            data[i] = new Color(new Vector4(data[i].ToVector3() * (data[i].A / 255f), (data[i].A / 255f)));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = new Color(new Vector4(data[i].ToVector3() * (data[i].A / 255f), (data[i].A / 255f)));
                    }
                }

                texture.SetData<Color>(data, 0, data.Length);
            }
        }
    }
}