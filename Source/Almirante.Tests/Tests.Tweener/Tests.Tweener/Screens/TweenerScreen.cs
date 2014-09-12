using System;
using System.Diagnostics;
using Almirante.Engine.Core;
using Almirante.Engine.Resources;
using Almirante.Engine.Scenes;
using Almirante.Engine.Tweens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tests.Tweener.Screens
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    [Startup]
    public class TweenerScreen : Scene
    {
        /// <summary>
        ///
        /// </summary>
        private Resource<Texture2D> character;

        /// <summary>
        ///
        /// </summary>
        private float angle;

        /// <summary>
        ///
        /// </summary>
        private ValueTweener angleTweener;

        /// <summary>
        ///
        /// </summary>
        private Color color;

        /// <summary>
        ///
        /// </summary>
        private ColorTweener colorTweener;

        /// <summary>
        ///
        /// </summary>
        private Vector2 position;

        /// <summary>
        ///
        /// </summary>
        private VectorTweener positionTweener;

        /// <summary>
        ///
        /// </summary>
        private Vector2[] positions;

        /// <summary>
        ///
        /// </summary>
        private ValueTweener[] positionsTweener;

        /// <summary>
        /// Initializes a new instance of the <see cref="TweenerScreen"/> class.
        /// </summary>
        public TweenerScreen()
        {
        }

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        protected override void OnInitialize()
        {
            // Character texture
            var resources = AlmiranteEngine.Resources;
            this.character = resources.LoadAsync<Texture2D>("Textures\\character");

            // Position tweener
            this.position = new Vector2(0, 0);
            this.positionTweener = new VectorTweener(new Vector2(470, 30), new Vector2(720, 180));
            this.positionTweener.ValueChanged += (tween, data) =>
            {
                this.position = tween.Value;
            };

            this.positionTweener.Forward(5, MotionTweens.ElasticInOut).Backward(5, MotionTweens.BounceInOut).Action(() =>
            {
            }).Repeat().Start();

            // Angle tweener
            this.angle = 0;
            this.angleTweener = new ValueTweener(0, MathHelper.ToRadians(360));
            this.angleTweener.ValueChanged += (tweener, data) =>
            {
                this.angle = tweener.Value;
            };

            this.angleTweener.Forward(1).Action(() =>
            {
            }).Repeat().Start();

            // Color tweener
            this.color = Color.White;
            this.colorTweener = new ColorTweener(Color.White, Color.Red);
            this.colorTweener.ValueChanged += (tweener, data) =>
            {
                this.color = tweener.Value;
            };

            this.colorTweener.Forward(0.8f).Backward(0.8f).Action(() =>
            {
            }).Repeat().Start();

            // Positions tweener
            this.positions = new Vector2[8];
            this.positionsTweener = new ValueTweener[8];

            for (int i = 0; i < this.positions.Length; i++)
            {
                this.positions[i] = new Vector2();
                this.positions[i].X = 20;
                this.positions[i].Y = 30 + (50 * i);

                this.positionsTweener[i] = new ValueTweener(this.positions[i].X, this.positions[i].X + 400, i);
                this.positionsTweener[i].ValueChanged += (tween, data) =>
                {
                    this.positions[(int)data].X = tween.Value;
                };
            }

            this.positionsTweener[0].Forward(5).Backward(5).Repeat().Start();
            this.positionsTweener[1].Backward(5).Forward(3).Backward(1).Forward(1).Repeat().Start();
            this.positionsTweener[2].Forward(4).Wait(2).Backward(4).Repeat().Start();
            this.positionsTweener[3].Forward(2).Backward(2).Wait(1).Repeat().Start();
            this.positionsTweener[4].Wait(2).Forward(2).Backward(2).Forward(2, MotionTweens.ElasticInOut).Backward(2, MotionTweens.SinusoidalInOut).Repeat().Start();
            this.positionsTweener[5].Forward(1).Backward(1).Repeat().Start();
            this.positionsTweener[6].Backward(5).Forward(1).Backward(1).Wait(1).Forward(1).Backward(0.5f).Forward(0.5f).Repeat().Start();
            this.positionsTweener[7].Forward(8, MotionTweens.ExponentialInOut).Backward(2, MotionTweens.QuinticInOut).Repeat().Start();

            // this.Almirante.IsCursorVisible = true;
        }

        /// <summary>
        /// Uninitializes the scene.
        /// </summary>
        protected override void OnUninitialize()
        {
        }

        /// <summary>
        /// Screen logic update function.
        /// </summary>
        protected override void OnUpdate()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                AlmiranteEngine.Stop();
            }

            this.angleTweener.Update(AlmiranteEngine.Time.Frame);
            this.colorTweener.Update(AlmiranteEngine.Time.Frame);
            this.positionTweener.Update(AlmiranteEngine.Time.Frame);

            for (int i = 0; i < this.positionsTweener.Length; i++)
            {
                this.positionsTweener[i].Update(AlmiranteEngine.Time.Frame);
            }
        }

        /// <summary>
        /// Screen graphic drawing function.
        /// </summary>
        protected override void OnDraw(SpriteBatch batch)
        {
            var resources = AlmiranteEngine.Resources;
            batch.Start();
            batch.Draw(resources.DefaultBackground, new Vector2(0, 0), Color.White);
            if (this.character.Loaded && this.character.Content != null)
            {
                for (int i = 0; i < this.positions.Length; i++)
                {
                    batch.Draw(this.character.Content, this.positions[i], Color.White);
                }
                batch.Draw(this.character.Content, this.position, Color.White);
                batch.Draw(this.character.Content, new Vector2(580, 280), null, this.color, this.angle, new Vector2(this.character.Content.Width / 2, this.character.Content.Height / 2), new Vector2(1, 1), SpriteEffects.None, 0);
            }
            batch.End();
        }
    }
}