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

namespace Almirante.Engine.Core
{
    /// <summary>
    /// Settings class.
    /// </summary>
    public sealed class Settings
    {
        /// <summary>
        /// Gets the resolution.
        /// </summary>
        public Resolution Resolution
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the window title string.
        /// </summary>
        public string WindowTitle
        {
            get
            {
                return AlmiranteEngine.Application.Window.Title;
            }
            set
            {
                AlmiranteEngine.Application.Window.Title = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether vertical sync is on.
        /// </summary>
        /// <value>
        ///   <c>true</c> if vertical sync is on; otherwise, <c>false</c>.
        /// </value>
        public bool VerticalSync
        {
            get
            {
                return AlmiranteEngine.DeviceManager.SynchronizeWithVerticalRetrace;
            }
            set
            {
                AlmiranteEngine.DeviceManager.SynchronizeWithVerticalRetrace = value;
                AlmiranteEngine.DeviceManager.ApplyChanges();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse is visible.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is mouse visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsCursorVisible
        {
            get
            {
                return AlmiranteEngine.Application.IsMouseVisible;
            }
            set
            {
                AlmiranteEngine.Application.IsMouseVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use fixed timestep.
        /// </summary>
        /// <value>
        ///   <c>true</c> if fixed timestep is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool UseFixedTimestep
        {
            get
            {
                return AlmiranteEngine.Application.IsFixedTimeStep;
            }
            set
            {
                AlmiranteEngine.Application.IsFixedTimeStep = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings" /> class.
        /// </summary>
        internal Settings()
        {
            this.Resolution = new Resolution();
        }
    }
}