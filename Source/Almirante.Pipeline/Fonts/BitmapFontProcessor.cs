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
    using System;
    using System.IO;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content.Pipeline;
    using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
    using TInput = Almirante.Pipeline.Common.CustomXmlContent;
    using TOutput = Almirante.Pipeline.Fonts.BitmapFontContent;

    /// <summary>
    /// Custom content processor for loading data and texture for the bitmapfont.
    /// </summary>
    [ContentProcessor(DisplayName = "BitmapFont Processor - Almirante Games")]
    public class BitmapFontProcessor : ContentProcessor<TInput, TOutput>
    {
        /// <summary>
        /// Processes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            BitmapFontContent bitmap = new BitmapFontContent();
            int fontSize = 0;
            string filename = input.Filename;

            var fontMetric = XElement.Parse(input.Content);
            // for every character node
            foreach (var charElement in fontMetric.Elements("character"))
            {
                // the attribute keycode
                char code = Convert.ToChar((uint)charElement.Attribute("key"));
                // rectangle with position for the key in the Texture
                Rectangle rect = new Rectangle((int)charElement.Element("x"), (int)charElement.Element("y"),
                    (int)charElement.Element("width"), (int)charElement.Element("height"));
                bitmap.Characters.Add(code, rect);

                // workaround to discover the font size as it is not described on the xml
                if (rect.Height > fontSize)
                {
                    fontSize = rect.Height;
                }
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
            bitmap.FontSize = fontSize;

            // Get the working directory for the xml
            string dir = Path.GetDirectoryName(filename);
            // Append the filename with .png extension (contentproj/Fonts/Font.bitmap will load contentproj/Fonts/Font.png)
            string fileTexture = Path.GetFileNameWithoutExtension(filename) + ".png";
            ExternalReference<TextureContent> textureReference = new ExternalReference<TextureContent>(dir + Path.DirectorySeparatorChar + fileTexture);

            // Build and Load the Texture for this font
            bitmap.Texture = context.BuildAndLoadAsset<TextureContent, TextureContent>(textureReference, "TextureProcessor");
            return bitmap;
        }
    }
}