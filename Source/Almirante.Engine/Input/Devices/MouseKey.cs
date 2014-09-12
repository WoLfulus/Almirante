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
    /// <summary>
    /// Mouse keys.
    /// </summary>
    public class MouseKey : InputKey
    {
        /// <summary>
        /// Creates a new instance of the <see cref="MouseKey" /> class.
        /// </summary>
        /// <param name="key">Current key index.</param>
        internal MouseKey(MouseButton key)
        {
            this.Key = key;
        }

        /// <summary>
        /// Gets the related mouse key index.
        /// </summary>
        public MouseButton Key
        {
            get;
            internal set;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Almirante.Engine.Input.Devices.MouseKey" /> to <see cref="System.Boolean" />.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator bool(MouseKey key)
        {
            return key.Down;
        }
    }
}