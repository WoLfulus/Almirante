using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Almirante.Engine.Core;
using Almirante.Engine.Interface;
using Almirante.Engine.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tests.Gui.Screens.Controls
{
    /// <summary>
    /// Text scroller component.
    /// </summary>
    public class TextScroller : Control
    {
        /// <summary>
        /// Stores the time.
        /// </summary>
        private double time;

        /// <summary>
        /// Stores the control strings.
        /// </summary>
        private readonly StringBuilder strings = new StringBuilder();

        /// <summary>
        /// Stores a reference to the resource manager.
        /// </summary>
        private ResourceManager resources;

        /// <summary>
        /// Background texture.
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextScroller"/> class.
        /// </summary>
        public TextScroller()
        {
            this.resources = AlmiranteEngine.Resources;
            this.texture = new Texture2D(AlmiranteEngine.Device, 1, 1);
            this.texture.SetData(new Color[] { Color.FromNonPremultiplied(128, 128, 128, 128) });
        }

        /// <summary>
        /// Writes the specified message to the scroller.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Write(string message)
        {
            this.strings.AppendLine(message);
        }

        /// <summary>
        /// Called when updating the HUD component.
        /// </summary>
        protected override void OnUpdate()
        {
            this.time += AlmiranteEngine.Time.Frame;
            if (this.time >= 0.25f)
            {
                this.time = time - 0.25f;
                int index = this.strings.ToString().IndexOf(System.Environment.NewLine);
                if (index >= 0)
                {
                    this.strings.Remove(0, index + System.Environment.NewLine.Length);
                }
            }
        }

        /// <summary>
        /// Called when [draw].
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <param name="position">The position.</param>
        protected override void OnDraw(SpriteBatch batch, Vector2 position)
        {
            var size = resources.DefaultFont.MeasureString(this.strings.ToString());
            size += new Vector2(20, 20);

            if (size.Y < this.Size.Y)
            {
                size.Y = this.Size.Y;
            }

            if (size.X < this.Size.X)
            {
                size.X = this.Size.X;
            }

            if (this.Active)
            {
                batch.Draw(this.texture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), Color.Blue);
            }
            else
            {
                batch.Draw(this.texture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), Color.White);
            }

            batch.DrawFont(resources.DefaultFont, position + new Vector2(10, 10), this.strings.ToString());
        }
    }
}