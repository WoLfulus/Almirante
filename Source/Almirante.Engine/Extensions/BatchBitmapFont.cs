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
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Almirante.Engine.Fonts;

    /// <summary>
    ///
    /// </summary>
    public static partial class BatchExtensions
    {
        /// <summary>
        /// OnDraw text on scene using default alignment (Left)
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <param name="font">The font.</param>
        /// <param name="position">Position to begin drawing</param>
        /// <param name="input">A formated string</param>
        /// <param name="args">Optional parameters for the formated string</param>
        public static void DrawFont(this SpriteBatch batch, BitmapFont font, Vector2 position, string input, params object[] args)
        {
            batch.DrawFont(font, position, BitmapFontAlignment.Left, font.Color, input, args);
        }

        /// <summary>
        /// OnDraw text on scene using default orientation (Left)
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <param name="font">The font.</param>
        /// <param name="position">Position to begin drawing</param>
        /// <param name="color">The color.</param>
        /// <param name="input">A formated string</param>
        /// <param name="args">Optional parameters for the formated string</param>
        public static void DrawFont(this SpriteBatch batch, BitmapFont font, Vector2 position, Color color, string input, params object[] args)
        {
            batch.DrawFont(font, position, BitmapFontAlignment.Left, color, input, args);
        }

        /// <summary>
        /// OnDraw text on scene
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <param name="font">The font.</param>
        /// <param name="position">Position to begin drawing</param>
        /// <param name="alignment">The alignment.</param>
        /// <param name="color">The color.</param>
        /// <param name="input">A formated string</param>
        /// <param name="args">Optional parameters for the formated string</param>
        public static void DrawFont(this SpriteBatch batch, BitmapFont font, Vector2 position, BitmapFontAlignment alignment, Color color, string input, params object[] args)
        {
            batch.DrawFont(font, position, alignment, color, String.Format(CultureInfo.CurrentCulture, input, args));
        }

        /// <summary>
        /// OnDraw text on scene
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <param name="font">The font.</param>
        /// <param name="position">Position to begin drawing</param>
        /// <param name="alignment">The alignment.</param>
        /// <param name="input">A formated string</param>
        /// <param name="args">Optional parameters for the formated string</param>
        public static void DrawFont(this SpriteBatch batch, BitmapFont font, Vector2 position, BitmapFontAlignment alignment, string input, params object[] args)
        {
            batch.DrawFont(font, position, alignment, font.Color, String.Format(CultureInfo.CurrentCulture, input, args));
        }

        /// <summary>
        /// OnDraw text on scene
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <param name="font">The font.</param>
        /// <param name="position">The position.</param>
        /// <param name="alignment">The alignment.</param>
        /// <param name="color">The color.</param>
        /// <param name="input">The input.</param>
        public static void DrawFont(this SpriteBatch batch, BitmapFont font, Vector2 position, BitmapFontAlignment alignment, Color color, string input)
        {
            Vector2 tempPos = Vector2.Zero;
            using (StringReader reader = new StringReader(input))
            {
                tempPos.X = position.X;
                tempPos.Y = position.Y + font.Offset.Y;// -(FontHeight / 2);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    switch (alignment)
                    {
                        case BitmapFontAlignment.Left:
                            tempPos.X += font.Offset.X;
                            break;

                        case BitmapFontAlignment.Center:
                            {
                                var size = font.MeasureString(line);
                                tempPos.X -= (int)(size.X / 2) - font.Offset.X;
                                break;
                            }

                        case BitmapFontAlignment.Right:
                            {
                                var size = font.MeasureString(line);
                                tempPos.X -= size.X - font.Offset.Width;
                                break;
                            }
                        default:
                            // do the defalut action
                            break;
                    }
                    font.DrawLine(batch, tempPos, color, line);
                    tempPos.X = position.X;
                    tempPos.Y += font.FontHeight + font.VerticalGap;
                }
            }
        }
    }
}