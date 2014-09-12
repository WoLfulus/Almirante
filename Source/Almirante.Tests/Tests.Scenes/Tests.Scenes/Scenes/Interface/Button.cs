// -----------------------------------------------------------------------
// <copyright file="Button.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Tests.Scenes.Scenes.Interface
{
    using System;
    using Almirante.Engine.Core;
    using Almirante.Engine.Fonts;
    using Almirante.Engine.Interface;
    using Almirante.Engine.Resources;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Button control class.
    /// </summary>
    public class Button : Control
    {
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
            var device = AlmiranteEngine.Device;
            this.texture = new Texture2D(device, 1, 1);
            this.texture.SetData(new Color[] { Color.FromNonPremultiplied(128, 128, 128, 255) });
            this.color = Color.Green;
        }

        /// <summary>
        /// Called when drawing the HUD component.
        /// </summary>
        /// <param name="batch">The sprite batch instance.</param>
        /// <param name="position"></param>
        protected override void OnDraw(SpriteBatch batch, Vector2 position)
        {
            var resources = AlmiranteEngine.Resources;

            var textSize = resources.DefaultFont.MeasureString(this.Text);
            batch.Draw(this.texture, new Rectangle((int)position.X, (int)position.Y, (int)this.Size.X, (int)this.Size.Y), this.color);

            var textPosition = position + ((this.Size / 2) - (textSize / 2));
            batch.DrawFont(resources.DefaultFont, textPosition, BitmapFontAlignment.Left, Color.White, this.Text);
        }

        /// <summary>
        /// Called when a mouse button is released.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Almirante.Interfaces.MouseEventArgs"/> instance containing the event data.</param>
        protected override void OnMouseUp(object sender, MouseEventArgs e)
        {
            this.color = Color.Yellow;
        }

        /// <summary>
        /// Called when a mouse button is pressed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected override void OnMouseDown(object sender, MouseEventArgs e)
        {
            this.color = Color.Red;
        }

        /// <summary>
        /// Called when mouse leaves the control region.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnMouseLeave(object sender, EventArgs e)
        {
            if (this.HasFocus)
            {
                this.color = Color.Pink;
            }
            else
            {
                this.color = Color.Green;
            }
        }

        /// <summary>
        /// Called when mouse enters the control region.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnMouseEnter(object sender, EventArgs e)
        {
            this.color = Color.Yellow;
        }

        protected override void OnEnter(object sender, EventArgs e)
        {
            this.color = Color.Pink;
        }

        protected override void OnLeave(object sender, EventArgs e)
        {
            this.color = Color.Green;
        }
    }
}