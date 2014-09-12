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
    using System.Collections.Generic;
    using Almirante.Engine.Tweens.States;

    /// <summary>
    /// Tweener class.
    /// </summary>
    public class ValueTweener
    {
        /// <summary>
        /// Stores the states.
        /// </summary>
        private readonly LinkedList<IState> states = new LinkedList<IState>();

        /// <summary>
        /// Stores the current state.
        /// </summary>
        private LinkedListNode<IState> currentState = null;

        /// <summary>
        /// Stores the initial value.
        /// </summary>
        private readonly float start/* = 0.0f*/;

        /// <summary>
        /// Stores the final value.
        /// </summary>
        private readonly float finish/* = 0.0f*/;

        /// <summary>
        /// Stores the current tween function.
        /// </summary>
        private readonly MotionTweens.TweenFunction tween = MotionTweens.Linear;

        /// <summary>
        /// User object data.
        /// </summary>
        private readonly object data = null;

        /// <summary>
        /// Stores a value that indicates whether the tweener is running or not.
        /// </summary>
        private bool running/* = false*/;

        /// <summary>
        /// Stores the current tween value.
        /// </summary>
        private float value/* = 0.0f*/;

        /// <summary>
        /// Stores if the current tweener has finished.
        /// </summary>
        private bool finished/* = false*/;

        /// <summary>
        /// Creates a new <see cref="ValueTweener"/> class instance.
        /// </summary>
        /// <param name="start">Initial value</param>
        /// <param name="finish">Final value</param>
        public ValueTweener(float start, float finish)
        {
            this.start = start;
            this.finish = finish;
            this.tween = MotionTweens.Linear;
        }

        /// <summary>
        /// Creates a new <see cref="ValueTweener"/> class instance.
        /// </summary>
        /// <param name="start">Initial value</param>
        /// <param name="finish">Final value</param>
        /// <param name="data">User data</param>
        public ValueTweener(float start, float finish, object data)
        {
            this.start = start;
            this.finish = finish;
            this.tween = MotionTweens.Linear;
            this.data = data;
        }

        /// <summary>
        /// Creates a new <see cref="ValueTweener"/> class instance.
        /// </summary>
        /// <param name="start">Initial value</param>
        /// <param name="finish">Final value</param>
        /// <param name="tween">Easing function</param>
        /// <param name="data">User data</param>
        public ValueTweener(float start, float finish, object data, MotionTweens.TweenFunction tween)
        {
            this.start = start;
            this.finish = finish;
            this.tween = tween ?? MotionTweens.Linear;
            this.data = data;
        }

        /// <summary>
        /// Delegate used to dispatch callbacks when a tweener state changes.
        /// </summary>
        /// <param name="sender">Tweener instance.</param>
        public delegate void TweenerFinished(ValueTweener sender);

        /// <summary>
        /// Delegate used to dispatch callbacks when a tweener value changes.
        /// </summary>
        /// <param name="sender">Tweener instance.</param>
        /// <param name="data">The data.</param>
        public delegate void TweenerValueChanged(ValueTweener sender, object data);

        /// <summary>
        /// Event used to dispatch tweener events.
        /// </summary>
        public event TweenerValueChanged ValueChanged;

        /// <summary>
        /// Event used to dispatch tweener events.
        /// </summary>
        public event TweenerFinished Finished;

        /// <summary>
        /// Gets the current tweener value.
        /// </summary>
        public float Value
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
        public ValueTweener Forward(float duration)
        {
            return this.RegisterState(new MoveState()
            {
                Start = this.start,
                Finish = this.finish,
                Duration = duration,
                Tween = this.tween
            });
        }

        /// <summary>
        /// Creates a forward movement state.
        /// </summary>
        /// <param name="duration">State duration time in seconds.</param>
        /// <param name="tween">Tween function for this state.</param>
        /// <returns>Current tweener instance for fluent configuration.</returns>
        public ValueTweener Forward(float duration, MotionTweens.TweenFunction tween)
        {
            return this.RegisterState(new MoveState()
            {
                Start = this.start,
                Finish = this.finish,
                Duration = duration,
                Tween = tween
            });
        }

        /// <summary>
        /// Creates a forward movement state.
        /// </summary>
        /// <param name="duration">State duration time in seconds.</param>
        /// <returns>Current tweener instance for fluent configuration.</returns>
        public ValueTweener Backward(float duration)
        {
            return this.RegisterState(new MoveState()
            {
                Start = this.finish,
                Finish = this.start,
                Duration = duration,
                Tween = this.tween
            });
        }

        /// <summary>
        /// Creates a forward movement state.
        /// </summary>
        /// <param name="duration">State duration time in seconds.</param>
        /// <param name="tween">Tween function for this state.</param>
        /// <returns>Current tweener instance for fluent configuration.</returns>
        public ValueTweener Backward(float duration, MotionTweens.TweenFunction tween)
        {
            return this.RegisterState(new MoveState()
            {
                Start = this.finish,
                Finish = this.start,
                Duration = duration,
                Tween = tween
            });
        }

        /// <summary>
        /// Creates a forward movement state.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>
        /// Current tweener instance for fluent configuration.
        /// </returns>
        public ValueTweener Wait(float duration)
        {
            return this.RegisterState(new WaitState()
            {
                Duration = duration
            });
        }

        /// <summary>
        /// Creates an event callback.
        /// </summary>
        /// <param name="act">
        /// Action callback.
        /// </param>
        /// <returns>Current tweener instance for fluent configuration.</returns>
        public ValueTweener Action(Action act)
        {
            if (act != null)
            {
                this.RegisterState(new ActionState()
                {
                    Action = act
                });
            }
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
        public ValueTweener Repeat(int count = -1)
        {
            if (count != 0)
            {
                this.RegisterState(new RepeatState()
                {
                    Count = count
                });
            }
            return this;
        }

        /// <summary>
        /// Resets all the states.
        /// </summary>
        public void Reset()
        {
            var node = this.states.First;
            while (node != null)
            {
                node.Value.Reset();
                node = node.Next;
            }
            this.currentState = this.states.First;
            this.finished = false;
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
            this.running = false;
        }

        /// <summary>
        /// Resumes the tweener.
        /// </summary>
        public void Resume()
        {
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
            if (this.finished || !this.running)
            {
                return;
            }

            if (this.currentState != null)
            {
                if (this.currentState.Value.Update(this, time))
                {
                    this.currentState.Value.Leave();

                    if (this.currentState.Value is RepeatState)
                    {
                        RepeatState state = this.currentState.Value as RepeatState;
                        if (state.Count == 0)
                        {
                            this.currentState = this.currentState.Next;
                        }
                        else
                        {
                            this.currentState = this.states.First;
                        }
                    }
                    else
                    {
                        this.currentState = this.currentState.Next;
                    }

                    if (this.currentState == null)
                    {
                        this.finished = true;
                        if (this.Finished != null)
                        {
                            this.Finished(this);
                        }
                    }
                    else
                    {
                        this.currentState.Value.Enter();
                        if (this.currentState.Value is ActionState)
                        {
                            this.currentState = this.currentState.Next;
                            if (this.currentState == null)
                            {
                                this.finished = true;
                                if (this.Finished != null)
                                {
                                    this.Finished(this);
                                }
                            }
                            else
                            {
                                this.currentState.Value.Enter();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Registers a new tween state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private ValueTweener RegisterState(IState state)
        {
            if (this.currentState == null)
            {
                this.currentState = this.states.AddFirst(state);
            }
            else
            {
                this.states.AddAfter(this.states.Last, state);
            }
            return this;
        }
    }
}