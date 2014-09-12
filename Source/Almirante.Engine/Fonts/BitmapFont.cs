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

namespace Almirante.Engine.Fonts
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using Almirante.Engine.Core;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Globalization;

    /// <summary>
    /// Bitmap font class.
    /// </summary>
    public class BitmapFont
    {
        /// <summary>
        /// A single instance to be used for calculating text size
        /// </summary>
        private Vector2 tempSize = new Vector2();

        /// <summary>
        /// A single instance to be used for text positioning
        /// </summary>
        private Vector2 tempPos = new Vector2();

        /// <summary>
        /// Dictionary where the key is a char and the value
        /// is the Rectangle which contains the position and size
        /// for that char in the Texture
        /// </summary>
        public Dictionary<char, Rectangle> Characters
        {
            get;
            set;
        }

        /// <summary>
        /// Font size
        /// </summary>
        public int FontHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the horizontal gap between every character
        /// </summary>
        /// <value>
        /// The horizontal gap.
        /// </value>
        public int HorizontalGap
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the vertical gap between every character
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
        /// <value>
        /// The offset.
        /// </value>
        public Rectangle Offset
        {
            get;
            set;
        }

        /// <summary>
        /// Texture containing all characters
        /// </summary>
        public Texture2D Texture
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the text color (default is Color.White).
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
        /// Initializes a new instance of the <see cref="BitmapFont"/> class.
        /// </summary>
        public BitmapFont()
        {
            this.Characters = new Dictionary<char, Rectangle>();
            this.HorizontalGap = 0;
            this.VerticalGap = 0;
            this.Offset = new Rectangle();
            this.Color = Color.White;
        }

        /// <summary>
        /// OnDraw the input string in the scene.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <param name="position">Adjusted position to start rendering</param>
        /// <param name="color">The color.</param>
        /// <param name="input">Pre-formated string line</param>
        internal void DrawLine(SpriteBatch batch, Vector2 position, Color color, string input)
        {
            int length = input.Length;
            for (int i = 0; i < length; i++)
            {
                Rectangle rect = Characters[input[i]];
                batch.Draw(Texture, position, rect, color);
                position.X += rect.Width + HorizontalGap;
            }
        }

        /// <summary>
        /// Calculate the amount of space the given string will occupy.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>
        /// Vector2 containing the width and height
        /// </returns>
        public Vector2 MeasureString(string input)
        {
            Vector2 size = new Vector2(Offset.X, Offset.Y);

            string line = "";
            using (StringReader reader = new StringReader(input))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    float width = 0.0f;
                    float height = 0.0f;

                    int length = line.Length;
                    for (int i = 0; i < length; i++)
                    {
                        char c = line[i];
                        width += this.Characters[c].Width + HorizontalGap;

                        if (height < this.Characters[c].Height)
                        {
                            height = this.Characters[c].Height;
                        }
                    }

                    if (size.X < width)
                    {
                        size.X = width;
                    }

                    size.Y += height + VerticalGap;
                }
            }
            size.Y += Offset.Height - VerticalGap;
            tempSize = size;
            return size;
        }

        /// <summary>
        /// Froms the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static BitmapFont FromFile(string path)
        {
            BitmapFont bitmap = new BitmapFont();
            bitmap.Color = Color.White;

            string dir = Path.GetDirectoryName(path);
            string texturePath = Path.Combine(dir, Path.GetFileNameWithoutExtension(path) + ".png");

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path + " not found.");
            }
            else if (!File.Exists(texturePath))
            {
                throw new FileNotFoundException(texturePath + " not found.");
            }

            var fontMetric = XElement.Parse(File.ReadAllText(path));

            foreach (var charElement in fontMetric.Elements("character"))
            {
                char code = Convert.ToChar((uint)charElement.Attribute("key"));

                Rectangle rect = new Rectangle(
                    (int)charElement.Element("x"), (int)charElement.Element("y"),
                    (int)charElement.Element("width"), (int)charElement.Element("height")
                );

                bitmap.Characters.Add(code, rect);
            }

            var hgap = fontMetric.Element("hgap");
            if (hgap != null)
            {
                bitmap.HorizontalGap = Convert.ToInt32(hgap.Value);
            }

            var vgap = fontMetric.Element("vgap");
            if (vgap != null)
            {
                bitmap.VerticalGap = Convert.ToInt32(vgap.Value);
            }

            int x = 0, y = 0, w = 0, h = 0;
            var offleft = fontMetric.Element("offleft");
            if (offleft != null)
            {
                x = Convert.ToInt32(offleft.Value);
            }

            var offtop = fontMetric.Element("offtop");
            if (offtop != null)
            {
                y = Convert.ToInt32(offtop.Value);
            }
            var offright = fontMetric.Element("offright");
            if (offright != null)
            {
                w = Convert.ToInt32(offright.Value);
            }

            var offbottom = fontMetric.Element("offbottom");
            if (offbottom != null)
            {
                h = Convert.ToInt32(offbottom.Value);
            }
            bitmap.Offset = new Rectangle(x, y, w, h);

            bitmap.Texture = AlmiranteEngine.Resources.LoadTexture(texturePath);
            return bitmap;
        }
    }
}