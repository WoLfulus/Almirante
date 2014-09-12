// -----------------------------------------------------------------------
// <copyright file="MainScene.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Tests.Scenes.Scenes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Almirante.Engine.Core;
    using Almirante.Engine.Interface;
    using Almirante.Engine.Resources;
    using Almirante.Engine.Scenes;
    using Almirante.Engine.Scenes.Transitions;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Tests.Scenes.Scenes.Interface;

    /// <summary>
    /// Main scene
    /// </summary>
    public class MainScene : Scene
    {
        private Button button1;
        private Button button2;
        private Button button3;

        /// <summary>
        /// Initializes the scene
        /// </summary>
        protected override void OnInitialize()
        {
            this.button1 = new Button()
            {
                Position = new Vector2(540, 270),
                Size = new Vector2(200, 50),
                Text = "Play"
            };
            this.button1.MouseClick += new EventHandler<MouseEventArgs>((obj, args) =>
            {
            });

            this.Interface.Controls.Add(this.button1);

            this.button2 = new Button()
            {
                Position = new Vector2(540, 270 + 60),
                Size = new Vector2(200, 50),
                Text = "Options"
            };
            this.button2.MouseClick += new EventHandler<MouseEventArgs>((obj, args) =>
            {
                AlmiranteEngine.Scenes.Push<OptionsScene>(FadeInTransition.Name, 1.0f);
            });

            this.Interface.Controls.Add(this.button2);

            this.button3 = new Button()
            {
                Position = new Vector2(540, 270 + 60 + 60),
                Size = new Vector2(200, 50),
                Text = "Exit"
            };
            this.button3.MouseClick += new EventHandler<MouseEventArgs>((obj, args) =>
            {
                AlmiranteEngine.Stop();
            });

            this.Interface.Controls.Add(this.button3);
        }

        /// <summary>
        /// Destroys the scene
        /// </summary>
        protected override void OnUninitialize()
        {
        }

        /// <summary>
        /// Updates the scene
        /// </summary>
        protected override void OnUpdate()
        {
        }

        /// <summary>
        /// Draws the scene
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnDraw(SpriteBatch batch)
        {
            batch.Start();
            batch.Draw(AlmiranteEngine.Resources.DefaultBackground, Vector2.Zero, Color.White);
            batch.End();
        }
    }
}