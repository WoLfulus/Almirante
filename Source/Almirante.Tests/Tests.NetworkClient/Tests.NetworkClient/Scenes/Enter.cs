using Almirante.Engine.Core;
using Almirante.Engine.Fonts;
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
using Tests.NetworkClient.Objects;
using Tests.NetworkProtocol;
using Almirante.Engine.Interface;

namespace Tests.NetworkClient.Scenes
{
    /// <summary>
    /// Login screen
    /// </summary>
    public class Enter : Scene
    {
        /// <summary>
        /// Overlay
        /// </summary>
        private Texture2D overlay;

        /// <summary>
        /// 
        /// </summary>
        private Textbox textname;

        /// <summary>
        /// Play
        /// </summary>
        private Button enter;

        /// <summary>
        /// Exit
        /// </summary>
        private Button back;

        /// <summary>
        /// 
        /// </summary>
        private Control panel_login;

        /// <summary>
        /// 
        /// </summary>
        private Label message_label;

        /// <summary>
        /// Panel
        /// </summary>
        private Control message_panel;

        /// <summary>
        /// Button
        /// </summary>
        private Button message_button;

        /// <summary>
        /// Initialization
        /// </summary>
        protected override void OnInitialize()
        {
            var device = AlmiranteEngine.Device;
            this.overlay = new Texture2D(device, 1, 1);
            this.overlay.SetData(new Color[] { Color.FromNonPremultiplied(0, 0, 0, 255) });

            ///
            /// Name panel
            ///

            this.panel_login = new Control()
            {
                Size = new Vector2(250, 250),
                Position = new Vector2((1280 - 250) / 2, (720 - 250) / 2)
            };

            var labellogin = new Label()
            {
                Text = "Name:",
                Visible = true,
                Size = new Vector2(250, 15),
                Position = new Vector2(0, 0)
            };
            this.panel_login.Controls.Add(labellogin);

            this.textname = new Textbox()
            {
                Visible = true,
                Size = new Vector2(250, 30),
                Position = labellogin.Position + new Vector2(0, 30)
            };
            this.panel_login.Controls.Add(this.textname);

            this.enter = new Button()
            {
                Text = "Enter",
                Visible = true,
                Size = new Vector2(120, 30),
                Position = textname.Position + new Vector2(0, 40)
            };
            this.enter.MouseClick += OnEnter;
            this.panel_login.Controls.Add(this.enter);

            this.back = new Button()
            {
                Text = "Back",
                Visible = true,
                Size = new Vector2(120, 30),
                Position = textname.Position + new Vector2(130, 40)
            };
            this.back.MouseClick += OnBack;
            this.panel_login.Controls.Add(this.back);

            this.Interface.Controls.Add(this.panel_login);

            ///
            /// Message
            ///

            this.message_label = new Label()
            {
                Size = new Vector2(600, 15),
                Position = new Vector2(300, 0),
                Alignment = FontAlignment.Center,
                Text = "?"
            };

            this.message_button = new Button()
            {
                Size = new Vector2(100, 30), 
                Position = new Vector2(250, 30),
                Text = "OK"
            };

            this.message_panel = new Control()
            {
                Size = new Vector2(600, 50),
                Position = new Vector2((1280 - 600) / 2, (720 - 50) / 2),
                Visible = false
            };

            this.message_panel.Controls.Add(this.message_label);
            this.message_panel.Controls.Add(this.message_button);

            this.Interface.Controls.Add(this.message_panel);

            ///
            /// Messages
            ///

            Player.Instance.Protocol.Subscribe<JoinResponse>(this.OnJoinResponse);

        }

        /// <summary>
        /// Exit button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBack(object sender, Almirante.Engine.Interface.MouseEventArgs e)
        {
            AlmiranteEngine.Scenes.Pop();
        }

        /// <summary>
        /// Play button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEnter(object sender, Almirante.Engine.Interface.MouseEventArgs e)
        {
            Player.Instance.Send<JoinRequest>(new JoinRequest()
            {
                Name = this.textname.Text
            });
            this.panel_login.Visible = false;
            this.message_panel.Visible = false;
        }

        /// <summary>
        /// Join response
        /// </summary>
        /// <param name="response"></param>
        private void OnJoinResponse(JoinResponse response)
        {
            if (response.Success)
            {
                AlmiranteEngine.Scenes.Switch<Play>();
            }
            else
            {
                this.message_label.Text = response.Message;
                this.message_panel.Visible = true;
            }
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
