namespace Tests.Entites.Screens
{
    using System;
    using System.Collections.Generic;
    using Almirante.Engine.Core;
    using Almirante.Engine.Resources;
    using Almirante.Engine.Scenes;
    using Almirante.Entities;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Tests.Entites.Entities;
    using Tests.Entites.Entities.Components;
    using Tests.Entites.Entities.Systems;

    /// <summary>
    /// This is the main info for your game
    /// </summary>
    [Startup]
    public class MainState : Scene
    {
        /// <summary>
        /// The texture
        /// </summary>
        private Texture2D texture = null;

        /// <summary>
        /// The entities
        /// </summary>
        private EntityManager entities = new EntityManager();

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        protected override void OnInitialize()
        {
            this.entities.Systems.Add(new CameraSystem());
            this.entities.Systems.Add(new MovementSystem());

            Random random = new Random();
            for (int i = 0; i < 50; i++)
            {
                var entity = this.entities.Create<Player>();
                entity.Position.Set(0, 0);
                entity.Velocity.Value = new Vector2((float)random.Next(-10, 10), (float)random.Next(-10, 10));
            }

            this.texture = AlmiranteEngine.Resources.CreateTexture(1, 1, Color.Gray);
        }

        /// <summary>
        /// Uninitializes the scene.
        /// </summary>
        protected override void OnUninitialize()
        {
        }

        /// <summary>
        /// Screen graphic drawing function.
        /// </summary>
        protected override void OnDraw(SpriteBatch batch)
        {
            base.OnDraw(batch);

            batch.Start(true);
            var background = AlmiranteEngine.Resources.DefaultBackground;
            batch.Draw(background, Vector2.Zero - new Vector2(background.Width / 2, background.Height / 2), Color.White);
            this.entities.Draw();
            batch.End();

            string text = "FPS: {0}\n" +
                          "Camera Position: {1}\n" +
                          "Camera Rotation: {3}\n" +
                          "Camera Zoom: {2}\n";
            var font = AlmiranteEngine.Resources.DefaultFont;

            batch.Start();
            batch.Draw(this.texture, new Rectangle(20, 20, 400, 120), Color.White);
            batch.DrawFont(font, new Vector2(25, 25), Color.White, text, AlmiranteEngine.Time.Fps.ToString(), AlmiranteEngine.Camera.Position, AlmiranteEngine.Camera.Zoom, AlmiranteEngine.Camera.Rotation);
            batch.End();
        }

        /// <summary>
        /// Update call.
        /// </summary>
        protected override void OnUpdate()
        {
            this.entities.Update(AlmiranteEngine.Time.Total);
        }
    }
}