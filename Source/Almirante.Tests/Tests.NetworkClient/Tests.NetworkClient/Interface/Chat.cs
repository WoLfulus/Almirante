using Almirante.Engine.Core;
using Almirante.Engine.Interface;
using Almirante.Engine.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Tests.NetworkClient.Interface
{
    public class Chat : Control
    {
        /// <summary>
        /// Max entries
        /// </summary>
        public int MaxEntries
        {
            get;
            set;
        }

        private List<string> messages;
        
        /// <summary>
        /// Font
        /// </summary>
        private Resource<SpriteFont> font;

        /// <summary>
        /// Texture
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Fade
        /// </summary>
        private Color color;

        /// <summary>
        /// Constructor
        /// </summary>
        public Chat()
        {
            var device = AlmiranteEngine.Device;
            this.texture = new Texture2D(device, 1, 1);
            this.texture.SetData(new Color[] { Color.FromNonPremultiplied(180, 180, 180, 255) });
            this.font = AlmiranteEngine.Resources.LoadSync<SpriteFont>("Fonts\\Textbox");
            this.color = Color.White * 0.8f;
            this.messages = new List<string>();
            this.MaxEntries = 100;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        public void AddMessage(string name, string message)
        {
            message = ("[" + DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture) + "] " + name + ": " + message).Replace("\r", "").Replace("\n", "");
            var temp = "";
            for (int i = 0; i < message.Length; i++)
            {
                var size = this.font.Content.MeasureText(temp + message[i]);
                if (size.X > this.Size.X - 10)
                {
                    temp = temp + "\n";
                }
                else
                {
                    temp = temp + message[i];
                }
            }
            this.messages.Add(temp);
            if (this.messages.Count > this.MaxEntries && this.MaxEntries != 0)
            {
                this.messages.RemoveAt(0);
            }
        }

        /// <summary>
        /// Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnEnter(object sender, EventArgs e)
        {
            this.color = Color.White;
        }

        /// <summary>
        /// Leave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnLeave(object sender, EventArgs e)
        {
            this.color = Color.White * 0.80f;
        }

        /// <summary>
        /// Scroll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnMouseScroll(object sender, MouseEventArgs e)
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
        /// <param name="position"></param>
        protected override void OnDraw(SpriteBatch batch, Vector2 position)
        {
            if (this.Visible)
            {
                // Background
                batch.Draw(this.texture, new Rectangle((int)position.X, (int)position.Y, (int)this.Size.X, (int)this.Size.Y), this.color);

                // Borders
                batch.DrawLine(position, position + new Vector2(this.Size.X, 0), Color.Black, 2);
                batch.DrawLine(position + new Vector2(this.Size.X, 0), position + new Vector2(this.Size.X, this.Size.Y), Color.Black, 2);
                batch.DrawLine(position + new Vector2(this.Size.X, this.Size.Y), position + new Vector2(0, this.Size.Y), Color.Black, 2);
                batch.DrawLine(position + new Vector2(0, this.Size.Y), position, Color.Black, 2);

                batch.EnableScissor(new Rectangle((int)position.X + 2, (int)position.Y + 2, (int)this.Size.X - 2, (int)this.Size.Y - 2));
                {
                    batch.Draw(this.texture, new Rectangle(0, 0, 1, 1), Color.White * 0.0f);

                    var lastPosition = position + new Vector2(2, this.Size.Y - 2);
                    for (int i = this.messages.Count - 1; i >= 0; i--)
                    {
                        var text = this.messages[i];
                        var size = this.font.Content.MeasureText(text);
                        lastPosition.Y -= size.Y;
                        batch.DrawString(this.font.Content, text, lastPosition, Color.Black);
                    }
                }
                batch.DisableScissor();
            }
        }
    }
}
