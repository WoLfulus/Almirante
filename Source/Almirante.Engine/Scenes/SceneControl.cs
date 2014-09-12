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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using Almirante.Engine.Core.Windows;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Control scenes
    /// </summary>
    public abstract class SceneControl : Scene
    {
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public Control Parent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color of the clear.
        /// </summary>
        /// <value>
        /// The color of the clear.
        /// </value>
        public Color BackgroundColor
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneControl"/> class.
        /// </summary>
        protected SceneControl()
        {
            this.BackgroundColor = Color.Black;
        }

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        internal override void Activate()
        {
            this.Parent.KeyDown += OnKeyDown;
            this.Parent.KeyPress += OnKeyPress;
            this.Parent.KeyUp += OnKeyUp;
            this.Parent.PreviewKeyDown += OnPreviewKeyDown;
            this.Parent.MouseClick += OnMouseClick;
            this.Parent.MouseDoubleClick += OnMouseDoubleClick;
            this.Parent.MouseDown += OnMouseDown;
            this.Parent.MouseEnter += OnMouseEnter;
            this.Parent.MouseHover += OnMouseHover;
            this.Parent.MouseLeave += OnMouseLeave;
            this.Parent.MouseMove += OnMouseMove;
            this.Parent.MouseUp += OnMouseUp;
            this.Parent.MouseWheel += OnMouseWheel;
            this.Parent.MouseCaptureChanged += OnMouseCaptureChanged;
            base.Activate();
        }

        /// <summary>
        /// Initializes the scene.
        /// </summary>
        internal override void Deactivate()
        {
            this.Parent.KeyDown -= OnKeyDown;
            this.Parent.KeyPress -= OnKeyPress;
            this.Parent.KeyUp -= OnKeyUp;
            this.Parent.PreviewKeyDown -= OnPreviewKeyDown;
            this.Parent.MouseClick -= OnMouseClick;
            this.Parent.MouseDoubleClick -= OnMouseDoubleClick;
            this.Parent.MouseDown -= OnMouseDown;
            this.Parent.MouseEnter -= OnMouseEnter;
            this.Parent.MouseHover -= OnMouseHover;
            this.Parent.MouseLeave -= OnMouseLeave;
            this.Parent.MouseMove -= OnMouseMove;
            this.Parent.MouseUp -= OnMouseUp;
            this.Parent.MouseWheel -= OnMouseWheel;
            this.Parent.MouseCaptureChanged -= OnMouseCaptureChanged;
            base.Deactivate();
        }

        /// <summary>
        /// Called when [preview key down].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PreviewKeyDownEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual void OnPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [key up].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnKeyUp(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [key press].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyPressEventArgs"/> instance containing the event data.</param>
        protected virtual void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [key down].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnKeyDown(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [mouse capture changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual void OnMouseCaptureChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [mouse wheel].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual void OnMouseWheel(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [mouse up].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual void OnMouseUp(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [mouse move].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [mouse leave].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual void OnMouseLeave(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [mouse hover].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual void OnMouseHover(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [mouse enter].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual void OnMouseEnter(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [mouse down].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual void OnMouseDown(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [mouse double click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Called when [mouse click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual void OnMouseClick(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}