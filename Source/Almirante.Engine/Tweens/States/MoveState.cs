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
    /// Forward motion tween state.
    /// </summary>
    public class MoveState : IState
    {
        /// <summary>
        /// Stores the elapsed time.
        /// </summary>
        private float elapsed;

        /// <summary>
        /// Creates a new <see cref="MoveState"/> instance.
        /// </summary>
        public MoveState()
        {
        }

        /// <summary>
        /// Gets the start value.
        /// </summary>
        public float Start
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the final value.
        /// </summary>
        public float Finish
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the current state duration in seconds.
        /// </summary>
        public float Duration
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the tween function.
        /// </summary>
        public MotionTweens.TweenFunction Tween
        {
            get;
            internal set;
        }

        /// <summary>
        /// Activates the current state.
        /// </summary>
        public void Enter()
        {
            this.Reset();
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
            this.elapsed = 0.0f;
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
            this.elapsed += time;

            var newValue = this.Tween(this.elapsed, this.Start, this.Finish - this.Start, this.Duration);
            if (this.elapsed >= this.Duration)
            {
                tweener.Value = this.Finish;
                return true;
            }
            else
            {
                tweener.Value = newValue;
            }

            return false;
        }
    }
}