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
using Tests.NetworkClient.Objects;

namespace Tests.NetworkClient.Scenes
{
    /// <summary>
    /// </summary>
    [Startup]
    public class Connect : Scene
    {
        /// <summary>
        /// 
        /// </summary>
        private Resource<SpriteFont> font;

        /// <summary>
        /// Splash fade.
        /// </summary>
        private ValueTweener fade;

        /// <summary>
        /// Scene creation.
        /// </summary>
        protected override void OnInitialize()
        {
            this.font = AlmiranteEngine.Resources.LoadSync<SpriteFont>("Fonts/Small");
            this.fade = new ValueTweener(0.5f, 1);
            this.fade.Forward(0.5f).Backward(0.5f).Repeat();
        }

        /// <summary>
        /// Scene destruction.
        /// </summary>
        protected override void OnUninitialize()
        {
        }

        /// <summary>
        /// Scene logic.
        /// </summary>
        protected override void OnUpdate()
        {
            this.fade.Update(AlmiranteEngine.Time.Frame); 
        }

        /// <summary>
        /// Scene enter
        /// </summary>
        protected override void OnEnter()
        {
            /// Connect 
            Player.Instance.Connect("localhost", 8000);
            this.fade.Restart();
        }

        /// <summary>
        /// Scene rendering.
        /// </summary>
        /// <param name="batch">Sprite batch</param>
        protected override void OnDraw(SpriteBatch batch)
        {
            if (!this.fade.IsFinished)
            {
                batch.Start(); 
                batch.DrawString(this.font.Content, "Connecting...", new Vector2(25, 25), Color.White * this.fade.Value);
                batch.End();
            }
        }
    }
}
