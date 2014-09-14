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

namespace Almirante.Engine.Scenes
{
    using System.Globalization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Almirante.Engine.Core;
    using Almirante.Engine.Tweens;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Screen manager.
    /// </summary>
    public class SceneManager : IDisposable
    {
        /// <summary>
        /// Stores a list of registered screens.
        /// </summary>
        private readonly Dictionary<Type, Scene> screens = new Dictionary<Type, Scene>();

        /// <summary>
        /// List of current transitions.
        /// </summary>
        private readonly Dictionary<string, Transition> transitions = new Dictionary<string, Transition>();

        /// <summary>
        /// List of current activated screens.
        /// </summary>
        private readonly Stack<Scene> current = new Stack<Scene>();

        /// <summary>
        /// Next scene.
        /// </summary>
        private Scene nextScene = null;

        /// <summary>
        /// Current transition effect.
        /// </summary>
        private Transition transition = null;

        /// <summary>
        /// Current transition type.
        /// </summary>
        private TransitionType transitionType = TransitionType.None;

        /// <summary>
        /// Transition tweener.
        /// </summary>
        private ValueTweener transitionTween;

        /// <summary>
        /// Previous scene target.
        /// </summary>
        internal RenderTarget2D renderTarget;

        /// <summary>
        /// Previous scene target.
        /// </summary>
        internal RenderTarget2D renderTargetNext;

        /// <summary>
        /// Creates a new instance of the <see cref="SceneManager"/> class.
        /// </summary>
        internal SceneManager()
        {
        }

        #region Registering Methods

        /// <summary>
        /// Registers a scene into the manager.
        /// </summary>
        /// <typeparam name="T">Type of the scene</typeparam>
        internal void Register<T>(bool startup)
            where T : Scene, new()
        {
            lock (this)
            {
                if (!this.screens.ContainsKey(typeof(T)))
                {
                    var screen = new T();
                    if (startup || this.current.Count == 0)
                    {
                        this.current.Clear();
                        this.current.Push(screen);
                    }
                    this.screens.Add(typeof(T), screen);
                }
            }
        }

        /// <summary>
        /// Registers a scene into the manager.
        /// </summary>
        /// <param name="type">Type of the scene.</param>
        /// <param name="startup">Is it the startup scene?</param>
        internal void Register(Type type, bool startup)
        {
            lock (this)
            {
                if (!this.screens.ContainsKey(type))
                {
                    var constructor = type.GetConstructor(new Type[0]);
                    if (constructor != null)
                    {
                        var screen = (Scene)constructor.Invoke(new object[] { });
                        if (startup || this.current.Count == 0)
                        {
                            this.current.Clear();
                            this.current.Push(screen);
                        }
                        this.screens.Add(type, screen);
                    }
                }
            }
        }

        /// <summary>
        /// Registers a scene into the manager.
        /// </summary>
        /// <typeparam name="T">Type of the scene</typeparam>
        internal void RegisterTransition<T>(string name)
            where T : Transition, new()
        {
            lock (this)
            {
                if (!this.transitions.ContainsKey(name))
                {
                    var transition = new T();
                    this.transitions.Add(name, transition);
                }
                else
                {
                    throw new InvalidOperationException("Duplicated transition names (" + name + ")");
                }
            }
        }

        /// <summary>
        /// Registers a transition into the manager.
        /// </summary>
        /// <param name="type">Type of the transition.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.Exception">Duplicated transition names</exception>
        internal void RegisterTransition(Type type, string name)
        {
            lock (this)
            {
                if (!this.transitions.ContainsKey(name))
                {
                    var constructor = type.GetConstructor(new Type[0]);
                    if (constructor != null)
                    {
                        var transition = (Transition)constructor.Invoke(new object[] { });
                        this.transitions.Add(name, transition);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Duplicated transition names (" + name + ")");
                }
            }
        }

        #endregion Registering Methods

        #region Transitions

        /// <summary>
        /// Setups the transition.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <param name="time">The time.</param>
        /// <exception cref="System.Exception">
        /// Cannot pop the only scene on the stack.
        /// or
        /// Invalid transition name:  + name
        /// </exception>
        internal void SetupTransition(Scene next, TransitionType type, string name, float time)
        {
            if (name == null)
            {
                switch (type)
                {
                    case TransitionType.Switch:
                        this.nextScene = next;
                        this.TransitionSwitchComplete();
                        break;

                    case TransitionType.Push:
                        this.nextScene = next;
                        this.TransitionPushComplete();
                        break;

                    case TransitionType.Pop:
                        this.TransitionPopComplete();
                        break;

                    default:
                        // do the defalut action
                        break;
                }
            }
            else
            {
                Transition trans = null;
                if (this.transitions.TryGetValue(name, out trans))
                {
                    this.nextScene = next;

                    var scene = this.current.Peek();
                    scene.SetTransitioning();
                    scene.Deactivate();

                    if (type != TransitionType.Pop)
                    {
                        scene = this.nextScene;
                    }
                    else
                    {
                        if (this.current.Count >= 2)
                        {
                            var scenes = this.current.ToArray();
                            scene = scenes[scenes.Length - 2];
                        }
                        else
                        {
                            throw new InvalidOperationException("Cannot pop the only scene on the stack.");
                        }
                    }

                    scene.Enter();
                    scene.SetTransitioning();

                    this.transition = trans;
                    this.transition.Start();

                    this.transitionType = type;

                    this.transitionTween = new ValueTweener(0, 1);
                    if (type == TransitionType.Switch)
                    {
                        this.transitionTween.Forward(time);
                        this.transitionTween.Action(new Action(this.TransitionSwitchComplete));
                        this.transitionTween.Start();
                    }
                    else if (type == TransitionType.Push)
                    {
                        this.transitionTween.Forward(time);
                        this.transitionTween.Action(new Action(this.TransitionPushComplete));
                        this.transitionTween.Start();
                    }
                    else if (type == TransitionType.Pop)
                    {
                        this.transitionTween.Forward(time);
                        this.transitionTween.Action(new Action(this.TransitionPopComplete));
                        this.transitionTween.Start();
                    }
                }
                else
                {
                    throw new InvalidOperationException("Invalid transition name: " + name);
                }
            }
        }

        /// <summary>
        /// Transitions the switch complete.
        /// </summary>
        internal void TransitionSwitchComplete()
        {
            lock (this)
            {
                this.current.Peek();
                this.current.Peek().ClearTransitioning();
                this.current.Peek().Deactivate();
                this.current.Peek().Leave();
                this.current.Pop();

                if (this.nextScene != null)
                {
                    this.nextScene.ClearTransitioning();

                    if (this.transition != null)
                    {
                        this.transition.Complete();
                    }

                    this.current.Push(this.nextScene);
                    this.nextScene.Enter();
                    this.nextScene.Activate();
                    this.nextScene = null;
                }

                if (this.transition != null)
                {
                    this.transition.Complete();
                }

                this.transitionType = TransitionType.None;
                this.transitionTween = null;
                this.transition = null;
            }
        }

        /// <summary>
        /// Transitions the push complete.
        /// </summary>
        internal void TransitionPushComplete()
        {
            lock (this)
            {
                this.current.Peek();
                this.current.Peek().ClearTransitioning();
                this.current.Peek().Deactivate();

                if (this.nextScene != null)
                {
                    this.nextScene.ClearTransitioning();

                    if (this.transition != null)
                    {
                        this.transition.Complete();
                    }

                    this.current.Push(this.nextScene);
                    this.nextScene.Enter();
                    this.nextScene.Activate();
                    this.nextScene = null;
                }
                else
                {
                    if (this.transition != null)
                    {
                        this.transition.Complete();
                    }
                }

                this.transitionType = TransitionType.None;
                this.transitionTween = null;
                this.transition = null;
            }
        }

        /// <summary>
        /// Transitions the pop complete.
        /// </summary>
        internal void TransitionPopComplete()
        {
            lock (this)
            {
                this.current.Peek().Deactivate();
                this.current.Peek().ClearTransitioning();
                this.current.Peek().Leave();
                this.current.Pop();

                this.current.Peek();
                this.current.Peek().ClearTransitioning();

                if (this.transition != null)
                {
                    this.transition.Complete();
                }

                this.current.Peek().Activate();

                this.transitionType = TransitionType.None;
                this.transitionTween = null;
                this.transition = null;
            }
        }

        #endregion Transitions

        /// <summary>
        /// Gets a scene instance by its type.
        /// </summary>
        /// <typeparam name="T">Type of the scene.</typeparam>
        /// <returns>Screen instance</returns>
        public T Get<T>()
            where T : Scene
        {
            lock (this)
            {
                Scene screen = null;
                if (this.screens.TryGetValue(typeof(T), out screen))
                {
                    return screen as T;
                }
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Screen type not registered: {0}", typeof(T).FullName));
            }
        }

        /// <summary>
        /// Switches the current scene to a new one.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <exception cref="System.Exception">Screen type not registered:  + type.FullName</exception>
        internal void Set(Type type)
        {
            if (this.screens.ContainsKey(type))
            {
                this.current.Clear();
                this.current.Push(this.screens[type]);
            }
            else
            {
                throw new InvalidOperationException("Screen type not registered: " + type.FullName);
            }
        }

        /// <summary>
        /// Switches the current scene to a new one.
        /// </summary>
        /// <typeparam name="T">Type of the scene.</typeparam>
        /// <param name="transition">The transition name.</param>
        /// <param name="time">The time to occur the transition.</param>
        public void Switch<T>(string transition = null, float time = 1.0f)
            where T : Scene
        {
            this.SetupTransition(this.Get<T>(), TransitionType.Switch, transition, time);
        }

        /// <summary>
        /// Pushes a scene on top of another.
        /// </summary>
        /// <typeparam name="T">Type of the scene.</typeparam>
        /// <param name="transition">The transition name.</param>
        /// <param name="time">The time to occur the transition.</param>
        /// <exception cref="System.InvalidOperationException">Cannot push a scene that is already on the stack.</exception>
        public void Push<T>(string transition = null, float time = 1.0f)
            where T : Scene
        {
            bool exists = (from scene in this.current.ToArray()
                           where scene.GetType() == typeof(T)
                           select scene).Any();
            if (exists)
            {
                throw new InvalidOperationException("Cannot push a scene that is already on the stack.");
            }
            this.SetupTransition(this.Get<T>(), TransitionType.Push, transition, time);
        }

        /// <summary>
        /// Pops the top scene from the scene manager.
        /// </summary>
        /// <param name="transition">The transition name.</param>
        /// <param name="time">The time to occur the transition.</param>
        public void Pop(string transition = null, float time = 1.0f)
        {
            if (this.current.Count == 0)
            {
                throw new InvalidOperationException("Cannot pop scenes - the stack is empty.");
            }
            this.SetupTransition(null, TransitionType.Pop, transition, time);
        }

        /// <summary>
        /// Initializes the scene manager.
        /// </summary>
        internal void Initialize()
        {
            lock (this)
            {
                if (this.screens.Count == 0)
                {
                    throw new InvalidOperationException("Screen list can't be empty.");
                }

                foreach (var transition in this.transitions)
                {
                    transition.Value.Initialize();
                }

                foreach (var screen in this.screens)
                {
                    screen.Value.Initialize();
                }

                this.renderTarget = new RenderTarget2D(AlmiranteEngine.Device, AlmiranteEngine.Settings.Resolution.BaseWidth, AlmiranteEngine.Settings.Resolution.BaseHeight);
                this.renderTargetNext = new RenderTarget2D(AlmiranteEngine.Device, AlmiranteEngine.Settings.Resolution.BaseWidth, AlmiranteEngine.Settings.Resolution.BaseHeight);
            }
        }

        /// <summary>
        /// Initializes the scene manager.
        /// </summary>
        internal void Start()
        {
            lock (this)
            {
                var s = this.current.Peek();
                if (s != null)
                {
                    s.Enter();
                    s.Activate();
                }
            }
        }

        /// <summary>
        /// Initializes the scene manager.
        /// </summary>
        internal void Uninitialize()
        {
            lock (this)
            {
                var scenes = this.current.ToArray();
                for (int i = scenes.Length - 1; i >= 0; i--)
                {
                    scenes[i].Leave();
                }

                foreach (var screen in this.screens)
                {
                    screen.Value.Uninitialize();
                }

                foreach (var transition in this.transitions)
                {
                    transition.Value.Uninitialize();
                }
            }
        }

        /// <summary>
        /// Updates the active scenes.
        /// </summary>
        internal void Update()
        {
            lock (this)
            {
                var screens = this.current.ToArray();
                for (int i = screens.Length - 1; i >= 0; i--)
                {
                    screens[i].Update();
                }

                if (this.transitionTween != null)
                {
                    this.transitionTween.Update(AlmiranteEngine.Time.Frame);
                }

                if (this.transition != null)
                {
                    this.transition.Update(this.transitionTween.Value);
                }

                if (this.nextScene != null)
                {
                    this.nextScene.Update();
                }
            }
        }

        /// <summary>
        /// Draws the active screens.
        /// </summary>
        internal void Draw(SpriteBatch batch)
        {
            lock (this)
            {
                if (this.transition != null)
                {
                    AlmiranteEngine.Application.GraphicsDevice.SetRenderTarget(this.renderTarget);
                    AlmiranteEngine.Application.GraphicsDevice.Clear(Color.Transparent);
                }

                Scene[] scenes = this.current.ToArray();

                int start = scenes.Length - 1;
                for (int i = 0; i < scenes.Length; i++)
                {
                    var scene = scenes[i];
                    if (!scene.Visible)
                    {
                        start = i + 1;
                        break;
                    }
                }

                var downTo = 0;
                var tempScene = this.nextScene;
                if (this.transitionType == TransitionType.Pop)
                {
                    tempScene = scenes[0];
                    downTo = 1;
                }

                for (int i = start; i >= downTo; i--)
                {
                    scenes[i].Draw(batch);
                }

                if (this.transition != null)
                {
                    if (tempScene != null)
                    {
                        AlmiranteEngine.Application.GraphicsDevice.SetRenderTarget(this.renderTargetNext);
                        AlmiranteEngine.Application.GraphicsDevice.Clear(Color.Transparent);
                        tempScene.Draw(batch);

                        AlmiranteEngine.Application.GraphicsDevice.SetRenderTarget(null);
                        AlmiranteEngine.Application.GraphicsDevice.Clear(Color.Black);
                        this.transition.Draw(batch, this.renderTarget, this.renderTargetNext);
                    }
                }
            }
        }

        #region IDisposable Implementation

        /// <summary>
        /// The disposed
        /// </summary>
        protected bool disposed/* = false*/;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                // Do nothing if the object has already been disposed of.
                if (disposed)
                    return;

                if (disposing)
                {
                    // Release disposable objects used by this instance here.

                    if (renderTarget != null)
                        renderTarget.Dispose();
                    if (renderTargetNext != null)
                        renderTargetNext.Dispose();
                }

                // Release unmanaged resources here. Don't access reference type fields.

                // Remember that the object has been disposed of.
                disposed = true;
            }
        }

        public virtual void Dispose()
        {
            Dispose(true);
            // Unregister object for finalization.
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Implementation
    }
}