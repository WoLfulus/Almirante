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

namespace Almirante.Engine.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Almirante.Engine.Core;
    using Almirante.Engine.Input.Devices;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Interface component class.
    /// </summary>
    public class Control
    {
        /// <summary>
        /// Stores whether the mouse is over this control.
        /// </summary>
        private bool isMouseOver/* = false*/;

        /// <summary>
        /// Stores the last mouse up time.
        /// </summary>
        private double lastMouseUp/* = 0.0f*/;

        /// <summary>
        /// Stores the last mouse up time.
        /// </summary>
        private MouseButton lastMouseKeyUp = 0.0f;

        /// <summary>
        /// Stores the last mouse up position.
        /// </summary>
        private Vector2 lastMouseUpPosition = Vector2.Zero;

        /// <summary>
        /// Stores the last position the mouse passed in this control.
        /// </summary>
        private Vector2 lastMousePosition = Vector2.Zero;

        /// <summary>
        /// Stores mouse buttons states.
        /// </summary>
        private Dictionary<MouseButton, bool> isMouseDown = new Dictionary<MouseButton, bool>();

        /// <summary>
        /// Stores the control position.
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// Stores the control size.
        /// </summary>
        private Vector2 size;

        /// <summary>
        /// Stores the active state.
        /// </summary>
        private bool active;

        /// <summary>
        /// Gets or sets the active state.
        /// </summary>
        public bool Active
        {
            get
            {
                return this.active;
            }
            internal set
            {
                this.active = value;
            }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector2 Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector2 RealPosition
        {
            get
            {
                var loc = this.Position;
                var parent = this.Parent;
                while (parent != null)
                {
                    loc += parent.Position;
                    parent = parent.Parent;
                }
                return loc;
            }
        }

        /// <summary>
        /// Gets the control's area.
        /// </summary>
        public Rectangle Area
        {
            get
            {
                return new Rectangle((int)this.position.X, (int)this.position.Y, (int)this.size.X, (int)this.size.Y);
            }
        }

        /// <summary>
        /// Gets the real control's area.
        /// </summary>
        public Rectangle RealArea
        {
            get
            {
                var loc = this.RealPosition;
                return new Rectangle((int)loc.X, (int)loc.Y, (int)this.size.X, (int)this.size.Y);
            }
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public Vector2 Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }

        /// <summary>
        /// Gets the parent control.
        /// </summary>
        public Control Parent
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Control"/> is focusable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if focusable; otherwise, <c>false</c>.
        /// </value>
        public bool CanFocus
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Control"/> is focusable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if focusable; otherwise, <c>false</c>.
        /// </value>
        public bool HasFocus
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Control"/> is visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if visible; otherwise, <c>false</c>.
        /// </value>
        public bool Visible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Control"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the controls.
        /// </summary>
        public ControlCollection Controls
        {
            get;
            private set;
        }

        /// <summary>
        /// Occurs when a keyboard key is pressed.
        /// </summary>
        public event EventHandler<KeyboardEventArgs> KeyDown;

        /// <summary>
        /// Occurs when keyboard key is released.
        /// </summary>
        public event EventHandler<KeyboardEventArgs> KeyUp;

        /// <summary>
        /// Occurs when mouse enter the control region.
        /// </summary>
        public event EventHandler MouseEnter;

        /// <summary>
        /// Occurs when mouse enter the control region.
        /// </summary>
        public event EventHandler MouseLeave;

        /// <summary>
        /// Occurs when mouse button is pressed into the control region.
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseDown;

        /// <summary>
        /// Occurs when mouse button is released.
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseUp;

        /// <summary>
        /// Occurs when mouse button is released.
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseScroll;

        /// <summary>
        /// Occurs when mouse button is released.
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseClick;

        /// <summary>
        /// Occurs when mouse button is released.
        /// </summary>
        public event EventHandler<MouseEventArgs> MouseDoubleClick;

        /// <summary>
        /// Occurs when control gets focused.
        /// </summary>
        public event EventHandler Enter;

        /// <summary>
        /// Occurs when control loses its focus.
        /// </summary>
        public event EventHandler Leave;

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        public Control()
        {
            this.Active = false;
            this.Enabled = true;
            this.CanFocus = true;
            this.Visible = true;

            this.Controls = new ControlCollection(this);

            this.KeyUp += OnKeyUp;
            this.KeyDown += OnKeyDown;
            this.MouseEnter += OnMouseEnter;
            this.MouseLeave += OnMouseLeave;
            this.MouseDown += OnMouseDown;
            this.MouseUp += OnMouseUp;
            this.MouseScroll += OnMouseScroll;
            this.Enter += OnEnter;
            this.Leave += OnLeave;
            this.MouseClick += OnMouseClick;
            this.MouseDoubleClick += OnMouseDoubleClick;

            this.isMouseDown.Add(MouseButton.Left, false);
            this.isMouseDown.Add(MouseButton.Middle, false);
            this.isMouseDown.Add(MouseButton.Right, false);
            this.isMouseDown.Add(MouseButton.XButton1, false);
            this.isMouseDown.Add(MouseButton.XButton2, false);
        }

        /// <summary>
        /// Moves the control to the specified position.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        public void Move(float x, float y)
        {
            this.position.X = x;
            this.position.Y = y;
        }

        /// <summary>
        /// Resizes the control with the specified size.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void Resize(float width, float height)
        {
            this.size.X = width;
            this.size.Y = height;
        }

        /// <summary>
        /// Sets the focus on the current control.
        /// </summary>
        /// <param name="value">if set to <c>true</c>, the control got focus.</param>
        internal void SetFocus(bool value)
        {
            if (value)
            {
                var old = this.HasFocus;
                this.HasFocus = true;
                if (!old)
                {
                    this.Active = true;

                    var parent = this.Parent;
                    while (parent != null)
                    {
                        parent.Active = true;
                        parent = parent.Parent;
                    }

                    this.Enter(this, new EventArgs());
                }
            }
            else
            {
                var old = this.HasFocus;
                this.HasFocus = false;
                if (old)
                {
                    this.Active = false;

                    var parent = this.Parent;
                    while (parent != null)
                    {
                        parent.Active = false;
                        parent = parent.Parent;
                    }

                    this.Leave(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets the control at the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The control found that the specified position.</returns>
        public Control GetControlAt(Vector2 position)
        {
            if (!this.Visible) // !this.CanFocus || !this.Enabled ||
            {
                return null;
            }

            // Check control rectangle.
            if (!this.IsPointInside(position))
            {
                return null;
            }

            // Mouse position relative to this control.
            var relative = position - this.Position;
            foreach (var control in this.Controls)
            {
                var result = control.GetControlAt(relative);
                if (result != null)
                {
                    return result;
                }
            }

            return this;
        }

        /// <summary>
        /// Called when [key down].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyboardEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual void OnKeyDown(object sender, KeyboardEventArgs e)
        {
        }

        /// <summary>
        /// Called when [key up].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyboardEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual void OnKeyUp(object sender, KeyboardEventArgs e)
        {
        }

        /// <summary>
        /// Called when click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Almirante.Engine.Interface.MouseEventArgs" /> instance containing the event data.</param>
        protected virtual void OnMouseClick(object sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when double click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Almirante.Engine.Interface.MouseEventArgs" /> instance containing the event data.</param>
        protected virtual void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when a mouse button is released.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Almirante.Engine.Interface.MouseEventArgs" /> instance containing the event data.</param>
        protected virtual void OnMouseUp(object sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when a mouse button is pressed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected virtual void OnMouseDown(object sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when mouse leaves the control region.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseLeave(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Called when mouse enters the control region.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseEnter(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Called when mouse wheel is rotated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Almirante.Engine.Interface.MouseEventArgs" /> instance containing the event data.</param>
        protected virtual void OnMouseScroll(object sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// Called when control loses its focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        protected virtual void OnEnter(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Called when control gets focused.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        protected virtual void OnLeave(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Determines whether the specified point is inside the control region.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>
        ///   <c>true</c> if [is point inside] [the specified point]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsPointInside(Vector2 point)
        {
            if (point.X >= this.Position.X && point.X <= this.Position.X + this.Size.X &&
                point.Y >= this.Position.Y && point.Y <= this.Position.Y + this.Size.Y)
            {
                return true;
            }
            return false;
        }

        #region Input Functions

        /// <summary>
        /// Called when a keyboard button is pressed.
        /// </summary>
        /// <param name="key">The key.</param>
        internal void OnInputKeyboardPress(KeyboardKey key)
        {
            if (this.HasFocus)
            {
                this.KeyDown(this, new KeyboardEventArgs(key));
            }
            else
            {
                foreach (var control in this.Controls)
                {
                    if (control.Enabled)
                    {
                        control.OnInputKeyboardPress(key);
                    }
                }
            }
        }

        /// <summary>
        /// Called when a keyboard button is released.
        /// </summary>
        /// <param name="key">The key.</param>
        internal void OnInputKeyboardRelease(KeyboardKey key)
        {
            if (this.HasFocus)
            {
                this.KeyUp(this, new KeyboardEventArgs(key));
            }
            else
            {
                foreach (var control in this.Controls)
                {
                    if (control.Enabled)
                    {
                        control.OnInputKeyboardRelease(key);
                    }
                }
            }
        }

        /// <summary>
        /// Called when a mouse button is pressed.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="key">The key.</param>
        internal void OnInputMousePress(Vector2 position, MouseKey key)
        {
            if (this.isMouseOver)
            {
                var relative = position - this.position;
                foreach (var control in this.Controls)
                {
                    if (control.Visible)
                    {
                        if (control.IsPointInside(relative))
                        {
                            if (control.Enabled)
                            {
                                control.OnInputMousePress(relative, key);
                            }
                            return;
                        }
                    }
                }

                this.isMouseDown[key.Key] = true;
                this.MouseDown(this, new MouseEventArgs(AlmiranteEngine.Time.Total, key));
            }
        }

        /// <summary>
        /// Called when a mouse button is released.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="key">The key.</param>
        internal void OnInputMouseRelease(Vector2 position, MouseKey key)
        {
            if (this.isMouseDown.ContainsKey(key.Key))
            {
                if (this.isMouseDown[key.Key])
                {
                    this.MouseClick(this, new MouseEventArgs(AlmiranteEngine.Time.Total, key));

                    if (AlmiranteEngine.Time.Total - this.lastMouseUp <= (SystemInformation.DoubleClickTime / 1000.0f) &&
                        Math.Abs(this.lastMouseUpPosition.X - position.X) <= SystemInformation.DoubleClickSize.Width &&
                        Math.Abs(this.lastMouseUpPosition.Y - position.Y) <= SystemInformation.DoubleClickSize.Height &&
                        this.lastMouseKeyUp == key.Key)
                    {
                        this.lastMouseUp = 0.0;
                        this.MouseDoubleClick(this, new MouseEventArgs(AlmiranteEngine.Time.Total, key));
                    }
                    else
                    {
                        this.lastMouseKeyUp = key.Key;
                        this.lastMouseUp = AlmiranteEngine.Time.Total;
                        this.lastMouseUpPosition = position;
                    }

                    this.isMouseDown[key.Key] = false;
                    this.MouseUp(this, new MouseEventArgs(AlmiranteEngine.Time.Total, key));

                    if (!this.IsPointInside(position))
                    {
                        this.isMouseOver = false;
                        this.MouseLeave(this, new EventArgs());
                    }
                }
            }

            var relative = position - this.position;
            foreach (var control in this.Controls)
            {
                control.OnInputMouseRelease(relative, key);
            }
        }

        /// <summary>
        /// Called when a mouse wheel scrolls.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="ammount">The ammount.</param>
        internal void OnInputMouseScroll(Vector2 position, int ammount)
        {
            this.MouseScroll(this, new MouseEventArgs(AlmiranteEngine.Time.Total, null, ammount));
        }

        /// <summary>
        /// Determines whether is any mouse button down.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if there's any mouse button down; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsMouseDown()
        {
            foreach (var v in isMouseDown)
            {
                if (v.Value)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Called when mouse moves.
        /// </summary>
        /// <param name="position">The position.</param>
        internal void OnInputMouseMove(Vector2 position)
        {
            if (this.IsPointInside(position))
            {
                if (!this.IsMouseDown())
                {
                    bool overInnerControl = false;

                    var relative = position - this.position;
                    foreach (var control in this.Controls)
                    {
                        if (control.Visible)
                        {
                            if (control.IsPointInside(relative))
                            {
                                overInnerControl = true;
                            }
                            control.OnInputMouseMove(relative);
                        }
                    }

                    if (!overInnerControl)
                    {
                        if (!this.isMouseOver)
                        {
                            this.isMouseOver = true;
                            this.MouseEnter(this, new EventArgs());
                        }

                        this.lastMousePosition = position;
                    }
                    else
                    {
                        if (this.isMouseOver)
                        {
                            this.isMouseOver = false;
                            this.MouseLeave(this, new EventArgs());
                        }
                    }
                }
            }
            else
            {
                if (this.isMouseOver)
                {
                    if (!this.IsMouseDown())
                    {
                        this.isMouseOver = false;
                        this.MouseLeave(this, new EventArgs());
                    }
                }

                var relative = position - this.position;
                foreach (var control in this.Controls)
                {
                    if (control.isMouseOver)
                    {
                        control.OnInputMouseMove(relative);
                    }
                }
            }
        }

        #endregion Input Functions

        #region Almirante Functions

        /// <summary>
        /// Updates this instance.
        /// </summary>
        internal virtual void Update()
        {
            this.OnUpdate();

            foreach (var control in this.Controls)
            {
                control.Update();
            }
        }

        /// <summary>
        /// Draws this HUD instance.
        /// </summary>
        /// <param name="batch">The sprite batch instance.</param>
        /// <param name="position">The position.</param>
        internal virtual void Draw(SpriteBatch batch, Vector2 position)
        {
            this.OnDraw(batch, position);

            foreach (var control in this.Controls)
            {
                control.Draw(batch, position + control.position);
            }
        }

        #endregion Almirante Functions

        #region Almirante Events

        /// <summary>
        /// Called when updating the HUD component.
        /// </summary>
        protected virtual void OnUpdate()
        {
        }

        /// <summary>
        /// Called when drawing the HUD component.
        /// </summary>
        /// <param name="batch">The sprite batch instance.</param>
        /// <param name="position">The position.</param>
        protected virtual void OnDraw(SpriteBatch batch, Vector2 position)
        {
        }

        #endregion Almirante Events
    }
}