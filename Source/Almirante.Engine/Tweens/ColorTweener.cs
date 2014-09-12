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

    /// <summary>
    /// Tweener class.
    /// </summary>
    public class ColorTweener
    {
        /// <summary>
        /// Stores the initial value.
        /// </summary>
        private readonly Microsoft.Xna.Framework.Color start;

        /// <summary>
        /// Stores the final value.
        /// </summary>
        private readonly Microsoft.Xna.Framework.Color finish;

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
        /// Stores a value indicating whether the current tweener is running or not.
        /// </summary>
        private bool running/* = false*/;

        /// <summary>
        /// Stores the current tween value.
        /// </summary>
        private Microsoft.Xna.Framework.Color value;

        /// <summary>
        /// Tweener for red color.
        /// </summary>
        private ValueTweener rTweener;

        /// <summary>
        /// Tweener for green color.
        /// </summary>
        private ValueTweener gTweener;

        /// <summary>
        /// Tweener for blue color.
        /// </summary>
        private ValueTweener bTweener;

        /// <summary>
        /// Tweener for alpha channel.
        /// </summary>
        private ValueTweener aTweener;

        /// <summary>
        /// Creates a new <see cref="ColorTweener"/> class instance.
        /// </summary>
        /// <param name="start">Initial value</param>
        /// <param name="finish">Final value</param>
        public ColorTweener(Microsoft.Xna.Framework.Color start, Microsoft.Xna.Framework.Color finish)
        {
            this.start = start;
            this.finish = finish;
            this.tween = MotionTweens.Linear;

            this.CreateTweens();
        }

        /// <summary>
        /// Creates a new <see cref="ColorTweener"/> class instance.
        /// </summary>
        /// <param name="start">Initial value</param>
        /// <param name="finish">Final value</param>
        /// <param name="data">User data</param>
        public ColorTweener(Microsoft.Xna.Framework.Color start, Microsoft.Xna.Framework.Color finish, object data)
        {
            this.start = start;
            this.finish = finish;
            this.tween = MotionTweens.Linear;
            this.data = data;

            this.CreateTweens();
        }

        /// <summary>
        /// Creates a new <see cref="ColorTweener"/> class instance.
        /// </summary>
        /// <param name="start">Initial value</param>
        /// <param name="finish">Final value</param>
        /// <param name="tween">Easing function</param>
        /// <param name="data">User data</param>
        public ColorTweener(Microsoft.Xna.Framework.Color start, Microsoft.Xna.Framework.Color finish, object data, MotionTweens.TweenFunction tween)
        {
            this.start = start;
            this.finish = finish;
            this.tween = tween ?? MotionTweens.Linear;
            this.data = data;

            this.CreateTweens();
        }

        /// <summary>
        /// Delegate used to dispatch callbacks when a tweener value changes.
        /// </summary>
        /// <param name="sender">Tweener instance.</param>
        /// <param name="data">The data.</param>
        public delegate void ColorTweenerValueChanged(ColorTweener sender, object data);

        /// <summary>
        /// Delegate used to dispatch callbacks when a tweener state changes.
        /// </summary>
        /// <param name="sender">Tweener instance.</param>
        public delegate void ColorTweenerFinished(ColorTweener sender);

        /// <summary>
        /// Event used to dispatch tweener events.
        /// </summary>
        public event ColorTweenerValueChanged ValueChanged;

        /// <summary>
        /// Gets the current tweener value.
        /// </summary>
        public Microsoft.Xna.Framework.Color Value
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
        public ColorTweener Forward(float duration)
        {
            this.rTweener.Forward(duration);
            this.gTweener.Forward(duration);
            this.bTweener.Forward(duration);
            this.aTweener.Forward(duration);
            return this;
        }

        /// <summary>
        /// Creates a forward movement state.
        /// </summary>
        /// <param name="duration">State duration time in seconds.</param>
        /// <param name="tween">Tween function for this state.</param>
        /// <returns>Current tweener instance for fluent configuration.</returns>
        public ColorTweener Forward(float duration, MotionTweens.TweenFunction tween)
        {
            this.rTweener.Forward(duration, tween);
            this.gTweener.Forward(duration, tween);
            this.bTweener.Forward(duration, tween);
            this.aTweener.Forward(duration, tween);
            return this;
        }

        /// <summary>
        /// Creates a forward movement state.
        /// </summary>
        /// <param name="duration">State duration time in seconds.</param>
        /// <returns>Current tweener instance for fluent configuration.</returns>
        public ColorTweener Backward(float duration)
        {
            this.rTweener.Backward(duration);
            this.gTweener.Backward(duration);
            this.bTweener.Backward(duration);
            this.aTweener.Backward(duration);
            return this;
        }

        /// <summary>
        /// Creates a forward movement state.
        /// </summary>
        /// <param name="duration">State duration time in seconds.</param>
        /// <param name="tween">Tween function for this state.</param>
        /// <returns>Current tweener instance for fluent configuration.</returns>
        public ColorTweener Backward(float duration, MotionTweens.TweenFunction tween)
        {
            this.rTweener.Backward(duration, tween);
            this.gTweener.Backward(duration, tween);
            this.bTweener.Backward(duration, tween);
            this.aTweener.Backward(duration, tween);
            return this;
        }

        /// <summary>
        /// Creates a forward movement state.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>
        /// Current tweener instance for fluent configuration.
        /// </returns>
        public ColorTweener Wait(float duration)
        {
            this.rTweener.Wait(duration);
            this.gTweener.Wait(duration);
            this.bTweener.Wait(duration);
            this.aTweener.Wait(duration);
            return this;
        }

        /// <summary>
        /// Creates an event callback.
        /// </summary>
        /// <param name="act">Action callback.</param>
        /// <returns>
        /// Current tweener instance for fluent configuration.
        /// </returns>
        public ColorTweener Action(Action act)
        {
            this.rTweener.Action(act);
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
        public ColorTweener Repeat(int count = -1)
        {
            this.rTweener.Repeat(count);
            this.gTweener.Repeat(count);
            this.bTweener.Repeat(count);
            this.aTweener.Repeat(count);
            return this;
        }

        /// <summary>
        /// Resets all the states.
        /// </summary>
        public void Reset()
        {
            this.rTweener.Reset();
            this.gTweener.Reset();
            this.bTweener.Reset();
            this.aTweener.Reset();
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
            this.rTweener.Pause();
            this.gTweener.Pause();
            this.bTweener.Pause();
            this.aTweener.Pause();
            this.running = false;
        }

        /// <summary>
        /// Resumes the tweener.
        /// </summary>
        public void Resume()
        {
            this.rTweener.Resume();
            this.gTweener.Resume();
            this.bTweener.Resume();
            this.aTweener.Resume();
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
            if ((this.rTweener.IsFinished && this.gTweener.IsFinished && this.bTweener.IsFinished && this.aTweener.IsFinished) || !this.running)
            {
                return;
            }

            this.rTweener.Update(time);
            this.gTweener.Update(time);
            this.bTweener.Update(time);
            this.aTweener.Update(time);

            this.Value = this.value;
        }

        /// <summary>
        /// Creates all motion tweens.
        /// </summary>
        private void CreateTweens()
        {
            this.rTweener = new ValueTweener(this.start.R, this.finish.R, null, this.tween);
            this.rTweener.ValueChanged += (t, d) =>
            {
                this.value.R = (byte)t.Value;
            };

            this.gTweener = new ValueTweener(this.start.G, this.finish.G, null, this.tween);
            this.gTweener.ValueChanged += (t, d) =>
            {
                this.value.G = (byte)t.Value;
            };

            this.bTweener = new ValueTweener(this.start.B, this.finish.B, null, this.tween);
            this.bTweener.ValueChanged += (t, d) =>
            {
                this.value.B = (byte)t.Value;
            };

            this.aTweener = new ValueTweener(this.start.A, this.finish.A, null, this.tween);
            this.aTweener.ValueChanged += (t, d) =>
            {
                this.value.A = (byte)t.Value;
            };
        }
    }
}