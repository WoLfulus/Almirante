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

namespace Almirante.Engine.Interface
{
    using System;
    using Almirante.Engine.Input.Devices;

    /// <summary>
    /// Mouse event args class.
    /// </summary>
    public class MouseEventArgs : ControlEventArgs
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        public MouseKey Key
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the scroll amount.
        /// </summary>
        /// <value>
        /// The scroll.
        /// </value>
        public int Scroll
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseEventArgs"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="key">The key.</param>
        /// <param name="amount">The amount.</param>
        public MouseEventArgs(double time, MouseKey key, int amount)
            : base(time)
        {
            this.Scroll = amount;
            this.Key = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseEventArgs"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="key">The key.</param>
        public MouseEventArgs(double time, MouseKey key)
            : base(time)
        {
            this.Scroll = 0;
            this.Key = key;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseEventArgs"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public MouseEventArgs(MouseKey key)
            : base()
        {
            this.Scroll = 0;
            this.Key = key;
        }
    }
}