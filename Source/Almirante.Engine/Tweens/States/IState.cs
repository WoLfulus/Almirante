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
    /// Tweener state.
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Activates the current state.
        /// </summary>
        void Enter();

        /// <summary>
        /// Leaves the current state.
        /// </summary>
        void Leave();

        /// <summary>
        /// Resets the current state.
        /// </summary>
        void Reset();

        /// <summary>
        /// Updates the current state.
        /// </summary>
        /// <param name="tweener">Current tweener instance.</param>
        /// <param name="time">Elapsed time since last frame in seconds.</param>
        /// <returns>True if the state has been cleared.</returns>
        bool Update(ValueTweener tweener, float time);
    }
}