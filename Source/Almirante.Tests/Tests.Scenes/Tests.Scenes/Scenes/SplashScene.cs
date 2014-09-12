// -----------------------------------------------------------------------
// <copyright file="SplashScene.cs" company="">
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
    using Almirante.Engine.Resources;
    using Almirante.Engine.Scenes;
    using Almirante.Engine.Scenes.Transitions;
    using Almirante.Engine.Tweens;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Startup scene
    /// </summary>
    [Startup]
    public class SplashScene : Scene
    {
        /// <summary>
        /// Stores the splash image index
        /// </summary>
        private int imageIndex = 0;

        /// <summary>
        /// Stores the list of images
        /// </summary>
        private List<Resource<Texture2D>> images = new List<Resource<Texture2D>>();

        /// <summary>
        /// Stores the value tweener
        /// </summary>
        private ValueTweener tweener;

        /// <summary>
        /// Initializes the scene
        /// </summary>
        protected override void OnInitialize()
        {
            var resources = AlmiranteEngine.Resources;

            this.images.Add(resources.LoadSync<Texture2D>("Textures/Splash01"));
            this.images.Add(resources.LoadSync<Texture2D>("Textures/Splash02"));

            tweener = new ValueTweener(0, 1);
            tweener.Wait(0.5f).Forward(0.5f).Wait(0.5f).Backward(0.5f);
            tweener.Action(new Action(() =>
            {
                this.imageIndex++;
                if (this.imageIndex > this.images.Count - 1)
                {
                    this.imageIndex = 0;
                }
            }));
            tweener.Repeat(this.images.Count);
            tweener.Action(new Action(() =>
            {
                AlmiranteEngine.Scenes.Push<MainScene>(FadeInTransition.Name, 3.0f);
            }));
        }

        /// <summary>
        /// Destroys the scene instance.
        /// </summary>
        protected override void OnUninitialize()
        {
        }

        /// <summary>
        /// When entering the scene.
        /// </summary>
        protected override void OnEnter()
        {
            this.imageIndex = 0;
            tweener.Restart();
        }

        /// <summary>
        /// Updates the scene instance.
        /// </summary>
        protected override void OnUpdate()
        {
            tweener.Update(AlmiranteEngine.Time.Frame);
        }

        /// <summary>
        /// Draws the scene instance.
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnDraw(SpriteBatch batch)
        {
            if (!this.tweener.IsFinished)
            {
                batch.Begin();
                Vector2 pos;
                pos.Y = (AlmiranteEngine.Settings.Resolution.BaseHeight - this.images[this.imageIndex].Content.Height) / 2;
                pos.X = (AlmiranteEngine.Settings.Resolution.BaseWidth - this.images[this.imageIndex].Content.Width) / 2;
                batch.Draw(this.images[this.imageIndex].Content, pos, Color.White * this.tweener.Value);
                batch.End();
            }
        }
    }
}