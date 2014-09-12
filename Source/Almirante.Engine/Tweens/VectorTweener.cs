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

namespace Almirante.Engine.Tweens
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Tweener class.
    /// </summary>
    public class VectorTweener
    {
        /// <summary>
        /// Stores if the current tweener has finished.
        /// </summary>
        private readonly bool finished/* = false*/;

        /// <summary>
        /// Stores the current tween function.
        /// </summary>
        private readonly MotionTweens.TweenFunction tween = MotionTweens.Linear;

        /// <summary>
        /// User object data.
        /// </summary>
        private readonly object data = null;

        /// <summary>
        /// Tweener for x axis.
        /// </summary>
        private readonly ValueTweener xTweener;

        /// <summary>
        /// Tweener for y axis.
        /// </summary>
        private readonly ValueTweener yTweener;

        /// <summary>
        /// Stores a value indicating whether the tweener is running or not.
        /// </summary>
        private bool running/* = false*/;

        /// <summary>
        /// Stores the current tween value.
        /// </summary>
        private Vector2 value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueTweener"/> class.
        /// </summary>
        /// <param name="start">Initial value</param>
        /// <param name="finish">Final value</param>
        public VectorTweener(Vector2 start, Vector2 finish)
        {
            this.tween = MotionTweens.Linear;

            this.xTweener = new ValueTweener(start.X, finish.X, null, this.tween);
            this.xTweener.ValueChanged += (t, d) =>
            {
                this.value.X = t.Value;
            };

            this.yTweener = new ValueTweener(start.Y, finish.Y, null, this.tween);
            this.yTweener.ValueChanged += (t, d) =>
            {
                this.value.Y = t.Value;
            };
        }

        /// <summary>
        /// Creates a new <see cref="ValueTweener"/> class instance.
        /// </summary>
        /// <param name="start">Initial value</param>
        /// <param name="finish">Final value</param>
        /// <param name="data">User data</param>
        public VectorTweener(Vector2 start, Vector2 finish, object data)
        {
            this.tween = MotionTweens.Linear;
            this.data = data;

            this.xTweener = new ValueTweener(start.X, finish.X, null, this.tween);
            this.xTweener.ValueChanged += (t, d) =>
            {
                this.value.X = t.Value;
            };

            this.yTweener = new ValueTweener(start.Y, finish.Y, null, this.tween);
            this.yTweener.ValueChanged += (t, d) =>
            {
                this.value.Y = t.Value;
            };
        }

        /// <summary>
        /// Creates a new <see cref="ValueTweener"/> class instance.
        /// </summary>
        /// <param name="start">Initial value</param>
        /// <param name="finish">Final value</param>
        /// <param name="tween">Easing function</param>
        /// <param name="data">User data</param>
        public VectorTweener(Vector2 start, Vector2 finish, object data, MotionTweens.TweenFunction tween)
        {
            this.tween = tween ?? MotionTweens.Linear;
            this.data = data;

            this.xTweener = new ValueTweener(start.X, finish.X, null, tween);
            this.xTweener.ValueChanged += (t, d) =>
            {
                this.value.X = t.Value;
            };

            this.yTweener = new ValueTweener(start.Y, finish.Y, null, tween);
            this.yTweener.ValueChanged += (t, d) =>
            {
                this.value.Y = t.Value;
            };
        }

        /// <summary>
        /// Delegate used to dispatch callbacks when a tweener value changes.
        /// </summary>
        /// <param name="sender">Tweener instance.</param>
        /// <param name="data">The data.</param>
        public delegate void VectorTweenerValueChanged(VectorTweener sender, object data);

        /// <summary>
        /// Delegate used to dispatch callbacks when a tweener state changes.
        /// </summary>
        /// <param name="sender">Tweener instance.</param>
        public delegate void VectorTweenerFinished(VectorTweener sender);

        /// <summary>
        /// Event used to dispatch tweener events.
        /// </summary>
        public event VectorTweenerValueChanged ValueChanged;

        /// <summary>
        /// Gets the current tweener value.
        /// </summary>
        public Vector2 Value
        {
            get
            {
                return this.value;
            }
            internal set
            {
                this.value = value;
                if (this.ValueChanged != null)
                {
                    this.ValueChanged(this, this.data);
                }
            }
        }

        /// <summary>
        /// Gets whether the current tween has finished.
        /// </summary>
        public bool IsFinished
        {
            get
            {
                return this.finished;
            }
        }

        /// <summary>
        /// Gets the user-defined data.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <returns>User-defined data.</returns>
        public T GetDataValue<T>()
            where T : struct
        {
            return (T)this.data;
        }

        /// <summary>
        /// Gets the user-defined data.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <returns>User-defined data.</returns>
        public T GetValue<T>()
            where T : class
        {
            return this.data as T;
        }

        /// <summary>
        /// Creates a forward movement state.
        /// </summary>
        /// <param name="duration">State duration time in seconds.</param>
        /// <returns>Current tweener instance for fluent configuration.</returns>
        public VectorTweener Forward(float duration)
        {
            this.xTweener.Forward(duration);
            this.yTweener.Forward(duration);
            return this;
        }

        /// <summary>
        /// Creates a forward movement state.
        /// </summary>
        /// <param name="duration">State duration time in seconds.</param>
        /// <param name="tween">Tween function for this state.</param>
        /// <returns>Current tweener instance for fluent configuration.</returns>
        public VectorTweener Forward(float duration, MotionTweens.TweenFunction tween)
        {
            this.xTweener.Forward(duration, tween);
            this.yTweener.Forward(duration, tween);
            return this;
        }

        /// <summary>
        /// Creates a forward movement state.
        /// </summary>
        /// <param name="duration">State duration time in seconds.</param>
        /// <param name="tweenx">Tween function for this state (x axis).</param>
        /// <param name="tweeny">Tween function for this state (y axis).</param>
        /// <returns>Current tweener instance for fluent configuration.</returns>
        public VectorTweener Forward(float duration, MotionTweens.TweenFunction tweenx, MotionTweens.TweenFunction tweeny)
        {
            this.xTweener.Forward(duration, tweenx);
            this.yTweener.Forward(duration, tweeny);
            return this;
        }

        /// <summary>
        /// Creates a forward movement state.
        /// </summary>
        /// <param name="duration">State duration time in seconds.</param>
        /// <returns>Current tweener instance for fluent configuration.</returns>
        public VectorTweener Backward(float duration)
        {
            this.xTweener.Backward(duration);
            this.yTweener.Backward(duration);
            return this;
        }

        /// <summary>
        /// Creates a forward movement state.
        /// </summary>
        /// <param name="duration">State duration time in seconds.</param>
        /// <param name="tween">Tween function for this state.</param>
        /// <returns>Current tweener instance for fluent configuration.</returns>
        public VectorTweener Backward(float duration, MotionTweens.TweenFunction tween)
        {
            this.xTweener.Backward(duration, tween);
            this.yTweener.Backward(duration, tween);
            return this;
        }

        /// <summary>
        /// Creates a forward movement state.
        /// </summary>
        /// <param name="duration">State duration time in seconds.</param>
        /// <param name="tweenx">Tween function for this state (x axis).</param>
        /// <param name="tweeny">Tween function for this state (y axis).</param>
        /// <returns>Current tweener instance for fluent configuration.</returns>
        public VectorTweener Backward(float duration, MotionTweens.TweenFunction tweenx, MotionTweens.TweenFunction tweeny)
        {
            this.xTweener.Backward(duration, tweenx);
            this.yTweener.Backward(duration, tweeny);
            return this;
        }

        /// <summary>
        /// Creates a forward movement state.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>
        /// Current tweener instance for fluent configuration.
        /// </returns>
        public VectorTweener Wait(float duration)
        {
            this.xTweener.Wait(duration);
            this.yTweener.Wait(duration);
            return this;
        }

        /// <summary>
        /// Creates an event callback.
        /// </summary>
        /// <param name="act">Action callback.</param>
        /// <returns>
        /// Current tweener instance for fluent configuration.
        /// </returns>
        public VectorTweener Action(Action act)
        {
            this.xTweener.Action(act);
            return this;
        }

        /// <summary>
        /// Creates a repeat state.
        /// </summary>
        /// <param name="count">
        /// Repetition count. <br/>
        /// count &lt; 0: repeat forever.<br/>
        /// count &gt; 0: number of times to repeat.<br/>
        /// count = 0: ignored.
        /// </param>
        /// <returns>Current tweener instance for fluent configuration.</returns>
        public VectorTweener Repeat(int count = -1)
        {
            this.xTweener.Repeat(count);
            this.yTweener.Repeat(count);
            return this;
        }

        /// <summary>
        /// Resets all the states.
        /// </summary>
        public void Reset()
        {
            this.xTweener.Reset();
            this.yTweener.Reset();
        }

        /// <summary>
        /// Starts the tweener.
        /// </summary>
        public void Start()
        {
            this.Reset();
            this.Resume();
        }

        /// <summary>
        /// Stops the tweener.
        /// </summary>
        public void Stop()
        {
            this.Reset();
            this.Pause();
        }

        /// <summary>
        /// Pauses the tweener.
        /// </summary>
        public void Pause()
        {
            this.xTweener.Pause();
            this.yTweener.Pause();
            this.running = false;
        }

        /// <summary>
        /// Resumes the tweener.
        /// </summary>
        public void Resume()
        {
            this.xTweener.Resume();
            this.yTweener.Resume();
            this.running = true;
        }

        /// <summary>
        /// Restarts the tweener.
        /// </summary>
        public void Restart()
        {
            this.Start();
        }

        /// <summary>
        /// Updates the current tween.
        /// </summary>
        /// <param name="time">Elapsed time in seconds.</param>
        public void Update(double time)
        {
            this.Update((float)time);
        }

        /// <summary>
        /// Updates the current tween.
        /// </summary>
        /// <param name="time">Elapsed time in seconds.</param>
        public void Update(float time)
        {
            if ((this.xTweener.IsFinished && this.yTweener.IsFinished) || !this.running)
            {
                return;
            }

            this.xTweener.Update(time);
            this.yTweener.Update(time);

            this.Value = this.value;
        }
    }
}