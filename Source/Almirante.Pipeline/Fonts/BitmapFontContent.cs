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
/// Special thanks to Guilherme Moreira (sephirothrx7)
///

namespace Almirante.Pipeline.Fonts
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

    /// <summary>
    /// Hold the data read from a Fonts xml.
    /// </summary>
    [ContentSerializerRuntimeType("Almirante.Engine.Fonts.BitmapFont, Almirante.Engine")]
    public class BitmapFontContent
    {
        /// <summary>
        /// Gets or sets the characters dictionary.
        /// </summary>
        /// <value>
        /// The characters.
        /// </value>
        public Dictionary<char, Rectangle> Characters
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>
        /// The size of the font.
        /// </value>
        public int FontSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the horizontal text padding.
        /// </summary>
        /// <value>
        /// The padding.
        /// </value>
        public int HorizontalGap
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the vertical gap.
        /// </summary>
        /// <value>
        /// The vertical gap.
        /// </value>
        public int VerticalGap
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        public Rectangle Offset
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        /// <value>
        /// The texture.
        /// </value>
        public TextureContent Texture
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the text color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapFontContent"/> class.
        /// </summary>
        public BitmapFontContent()
        {
            Characters = new Dictionary<char, Rectangle>();
            HorizontalGap = 0;
            VerticalGap = 0;
            Offset = new Rectangle();
            Color = Color.White;
        }
    }
}