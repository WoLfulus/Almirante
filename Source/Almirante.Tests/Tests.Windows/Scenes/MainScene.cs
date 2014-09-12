namespace Tests.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Almirante.Engine.Core;
    using Almirante.Engine.Fonts;
    using Almirante.Engine.Resources;
    using Almirante.Engine.Scenes;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// This is the main info for your game
    /// </summary>
    public class MainScene : SceneControl
    {
        /// <summary>
        /// The texture
        /// </summary>
        private Texture2D texture = null;

        /// <summary>
        /// Down
        /// </summary>
        private bool down = false;

        /// <summary>
        /// The font
        /// </summary>
        private BitmapFont font;

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        protected override void OnInitialize()
        {
            this.BackgroundColor = Color.SlateGray;
            this.texture = AlmiranteEngine.Resources.LoadTexture("Content\\Box.png");
            this.font = BitmapFont.FromFile("Content\\Default.bitmapfont");
        }

        /// <summary>
        /// Uninitializes the scene.
        /// </summary>
        protected override void OnUninitialize()
        {
        }

        /// <summary>
        /// Called when [mouse down].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        protected override void OnMouseDown(object sender, MouseEventArgs e)
        {
            this.down = true;
        }

        /// <summary>
        /// Called when [mouse up].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        protected override void OnMouseUp(object sender, MouseEventArgs e)
        {
            this.down = false;
        }

        /// <summary>
        /// Screen graphic drawing function.
        /// </summary>
        protected override void OnDraw(SpriteBatch batch)
        {
            base.OnDraw(batch);

            batch.Start(true);
            // batch.Draw(this.texture, Vector2.Zero, Color.White * (down ? 1.0f : 0.5f));
            batch.Draw(this.font.Texture, Vector2.Zero, Color.White);
            batch.DrawFont(this.font, new Vector2(-40, -20), "abcdefghijklmnopqrstuvxyz");
            batch.DrawFont(this.font, new Vector2(-40, -30), "ABCDEFGHIJKLMNOPQRSTUVXYZ");
            batch.End();
        }

        /// <summary>
        /// Update call.
        /// </summary>
        protected override void OnUpdate()
        {
        }
    }
}