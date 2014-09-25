using Almirante.Engine.Core;
using Almirante.Engine.Fonts;
using Almirante.Engine.Resources;
using Almirante.Engine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tests.Resources.Screens
{
    /// <summary>
    /// Loading scene class.
    /// </summary>
    public class LoadingScreen : Scene
    {
        /// <summary>
        /// Stores the loading text position.
        /// </summary>
        private readonly Vector2 pos = new Vector2(100, 100);

        private Texture2D black;

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        protected override void OnInitialize()
        {
            Color[] colors = new Color[1] { Color.Black };
            this.black = new Texture2D(AlmiranteEngine.Device, 1, 1);
            this.black.SetData(colors);
        }

        /// <summary>
        /// Uninitializes the scene.
        /// </summary>
        protected override void OnUninitialize()
        {
        }

        /// <summary>
        /// Updates the scene logic.
        /// </summary>
        protected override void OnUpdate()
        {
            if (!AlmiranteEngine.Resources.IsLoading)
            {
                AlmiranteEngine.Scenes.Pop();
            }
        }

        /// <summary>
        /// Draws the scene.
        /// </summary>
        protected override void OnDraw(SpriteBatch batch)
        {
            var resources = AlmiranteEngine.Resources;
            var settings = AlmiranteEngine.Settings;

            batch.Start();
            batch.Draw(this.black, new Rectangle(0, 0, settings.Resolution.BaseWidth, settings.Resolution.BaseHeight), Color.White);
            batch.DrawFont(resources.DefaultFont, this.pos, FontAlignment.Left, Color.White, string.Format("Loading resources... {0}", resources.PendingResources));
            batch.End();
        }
    }
}