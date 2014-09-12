using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Almirante.Engine.Core;
using Almirante.Engine.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tests.Gui.Screens.Controls
{
    /// <summary>
    /// Panel control class.
    /// </summary>
    public class Panel : Control
    {
        /// <summary>
        /// Background texture.
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        public Panel()
        {
            var device = AlmiranteEngine.Device;
            this.texture = new Texture2D(device, 1, 1);
            this.texture.SetData(new Color[] { Color.FromNonPremultiplied(128, 128, 128, 128) });
        }

        /// <summary>
        /// Called when updating the HUD component.
        /// </summary>
        protected override void OnUpdate()
        {
        }

        /// <summary>
        /// Called when drawing the HUD component.
        /// </summary>
        /// <param name="batch">The sprite batch instance.</param>
        /// <param name="position">The position.</param>
        protected override void OnDraw(SpriteBatch batch, Vector2 position)
        {
            if (this.Active)
            {
                batch.Draw(this.texture, new Rectangle((int)position.X, (int)position.Y, (int)this.Size.X, (int)this.Size.Y), Color.Blue);
            }
            else
            {
                batch.Draw(this.texture, new Rectangle((int)position.X, (int)position.Y, (int)this.Size.X, (int)this.Size.Y), Color.White);
            }
        }
    }
}