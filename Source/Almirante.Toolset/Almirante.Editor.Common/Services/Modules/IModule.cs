﻿/// 
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

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Almirante.Editor.Common.Services.Modules
{
    /// <summary>
    /// Plugin.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Gets the plugin id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        string Id
        {
            get;
        }

        /// <summary>
        /// Gets the plugin name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets the plugin description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        string Description
        {
            get;
        }

        /// <summary>
        /// Gets the plugin author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        string Author
        {
            get;
        }

        /// <summary>
        /// Gets the menus.
        /// </summary>
        /// <value>
        /// The menus.
        /// </value>
        IEnumerable<IModuleMenu> Menu
        {
            get;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Uninitializes this instance.
        /// </summary>
        void Uninitialize();
    }
}