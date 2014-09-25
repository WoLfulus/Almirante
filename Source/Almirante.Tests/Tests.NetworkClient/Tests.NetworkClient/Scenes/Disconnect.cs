using Almirante.Engine.Core;
using Almirante.Engine.Fonts;
using Almirante.Engine.Resources;
using Almirante.Engine.Scenes;
using Almirante.Engine.Tweens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tests.NetworkClient.Interface;

namespace Tests.NetworkClient.Scenes
{
    /// <summary>
    /// </summary>
    public class Disconnect : Scene
    {
        /// <summary>
        /// Overlay
        /// </summary>
        private Texture2D overlay;

        /// <summary>
        /// Play
        /// </summary>
        private Button yes;

        /// <summary>
        /// Exit
        /// </summary>
        private Button no;

        /// <summary>
        /// Initialization
        /// </summary>
        protected override void OnInitialize()
        {
            var device = AlmiranteEngine.Device;
            this.overlay = new Texture2D(device, 1, 1);
            this.overlay.SetData(new Color[] { Color.FromNonPremultiplied(0, 0, 0, 255) });

            var labellogin = new Label()
            {
                Text = "Connection failed. Reconnect?",
                Visible = true,
                Size = new Vector2(250, 15),
                Position = new Vector2(1280 / 2, (720 - 15) / 2),
                Alignment = FontAlignment.Center
            };
            this.Interface.Controls.Add(labellogin);

            this.yes = new Button()
            {
                Text = "Enter",
                Visible = true,
                Size = new Vector2(120, 30),
                Position = new Vector2((1280 - 250) / 2, ((720 - 15) / 2) + 40)
            };
            this.yes.MouseClick += OnYes;
            this.Interface.Controls.Add(this.yes);

            this.no = new Button()
            {
                Text = "Exit",
                Visible = true,
                Size = new Vector2(120, 30),
                Position = new Vector2(((1280 - 250) / 2) + 145, ((720 - 15) / 2) + 40)
            };
            this.no.MouseClick += OnNo;
            this.Interface.Controls.Add(this.no);
        }

        /// <summary>
        /// Exit button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNo(object sender, Almirante.Engine.Interface.MouseEventArgs e)
        {
            AlmiranteEngine.Stop();
        }

        /// <summary>
        /// Play button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnYes(object sender, Almirante.Engine.Interface.MouseEventArgs e)
        {
            AlmiranteEngine.Scenes.PushClear<Connect>();
        }

        /// <summary>
        /// Destroy
        /// </summary>
        protected override void OnUninitialize()
        {
        }

        /// <summary>
        /// Update
        /// </summary>
        protected override void OnUpdate()
        {
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnDraw(SpriteBatch batch)
        {
            batch.Start();
            batch.Draw(this.overlay, new Rectangle(0, 0, 1280, 720), Color.White * 0.80f); // Background overlay (Black)
            batch.End();
        }
    }
}
