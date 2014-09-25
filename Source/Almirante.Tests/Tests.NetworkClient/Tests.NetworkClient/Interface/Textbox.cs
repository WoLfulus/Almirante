using Almirante.Engine.Core;
using Almirante.Engine.Fonts;
using Almirante.Engine.Interface;
using Almirante.Engine.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Tests.NetworkClient.Interface
{
    /// <summary>
    /// Textbox
    /// </summary>
    public class Textbox : Control
    {
        /// <summary>
        /// Font
        /// </summary>
        private Resource<SpriteFont> font;

        /// <summary>
        /// Background texture.
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// Color.
        /// </summary>
        private Color color;

        /// <summary>
        /// Gets or sets the button text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Password
        /// </summary>
        public bool Password
        {
            get;
            set;
        }

        /// <summary>
        /// Maximum length
        /// </summary>
        public int MaxLength
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Textbox"/> class.
        /// </summary>
        public Textbox()
        {
            this.Text = "";
            this.MaxLength = 0;
            var device = AlmiranteEngine.Device;
            this.texture = new Texture2D(device, 1, 1);
            this.texture.SetData(new Color[] { Color.FromNonPremultiplied(255, 255, 255, 255) });
            this.font = AlmiranteEngine.Resources.LoadSync<SpriteFont>("Fonts\\Textbox");
            this.color = Color.White * 0.8f;
            this.Password = false;
        }

        /// <summary>
        /// Called when drawing the HUD component.
        /// </summary>
        /// <param name="batch">The sprite batch instance.</param>
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

                batch.EnableScissor(new Rectangle((int)position.X + 2, (int)position.Y + 2, (int)this.Size.X - 4, (int)this.Size.Y - 4));
                {
                    var text = this.Password ? (this.Text.Length > 0 ? Enumerable.Repeat("*", this.Text.Length).Aggregate((s, n) => s + n) : "") : this.Text;
                    var size = this.font.Content.MeasureString(this.Text);
                    var pos = position + new Vector2(5, (this.Size.Y - size.Y) / 2);
                    if (size.X > this.Size.X - 10)
                    {
                        pos.X = (pos.X + this.Size.X - 10) - size.X;
                    }
                    batch.DrawString(this.font.Content, text, pos, Color.Black);
                }
                batch.DisableScissor();
            }
        }

        /// <summary>
        /// Key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnKeyDown(object sender, KeyboardEventArgs e)
        {
            bool shift = AlmiranteEngine.Input.Keyboard[Keys.LeftShift].Down || AlmiranteEngine.Input.Keyboard[Keys.RightShift].Down;
            if (e.Key.Key == Keys.Back)
            {
                if (this.Text.Length > 0)
                {
                    this.Text = this.Text.Substring(0, this.Text.Length - 1);
                }
                return;
            }

            char key = '\0';
            switch (e.Key.Key)
            {
                case Keys.A: if (shift) { key = 'A'; } else { key = 'a'; } break;
                case Keys.B: if (shift) { key = 'B'; } else { key = 'b'; } break;
                case Keys.C: if (shift) { key = 'C'; } else { key = 'c'; } break;
                case Keys.D: if (shift) { key = 'D'; } else { key = 'd'; } break;
                case Keys.E: if (shift) { key = 'E'; } else { key = 'e'; } break;
                case Keys.F: if (shift) { key = 'F'; } else { key = 'f'; } break;
                case Keys.G: if (shift) { key = 'G'; } else { key = 'g'; } break;
                case Keys.H: if (shift) { key = 'H'; } else { key = 'h'; } break;
                case Keys.I: if (shift) { key = 'I'; } else { key = 'i'; } break;
                case Keys.J: if (shift) { key = 'J'; } else { key = 'j'; } break;
                case Keys.K: if (shift) { key = 'K'; } else { key = 'k'; } break;
                case Keys.L: if (shift) { key = 'L'; } else { key = 'l'; } break;
                case Keys.M: if (shift) { key = 'M'; } else { key = 'm'; } break;
                case Keys.N: if (shift) { key = 'N'; } else { key = 'n'; } break;
                case Keys.O: if (shift) { key = 'O'; } else { key = 'o'; } break;
                case Keys.P: if (shift) { key = 'P'; } else { key = 'p'; } break;
                case Keys.Q: if (shift) { key = 'Q'; } else { key = 'q'; } break;
                case Keys.R: if (shift) { key = 'R'; } else { key = 'r'; } break;
                case Keys.S: if (shift) { key = 'S'; } else { key = 's'; } break;
                case Keys.T: if (shift) { key = 'T'; } else { key = 't'; } break;
                case Keys.U: if (shift) { key = 'U'; } else { key = 'u'; } break;
                case Keys.V: if (shift) { key = 'V'; } else { key = 'v'; } break;
                case Keys.W: if (shift) { key = 'W'; } else { key = 'w'; } break;
                case Keys.X: if (shift) { key = 'X'; } else { key = 'x'; } break;
                case Keys.Y: if (shift) { key = 'Y'; } else { key = 'y'; } break;
                case Keys.Z: if (shift) { key = 'Z'; } else { key = 'z'; } break;
                case Keys.D0: if (shift) { key = ')'; } else { key = '0'; } break;
                case Keys.D1: if (shift) { key = '!'; } else { key = '1'; } break;
                case Keys.D2: if (shift) { key = '@'; } else { key = '2'; } break;
                case Keys.D3: if (shift) { key = '#'; } else { key = '3'; } break;
                case Keys.D4: if (shift) { key = '$'; } else { key = '4'; } break;
                case Keys.D5: if (shift) { key = '%'; } else { key = '5'; } break;
                case Keys.D6: if (shift) { key = '^'; } else { key = '6'; } break;
                case Keys.D7: if (shift) { key = '&'; } else { key = '7'; } break;
                case Keys.D8: if (shift) { key = '*'; } else { key = '8'; } break;
                case Keys.D9: if (shift) { key = '('; } else { key = '9'; } break;
                case Keys.NumPad0: key = '0'; break;
                case Keys.NumPad1: key = '1'; break;
                case Keys.NumPad2: key = '2'; break;
                case Keys.NumPad3: key = '3'; break;
                case Keys.NumPad4: key = '4'; break;
                case Keys.NumPad5: key = '5'; break;
                case Keys.NumPad6: key = '6'; break;
                case Keys.NumPad7: key = '7'; break;
                case Keys.NumPad8: key = '8'; break;
                case Keys.NumPad9: key = '9'; break;
                case Keys.OemTilde: if (shift) { key = '~'; } else { key = '`'; } break;
                case Keys.OemSemicolon: if (shift) { key = ':'; } else { key = ';'; } break;
                case Keys.OemQuotes: if (shift) { key = '"'; } else { key = '\''; } break;
                case Keys.OemQuestion: if (shift) { key = '?'; } else { key = '/'; } break;
                case Keys.OemPlus: if (shift) { key = '+'; } else { key = '='; } break;
                case Keys.OemPipe: if (shift) { key = '|'; } else { key = '\\'; } break;
                case Keys.OemPeriod: if (shift) { key = '>'; } else { key = '.'; } break;
                case Keys.OemOpenBrackets: if (shift) { key = '{'; } else { key = '['; } break;
                case Keys.OemCloseBrackets: if (shift) { key = '}'; } else { key = ']'; } break;
                case Keys.OemMinus: if (shift) { key = '_'; } else { key = '-'; } break;
                case Keys.OemComma: if (shift) { key = '<'; } else { key = ','; } break;
                case Keys.Space: key = ' '; break;
                default: return;
            }

            if (this.MaxLength <= 0 || this.Text.Length < this.MaxLength)
            {
                this.Text = this.Text + key;
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
    }
}
