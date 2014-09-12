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
    using Almirante.Engine.Core;
    using Almirante.Engine.Input;
    using Almirante.Engine.Input.Devices;
    using Almirante.Engine.Interface;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Screen class.
    /// </summary>
    public abstract class Scene
    {
        /// <summary>
        /// Stores the current scene state.
        /// </summary>
        private SceneState state = SceneState.Default;

        /// <summary>
        /// Stores the next state of the top property.
        /// </summary>
        private bool nextTop/* = false*/;

        /// <summary>
        /// Stores the current state of the top property.
        /// </summary>
        private bool currentTop/* = false*/;

        /// <summary>
        /// Stores the focused control.
        /// </summary>
        private Control focus;

        /// <summary>
        /// Stores a reference to the input manager.
        /// </summary>
        private InputManager input;

        /// <summary>
        /// Stores the container (root) control.
        /// </summary>
        protected Control Interface
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the current scene in on to.
        /// </summary>
        public bool Active
        {
            get
            {
                return this.currentTop;
            }
            internal set
            {
#warning Double check this
                this.currentTop = value;
                this.nextTop = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the scene is visible or not.
        /// </summary>
        public bool Visible
        {
            get
            {
                return !this.state.HasFlag(SceneState.Hidden);
            }
            set
            {
                if (value)
                {
                    if (!this.Visible)
                    {
                        this.Show();
                    }
                }
                else
                {
                    if (this.Visible)
                    {
                        this.Hide();
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the scene is paused or not.
        /// </summary>
        public bool Paused
        {
            get
            {
                return this.state.HasFlag(SceneState.Paused);
            }
            set
            {
                if (value)
                {
                    if (!this.Paused)
                    {
                        this.Pause();
                    }
                }
                else
                {
                    if (this.Paused)
                    {
                        this.Resume();
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the scene is paused or not.
        /// </summary>
        public bool Transitioning
        {
            get
            {
                return this.state.HasFlag(SceneState.Transitioning);
            }
            internal set
            {
                if (value)
                {
                    if (!this.Transitioning)
                    {
                        this.SetTransitioning();
                    }
                }
                else
                {
                    if (this.Transitioning)
                    {
                        this.ClearTransitioning();
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        internal virtual void Initialize()
        {
            this.state = SceneState.Default;

            this.input = AlmiranteEngine.Input;

            var settings = AlmiranteEngine.Settings;
            this.Interface = new RootControl()
            {
                Active = true,
                Position = Vector2.Zero,
                Size = new Vector2(settings.Resolution.BaseWidth, settings.Resolution.BaseHeight),
                Parent = null,
            };

            this.focus = Interface;

            this.OnInitialize();
        }

        /// <summary>
        /// Sets the input events.
        /// </summary>
        internal void SetInputEvents()
        {
            this.input.Keyboard.Pressed += OnInputKeyboardPressed;
            this.input.Keyboard.Released += OnInputKeyboardReleased;
            this.input.Mouse.Pressed += OnInputMousePressed;
            this.input.Mouse.Released += OnInputMouseReleased;
            this.input.Mouse.Moved += OnInputMouseMoved;
            this.input.Mouse.Scroll += OnInputMouseScroll;
        }

        /// <summary>
        /// Clears the input events.
        /// </summary>
        internal void ClearInputEvents()
        {
            this.input.Keyboard.Pressed -= OnInputKeyboardPressed;
            this.input.Keyboard.Released -= OnInputKeyboardReleased;
            this.input.Mouse.Pressed -= OnInputMousePressed;
            this.input.Mouse.Released -= OnInputMouseReleased;
            this.input.Mouse.Moved -= OnInputMouseMoved;
            this.input.Mouse.Scroll -= OnInputMouseScroll;
        }

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        internal virtual void Uninitialize()
        {
            this.ClearInputEvents();
            this.OnUninitialize();
        }

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        internal virtual void Activate()
        {
            this.Active = true;
            this.SetInputEvents();
            this.OnActivate();
        }

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        internal virtual void Deactivate()
        {
            this.Active = false;
            this.ClearInputEvents();
            this.OnDeactivate();
        }

        /// <summary>
        /// Pauses the current scene.
        /// </summary>
        internal virtual void Pause()
        {
            this.state = (this.state | SceneState.Paused);
        }

        /// <summary>
        /// Resumes the current scene.
        /// </summary>
        internal virtual void Resume()
        {
            this.state = (this.state & ~SceneState.Paused);
        }

        /// <summary>
        /// Hides the current scene.
        /// </summary>
        internal virtual void Hide()
        {
            this.state = (this.state | SceneState.Hidden);
        }

        /// <summary>
        /// Shows the current scene.
        /// </summary>
        internal virtual void Show()
        {
            this.state = (this.state & ~SceneState.Hidden);
        }

        /// <summary>
        /// Sets the transitioning state.
        /// </summary>
        internal virtual void SetTransitioning()
        {
            this.state = (this.state | SceneState.Transitioning);
        }

        /// <summary>
        /// Removes the transitioning state.
        /// </summary>
        internal virtual void ClearTransitioning()
        {
            this.state = (this.state & ~SceneState.Transitioning);
        }

        #region Interface Methods

        /// <summary>
        /// Focuses the specified control.
        /// </summary>
        /// <param name="control">The control to focus on.</param>
        protected void Focus(Control control)
        {
            if (this.focus == control)
            {
                return;
            }
            else
            {
                if (this.focus != null)
                {
                    this.focus.SetFocus(false);
                }
                control.SetFocus(true);
                this.focus = control;
            }
        }

        /// <summary>
        /// Listens for keyboard button press.
        /// </summary>
        /// <param name="key">The key.</param>
        private void OnInputKeyboardPressed(KeyboardKey key)
        {
            if (!this.Transitioning)
            {
                if (this.focus != null)
                {
                    this.focus.OnInputKeyboardPress(key);
                }
            }
        }

        /// <summary>
        /// Listens for keyboard button release.
        /// </summary>
        /// <param name="key">The key.</param>
        private void OnInputKeyboardReleased(KeyboardKey key)
        {
            if (!this.Transitioning)
            {
                if (this.focus != null)
                {
                    this.focus.OnInputKeyboardRelease(key);
                }
            }
        }

        /// <summary>
        /// Listens for mouse movement.
        /// </summary>
        /// <param name="position">New position.</param>
        /// <param name="worldPosition">New world position.</param>
        private void OnInputMouseMoved(Vector2 position, Vector2 worldPosition)
        {
            if (!this.Transitioning)
            {
                this.Interface.OnInputMouseMove(position);
            }
        }

        /// <summary>
        /// Listens for mouse button press events.
        /// </summary>
        /// <param name="key">Pressed key.</param>
        private void OnInputMousePressed(MouseKey key)
        {
            if (!this.Transitioning)
            {
                var control = this.Interface.GetControlAt(this.input.Mouse.Position);
                if (control != null)
                {
                    if (key.Key == MouseButton.Left)
                    {
                        if (control.CanFocus)
                        {
                            if (this.focus != control)
                            {
                                this.Focus(control);
                            }
                        }
                    }
                    control.OnInputMousePress(this.input.Mouse.Position - control.RealPosition, key);
                }
            }
        }

        /// <summary>
        /// Listens for mouse button release events.
        /// </summary>
        /// <param name="key">Released key.</param>
        private void OnInputMouseReleased(MouseKey key)
        {
            if (!this.Transitioning)
            {
                this.Interface.OnInputMouseRelease(this.input.Mouse.Position, key);
            }
        }

        /// <summary>
        /// Listens for mouse scroll events.
        /// </summary>
        /// <param name="ammount">Ammount scrolled.</param>
        private void OnInputMouseScroll(int ammount)
        {
            if (!this.Transitioning)
            {
                if (this.focus != null)
                {
                    this.focus.OnInputMouseScroll(this.focus.RealPosition - this.input.Mouse.Position, ammount);
                }
            }
        }

        #endregion Interface Methods

        /// <summary>
        /// Updates the current scene.
        /// </summary>
        internal virtual void Update()
        {
            this.currentTop = this.nextTop;
            if (!this.Transitioning)
            {
                this.Interface.Update();
            }
            this.OnUpdate();
        }

        /// <summary>
        /// Draws the current scene.
        /// </summary>
        /// <param name="batch">The batch.</param>
        internal virtual void Draw(SpriteBatch batch)
        {
            this.OnDraw(batch);
            this.Interface.Draw(batch, Vector2.Zero);
        }

        /// <summary>
        /// Enters the current scene.
        /// </summary>
        internal virtual void Enter()
        {
            this.OnEnter();
        }

        /// <summary>
        /// Leaves the current scene.
        /// </summary>
        internal virtual void Leave()
        {
            this.OnLeave();
        }

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        protected abstract void OnInitialize();

        /// <summary>
        /// Uninitializes the scene.
        /// </summary>
        protected abstract void OnUninitialize();

        /// <summary>
        /// Screen logic update function.
        /// </summary>
        protected abstract void OnUpdate();

        /// <summary>
        /// Screen graphic drawing function.
        /// </summary>
        /// <param name="batch">The batch instance.</param>
        protected virtual void OnDraw(SpriteBatch batch)
        {
        }

        /// <summary>
        /// When the scene gets paused.
        /// </summary>
        protected virtual void OnPause()
        {
        }

        /// <summary>
        /// When scene gets resumed.
        /// </summary>
        protected virtual void OnResume()
        {
        }

        /// <summary>
        /// When scene gets hidden.
        /// </summary>
        protected virtual void OnHide()
        {
        }

        /// <summary>
        /// When scene is shown.
        /// </summary>
        protected virtual void OnShow()
        {
        }

        /// <summary>
        /// When entering the scene.
        /// </summary>
        protected virtual void OnEnter()
        {
        }

        /// <summary>
        /// When leaving the scene.
        /// </summary>
        protected virtual void OnLeave()
        {
        }

        /// <summary>
        /// When entering the scene.
        /// </summary>
        protected virtual void OnActivate()
        {
        }

        /// <summary>
        /// When leaving the scene.
        /// </summary>
        protected virtual void OnDeactivate()
        {
        }

        /// <summary>
        /// When game gets resized.
        /// </summary>
        protected virtual void OnResize()
        {
        }
    }
}