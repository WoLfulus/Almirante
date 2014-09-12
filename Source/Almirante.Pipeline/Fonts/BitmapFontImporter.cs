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
    using System.IO;
    using Almirante.Pipeline.Common;
    using Microsoft.Xna.Framework.Content.Pipeline;
    using TImport = Almirante.Pipeline.Common.CustomXmlContent;

    /// <summary>
    /// Custom content importer for loading .bitmapfont font files.
    /// </summary>
    [ContentImporter(".bitmapfont", DisplayName = "BitmapFont Importer - Almirante Engine", DefaultProcessor = "BitmapFontProcessor")]
    public class BitmapFontImporter : ContentImporter<TImport>
    {
        /// <summary>
        /// Imports the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override TImport Import(string filename, ContentImporterContext context)
        {
            return new CustomXmlContent(filename, File.ReadAllText(filename));
        }
    }
}