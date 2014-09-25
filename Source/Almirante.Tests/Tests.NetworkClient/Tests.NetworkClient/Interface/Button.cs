using Almirante.Engine.Core;
using Almirante.Engine.Fonts;
using Almirante.Engine.Interface;
using Almirante.Engine.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.NetworkClient.Interface
{
    /// <summary>
    /// Button
    /// </summary>
    public class Button : Control
    {
        /// <summary>
        /// Font
        /// </summary>
        private Resource<BitmapFont> font;

        /// <summary>
        /// Background texture.
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Color.
        /// </summary>
        private Color color;

        /// <summary>
        /// Gets or sets the button text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        public Button()
        {
            this.Text = "";
            var device = AlmiranteEngine.Device;
            this.texture = new Texture2D(device, 1, 1);
            this.texture.SetData(new Color[] { Color.FromNonPremultiplied(128, 128, 128, 255) });
            this.font = AlmiranteEngine.Resources.LoadSync<BitmapFont>("Fonts\\Pixel16");
            this.color = Color.White * 0.5f;
        }

        /// <summary>
        /// Called when drawing the HUD component.
        /// </summary>
        /// <param name="batch">The sprite batch instance.</param>
        /// <param name="position"></param>
        protected override void OnDraw(SpriteBatch batch, Vector2 position)
        {
            if (this.Visible)
            {
                // Background
                batch.Draw(this.texture, new Rectangle((int)position.X, (int)position.Y, (int)this.Size.X, (int)this.Size.Y), this.color);

                // Borders
                batch.DrawLine(position, position + new Vector2(this.Size.X, 0), Color.Black); // topl -> topr
                batch.DrawLine(position + new Vector2(this.Size.X, 0), position + new Vector2(this.Size.X, this.Size.Y), Color.Black); // topr -> botr
                batch.DrawLine(position + new Vector2(this.Size.X, this.Size.Y), position + new Vector2(0, this.Size.Y), Color.Black); // botr -> botl
                batch.DrawLine(position + new Vector2(0, this.Size.Y), position, Color.Black); // botl -> topl

                // Font
                var size = this.font.Content.MeasureString(this.Text);
                batch.DrawFont(this.font.Content, position + (this.Size / 2) - new Vector2(0, size.Y / 2), FontAlignment.Center, Color.White, this.Text);
            }
        }

        /// <summary>
        /// Called when a mouse button is released.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Almirante.Interfaces.MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseUp(object sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when a mouse button is pressed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected override void OnMouseDown(object sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when mouse leaves the control region.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnMouseLeave(object sender, EventArgs e)
        {
            this.color = Color.White * 0.5f;
        }

        /// <summary>
        /// Called when mouse enters the control region.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnMouseEnter(object sender, EventArgs e)
        {
            this.color = Color.White * 0.75f;
        }

        /// <summary>
        /// Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnEnter(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnLeave(object sender, EventArgs e)
        {
        }
    }
}
