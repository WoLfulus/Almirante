using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
    public static class FontExtensions
    {
        public static Vector2 MeasureText(this SpriteFont font, string text)
        {
            Vector2 size = Vector2.Zero;
            using (StringReader reader = new StringReader(text))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    var temp = font.MeasureString(line);
                    if (temp.X > size.X)
                    {
                        size.X = temp.X;
                    }
                    size.Y += temp.Y;
                }
            }
            return size;
        }
    }
}
