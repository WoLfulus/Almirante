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

namespace Almirante.Engine.Input
{
    using Almirante.Engine.Core;

    /// <summary>
    /// Input key class.
    /// </summary>
    public class InputKey
    {
        /// <summary>
        /// Stores the time when the user pressed the button.
        /// </summary>
        private double timePressed;

        /// <summary>
        /// Stores the time when the user released the button.
        /// </summary>
        private double timeReleased;

        /// <summary>
        /// Gets whether the user has just pressed the input or not.
        /// </summary>
        public bool Pressed
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets whether the user is holding the input down or not.
        /// </summary>
        public bool Down
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets a value indicating whether the user has just released the input or not.
        /// </summary>
        public bool Released
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets how much time the user is holding the input down.
        /// </summary>
        public double HoldTime
        {
            get;
            internal set;
        }

        /// <summary>
        /// Updates the current input.
        /// </summary>
        /// <param name="down">Indicates whether the current input is down.</param>
        public void Update(bool down)
        {
            if (down)
            {
                if (this.Pressed)
                {
                    this.Pressed = false;
                }
                else if (!this.Down)
                {
                    this.timePressed = AlmiranteEngine.Time.Total;
                    this.Pressed = true;
                }

                this.Down = true;
                this.Released = false;
            }
            else
            {
                if (this.Released)
                {
                    this.Released = false;
                }
                else if (this.Down)
                {
                    this.timeReleased = AlmiranteEngine.Time.Total;
                    this.Released = true;
                }

                this.Down = false;
                this.Pressed = false;
            }

            this.HoldTime = this.Down ? (AlmiranteEngine.Time.Total - this.timePressed) : (this.timeReleased - this.timePressed);
        }
    }
}