// -----------------------------------------------------------------------
// <copyright file="OptionsScene.cs" company="">
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
    using Almirante.Engine.Scenes;
    using Almirante.Engine.Scenes.Transitions;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Tests.Scenes.Scenes.Interface;

    /// <summary>
    /// Options scene.
    /// </summary>
    public class OptionsScene : Scene
    {
        /// <summary>
        /// The button1
        /// </summary>
        private Button button1;

        /// <summary>
        /// The button2
        /// </summary>
        private Button button2;

        /// <summary>
        /// The button3
        /// </summary>
        private Button button3;

        /// <summary>
        /// The button4
        /// </summary>
        private Button button4;

        /// <summary>
        /// The background
        /// </summary>
        private Texture2D background;

        /// <summary>
        /// Initializes the scene
        /// </summary>
        protected override void OnInitialize()
        {
            this.background = AlmiranteEngine.Resources.CreateTexture(1, 1, Color.Black);

            this.button1 = new Button()
            {
                Position = new Vector2(540, 240),
                Size = new Vector2(200, 50),
                Text = "Graphics"
            };
            this.button1.MouseClick += new EventHandler<MouseEventArgs>((obj, args) =>
            {
            });

            this.Interface.Controls.Add(this.button1);

            this.button2 = new Button()
            {
                Position = new Vector2(540, 240 + 60),
                Size = new Vector2(200, 50),
                Text = "Audio"
            };
            this.button2.MouseClick += new EventHandler<MouseEventArgs>((obj, args) =>
            {
            });

            this.Interface.Controls.Add(this.button2);

            this.button3 = new Button()
            {
                Position = new Vector2(540, 240 + 60 + 60),
                Size = new Vector2(200, 50),
                Text = "Controls"
            };
            this.button3.MouseClick += new EventHandler<MouseEventArgs>((obj, args) =>
            {
            });

            this.Interface.Controls.Add(this.button3);

            this.button4 = new Button()
            {
                Position = new Vector2(540, 240 + 60 + 60 + 60),
                Size = new Vector2(200, 50),
                Text = "Back"
            };
            this.button4.MouseClick += new EventHandler<MouseEventArgs>((obj, args) =>
            {
                AlmiranteEngine.Scenes.Pop(FadeOutTransition.Name, 1.0f);
            });

            this.Interface.Controls.Add(this.button4);
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
            Rectangle rect;
            rect.X = 0;
            rect.Y = 0;
            rect.Width = AlmiranteEngine.Settings.Resolution.BaseWidth;
            rect.Height = AlmiranteEngine.Settings.Resolution.BaseHeight;

            batch.Start();
            batch.Draw(this.background, rect, Color.White * 0.8f);
            batch.End();
        }
    }
}