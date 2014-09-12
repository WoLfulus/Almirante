/// 
/// The MIT License (MIT)
/// 
/// Copyright (c) 2014 João Francisco Biondo Trinca <wolfulus@gmail.com>
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in
/// all copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
/// THE SOFTWARE.
/// 

namespace Almirante.Engine.Input.Devices
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework.Input;
    using Message = System.Windows.Forms.Message;

    /// <summary>
    /// Class that hold the states of a keyboard
    /// </summary>
    public class KeyboardDevice : IInputDevice
    {
        /// <summary>
        /// Stores all keyboard device keys.
        /// </summary>
        private readonly Dictionary<Keys, KeyboardKey> keys;

        /// <summary>
        /// Stores the current keyboard state.
        /// </summary>
        private KeyboardState state;

        /// <summary>
        /// Creates a new instance of the <see cref="KeyboardDevice"/> class.
        /// </summary>
        internal KeyboardDevice()
        {
            this.keys = new Dictionary<Keys, KeyboardKey>();
            for (int i = 0; i < 256; i++) // foreach (Keys k in Enum.GetValues(typeof(Keys)))
            {
                this.keys.Add((Keys)i, new KeyboardKey((Keys)i));
            }
        }

        /// <summary>
        /// Keyboard event delegate.
        /// </summary>
        /// <param name="key">Key.</param>
        public delegate void KeyboardEvent(KeyboardKey key);

        /// <summary>
        /// Keyboard message event delegate.
        /// </summary>
        /// <param name="msg">Message data.</param>
        public delegate void KeyboardMessageEvent(Message msg);

        /// <summary>
        /// Key press event.
        /// </summary>
        public event KeyboardEvent Pressed;

        /// <summary>
        /// Key press event.
        /// </summary>
        public event KeyboardEvent Released;

        /// <summary>
        /// Gets the key state for the specified keyboard key.
        /// </summary>
        /// <param name="key">Requested key.</param>
        /// <returns>State of the requested key.</returns>
        public KeyboardKey this[Keys key]
        {
            get
            {
                return this.keys[key];
            }
            internal set
            {
                this.keys[key] = value;
            }
        }

        /// <summary>
        /// Update the keyboard keys state
        /// </summary>
        public void Update()
        {
            // this.lastState = this.state;
            this.state = Keyboard.GetState();

            foreach (var key in this.keys.Keys)
            {
                this[key].Update(this.state.IsKeyDown(key));
                if (this[key].Pressed)
                {
                    if (this.Pressed != null)
                    {
                        this.Pressed(this[key]);
                    }
                }
                else if (this[key].Released)
                {
                    if (this.Released != null)
                    {
                        this.Released(this[key]);
                    }
                }
            }
        }
    }
}