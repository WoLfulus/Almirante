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

namespace Almirante.Engine.Tweens.States
{
    /// <summary>
    /// Repeat motion tween state.
    /// </summary>
    public class RepeatState : IState
    {
        /// <summary>
        /// Stores the number of repetitions.
        /// </summary>
        private int saveCount;

        /// <summary>
        /// Current number of repetitions.
        /// </summary>
        private int count;

        /// <summary>
        /// Creates a new <see cref="RepeatState"/> instance.
        /// </summary>
        public RepeatState()
        {
        }

        /// <summary>
        /// Gets the current number of repetitions.
        /// </summary>
        public int Count
        {
            get
            {
                return this.count;
            }
            internal set
            {
                this.saveCount = value;
                this.count = value;
            }
        }

        /// <summary>
        /// Activates the current state.
        /// </summary>
        public void Enter()
        {
            this.count--;
        }

        /// <summary>
        /// Leaves the current state.
        /// </summary>
        public void Leave()
        {
        }

        /// <summary>
        /// Resets the current state.
        /// </summary>
        public void Reset()
        {
            this.count = this.saveCount;
        }

        /// <summary>
        /// Updates the current state.
        /// </summary>
        /// <param name="tweener">Current tweener instance.</param>
        /// <param name="time">Elapsed time since last frame in seconds.</param>
        /// <returns>
        /// True if the state has been cleared.
        /// </returns>
        public bool Update(ValueTweener tweener, float time)
        {
            return true;
        }
    }
}