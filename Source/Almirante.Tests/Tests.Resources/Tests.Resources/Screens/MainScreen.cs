using Almirante.Engine.Core;
using Almirante.Engine.Resources;
using Almirante.Engine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tests.Resources.Screens;

namespace Tests.Resources
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    [Startup]
    public class MainScreen : Scene
    {
        /// <summary>
        /// Stores the textures.
        /// </summary>
        private readonly Resource<Texture2D>[] textures = new Resource<Texture2D>[16];

        /// <summary>
        /// Initializes the main scene.
        /// </summary>
        protected override void OnInitialize()
        {
            var resources = AlmiranteEngine.Resources;

            // 1.5 seconds for each resource.
            resources.Delay = 200;

            // Requests asynchronous loading of the specified assets.
            for (int i = 0; i < this.textures.Length; i++)
            {
                this.textures[i] = resources.LoadAsync<Texture2D>(string.Format("Textures\\sprite{0}", (i + 1).ToString()));
            }

            // Pushes a loading scene to the scene stack.
            var screenManager = AlmiranteEngine.Scenes;
            screenManager.Push<LoadingScreen>();
        }

        /// <summary>
        /// Uninitializes the scene.
        /// </summary>
        protected override void OnUninitialize()
        {
        }

        /// <summary>
        /// Updates the scene.
        /// </summary>
        protected override void OnUpdate()
        {
        }

        /// <summary>
        /// Draws the scene.
        /// </summary>
        protected override void OnDraw(SpriteBatch batch)
        {
            var resources = AlmiranteEngine.Resources;

            batch.Start();
            batch.Draw(resources.DefaultBackground, new Vector2(0, 0), Color.White);
            for (int i = 0; i < this.textures.Length; i++)
            {
                if (this.textures[i].Content != null)
                {
                    batch.Draw(this.textures[i].Content, new Vector2(20 + i * 80, 20 + i * 40), Color.White);
                }
            }
            batch.End();
        }
    }
}