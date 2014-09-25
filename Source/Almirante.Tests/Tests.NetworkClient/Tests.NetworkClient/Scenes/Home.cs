using Almirante.Engine.Core;
using Almirante.Engine.Resources;
using Almirante.Engine.Scenes;
using Almirante.Engine.Scenes.Transitions;
using Tests.NetworkClient.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.NetworkClient.Scenes
{
    /// <summary>
    /// Home
    /// </summary>
    public class Home : Scene
    {
        /// <summary>
        /// BG
        /// </summary>
        private Resource<Texture2D> background;

        /// <summary>
        /// Play
        /// </summary>
        private Button play;

        /// <summary>
        /// Exit
        /// </summary>
        private Button exit;

        /// <summary>
        /// Initialization
        /// </summary>
        protected override void OnInitialize()
        {
            this.background = AlmiranteEngine.Resources.LoadSync<Texture2D>("Interface\\Background");
            
            this.play = new Button()
            {
                Text = "Enter", 
                Visible = true, 
                Size = new Vector2(200, 30), 
                Position = new Vector2(50, 520)
            };
            this.play.MouseClick += OnPlay;
            this.Interface.Controls.Add(this.play);

            this.exit = new Button()
            {
                Text = "Exit",
                Visible = true,
                Size = new Vector2(200, 30),
                Position = new Vector2(50, 560)
            };
            this.exit.MouseClick += OnExit;
            this.Interface.Controls.Add(this.exit);
        }

        /// <summary>
        /// Activate
        /// </summary>
        protected override void OnActivate()
        {
        }

        /// <summary>
        /// Play button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPlay(object sender, Almirante.Engine.Interface.MouseEventArgs e)
        {
            AlmiranteEngine.Scenes.Push<Enter>();
        }

        /// <summary>
        /// Exit button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExit(object sender, Almirante.Engine.Interface.MouseEventArgs e)
        {
            AlmiranteEngine.Stop();
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
            batch.Draw(this.background.Content, Vector2.Zero, Color.White);
            batch.End();
        }
    }
}
