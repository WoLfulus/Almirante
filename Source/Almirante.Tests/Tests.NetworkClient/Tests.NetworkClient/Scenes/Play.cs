using Almirante.Engine.Core;
using Almirante.Engine.Resources;
using Almirante.Engine.Scenes;
using Almirante.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tests.NetworkClient.Interface;
using Tests.NetworkClient.Objects;
using Tests.NetworkProtocol;

namespace Tests.NetworkClient.Scenes
{
    /// <summary>
    /// Play scene
    /// </summary>
    public class Play : Scene
    {
        /// <summary>
        /// Overlay
        /// </summary>
        private Texture2D background;

        /// <summary>
        /// 
        /// </summary>
        private Resource<Texture2D> map;

        /// <summary>
        /// Used to store all entities inside the game.
        /// </summary>
        private EntityManager entities;

        /// <summary>
        /// Textbox
        /// </summary>
        private Textbox text;

        /// <summary>
        /// Chatbox
        /// </summary>
        private Chat chat;

        /// <summary>
        /// Scene initialization
        /// </summary>
        protected override void OnInitialize()
        {
            var device = AlmiranteEngine.Device;
            this.background = new Texture2D(device, 1, 1);
            this.background.SetData(new Color[] { Color.FromNonPremultiplied(0, 0, 0, 255) });
            this.map = AlmiranteEngine.Resources.LoadSync<Texture2D>("Objects\\Back");

            this.text = new Textbox()
            {
                Size = new Vector2(620, 30),
                Position = new Vector2(10, 720 - 40),
                MaxLength = 60
            };
            this.text.KeyDown += OnKeyDown;
            this.text.Enter += OnFocus;
            this.text.Leave += OnUnfocus;

            this.Interface.Controls.Add(this.text);

            this.chat = new Chat()
            {
                Size = new Vector2(620, 620),
                Position = new Vector2(10, 720 - 40 - 10 - 620),
                MaxEntries = 50
            };
            this.Interface.Controls.Add(this.chat);

            Player.Instance.Protocol.Subscribe<PlayerMessage>(this.OnMessage);
        }

        /// <summary>
        /// Focus 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnUnfocus(object sender, EventArgs e)
        {
            this.text.Text = "Message...";
        }

        /// <summary>
        /// Focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFocus(object sender, EventArgs e)
        {
            this.text.Text = "";
        }

        /// <summary>
        /// Message
        /// </summary>
        /// <param name="message"></param>
        void OnMessage(PlayerMessage message)
        {
            this.chat.AddMessage(message.Name, message.Message);
        }

        /// <summary>
        /// Key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnKeyDown(object sender, Almirante.Engine.Interface.KeyboardEventArgs e)
        {
            if (e.Key.Key == Keys.Enter)
            {
                Player.Instance.Send<MessageRequest>(new MessageRequest()
                {
                    Message = this.text.Text
                });
                this.text.Text = "";
            }
        }

        /// <summary>
        /// Uninitialize
        /// </summary>
        protected override void OnUninitialize()
        {
        }

        /// <summary>
        /// Entering the scene
        /// </summary>
        protected override void OnEnter()
        {
            // Creates a new entity manager
            this.entities = new EntityManager();
        }

        /// <summary>
        /// Leaving the scene
        /// </summary>
        protected override void OnLeave()
        {
            this.entities = null;
        }

        /// <summary>
        /// Update
        /// </summary>
        protected override void OnUpdate()
        {
            // Updates the entity system
            this.entities.Update(AlmiranteEngine.Time.Frame); 
        }

        /// <summary>
        /// Draw function
        /// </summary>
        /// <param name="batch"></param>
        protected override void OnDraw(SpriteBatch batch)
        {
            batch.Start();
            batch.Draw(this.background, new Rectangle(0, 0, 1280, 720), Color.White); // Background overlay (Black)
            batch.End();

            // Starts the batch drawing using the camera transformation matrix
            batch.Start(true);
            // Draws a background (map)
            int sx = -1000;
            int sy = -1000;
            for (int x = 0; x < 4; x++)
            {
                sy = -1000;
                for (int y = 0; y < 4; y++)
                {
                    batch.Draw(this.map.Content, new Vector2(sx, sy), Color.White);
                    sy += 500;
                }
                sx += 500;
            }
            // Draws the entities to the screen
            this.entities.Draw();
            // Ends the drawing
            batch.End();
        }
    }
}
