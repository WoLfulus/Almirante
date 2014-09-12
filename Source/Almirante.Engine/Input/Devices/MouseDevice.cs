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
    using System;
    using System.Collections.Generic;
    using Almirante.Engine.Core;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Mouse input device.
    /// </summary>
    public class MouseDevice : IInputDevice
    {
        /// <summary>
        /// Stores all keyboard device keys.
        /// </summary>
        private readonly Dictionary<MouseButton, MouseKey> keys;

        /// <summary>
        /// Stores the current keyboard state.
        /// </summary>
        private MouseState state;

        /// <summary>
        /// Stores the current keyboard state.
        /// </summary>
        private MouseState lastState;

        /// <summary>
        /// Stores the current mouse position.
        /// </summary>
        private Vector2 currentPosition;

        /// <summary>
        /// Creates a new instance of the <see cref="KeyboardDevice"/> class.
        /// </summary>
        internal MouseDevice()
        {
            this.lastState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            this.state = this.lastState;

            this.keys = new Dictionary<MouseButton, MouseKey>();
            foreach (MouseButton k in Enum.GetValues(typeof(MouseButton)))
            {
                this.keys.Add(k, new MouseKey(k));
            }
        }

        /// <summary>
        /// Mouse key event delegate.
        /// </summary>
        /// <param name="key">Key event</param>
        public delegate void MouseKeyEvent(MouseKey key);

        /// <summary>
        /// Mouse move event delegate.
        /// </summary>
        /// <param name="position">Current mouse position.</param>
        /// <param name="worldPosition">Current mouse position relative to the world (camera).</param>
        public delegate void MouseMovedEvent(Vector2 position, Vector2 worldPosition);

        /// <summary>
        /// Mouse scroll event delegate.
        /// </summary>
        /// <param name="ammount">Scroll ammount.</param>
        public delegate void MouseScrollEvent(int ammount);

        /// <summary>
        /// Key press event.
        /// </summary>
        public event MouseKeyEvent Pressed;

        /// <summary>
        /// Key press event.
        /// </summary>
        public event MouseKeyEvent Released;

        /// <summary>
        /// Mouse moved event.
        /// </summary>
        public event MouseMovedEvent Moved;

        /// <summary>
        /// Mouse moved event.
        /// </summary>
        public event MouseScrollEvent Scroll;

        /// <summary>
        /// Gets or sets the mouse position.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return this.currentPosition;
            }
            set
            {
                this.currentPosition = value;
                this.Dislocation = Vector2.Zero;
                Mouse.SetPosition(Convert.ToInt32(value.X), Convert.ToInt32(value.Y));
            }
        }

        /// <summary>
        /// Gets or sets the mouse position.
        /// </summary>
        public Vector2 WorldPosition
        {
            get
            {
                var viewMatrix = Matrix.Invert(AlmiranteEngine.Camera.Matrix);
                var worldPosition = Vector2.Transform(this.currentPosition, viewMatrix);
                return worldPosition;
            }
        }

        /// <summary>
        ///   Gets or sets the actual dislocation speed of the mouse cursor within this window
        /// </summary>
        public Vector2 Dislocation
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the accumulated mouse scroll.
        /// </summary>
        public int ScrollTotal
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the accumulated mouse scroll.
        /// </summary>
        public int ScrollValue
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the key state for the specified mouse key.
        /// </summary>
        /// <param name="key">Requested key.</param>
        /// <returns>State of the requested key.</returns>
        public MouseKey this[MouseButton key]
        {
            get
            {
                return this.keys[key];
            }
            internal set
            {
                this.keys[key] = value;
            }
        }

        /// <summary>
        /// Update the keyboard keys state
        /// </summary>
        public void Update()
        {
            this.lastState = this.state;

            this.state = Mouse.GetState();

            var mousePosition = new Vector2(this.state.X, this.state.Y);

            var resolution = AlmiranteEngine.Settings.Resolution;
            var position = Vector2.Transform(mousePosition - resolution.ViewportSize, Matrix.Invert(resolution.Matrix));

            this.state = new MouseState(
                (int)position.X,
                (int)position.Y,
                state.ScrollWheelValue,
                state.LeftButton,
                state.MiddleButton,
                state.RightButton,
                state.XButton1,
                state.XButton2
            );

            if (this.lastState.X != this.state.X || this.lastState.Y != this.state.Y)
            {
                this.currentPosition = new Vector2(this.state.X, this.state.Y);

                Vector2 diff = this.Dislocation;
                diff.X = this.state.X - this.lastState.X;
                diff.Y = this.state.Y - this.lastState.Y;
                this.Dislocation = diff;

                if (this.Moved != null)
                {
                    this.Moved(this.Position, this.WorldPosition);
                }
            }

            this.ScrollTotal = this.state.ScrollWheelValue;
            this.ScrollValue = this.state.ScrollWheelValue - this.lastState.ScrollWheelValue;
            if (this.ScrollValue != 0)
            {
                if (this.Scroll != null)
                {
                    this.Scroll(this.ScrollValue);
                }
            }

            foreach (var key in this.keys.Keys)
            {
                switch (key)
                {
                    case MouseButton.Left:
                        this[key].Update(this.state.LeftButton == ButtonState.Pressed);
                        break;

                    case MouseButton.Middle:
                        this[key].Update(this.state.MiddleButton == ButtonState.Pressed);
                        break;

                    case MouseButton.Right:
                        this[key].Update(this.state.RightButton == ButtonState.Pressed);
                        break;

                    case MouseButton.XButton1:
                        this[key].Update(this.state.XButton1 == ButtonState.Pressed);
                        break;

                    case MouseButton.XButton2:
                        this[key].Update(this.state.XButton2 == ButtonState.Pressed);
                        break;
                    default:
                        // do the defalut action
                        break;
                }
                if (this[key].Pressed)
                {
                    if (this.Pressed != null)
                    {
                        this.Pressed(this[key]);
                    }
                }
                else if (this[key].Released)
                {
                    if (this.Released != null)
                    {
                        this.Released(this[key]);
                    }
                }
            }
        }
    }
}