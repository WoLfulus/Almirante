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

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Almirante.Editor.Common;
using Almirante.Editor.Common.Services.Modules;

namespace Almirante.Editor.Plugins.Behavior
{
    /// <summary>
    /// Behavior editor module.
    /// </summary>
    [Export(typeof(IModule))]
    public class BehaviorModule : IModule
    {
        /// <summary>
        /// Gets the plugin id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public string Id
        {
            get
            {
                return "BehaviorEditorModule";
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return "Behavior Editor";
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description
        {
            get
            {
                return "Behavior tree editor.";
            }
        }

        /// <summary>
        /// Gets the author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        public string Author
        {
            get
            {
                return "WoLfulus";
            }
        }

        /// <summary>
        /// Gets the menus.
        /// </summary>
        /// <value>
        /// The menus.
        /// </value>
        public IEnumerable<IModuleMenu> Menu
        {
            get
            {
                return this.menu;
            }
        }

        /// <summary>
        /// The menu
        /// </summary>
        private List<IModuleMenu> menu;

        /// <summary>
        /// Initializes the specified services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void Initialize()
        {
            this.menu = new List<IModuleMenu>();
            this.menu.Add(new BehaviorMenu(this));
        }

        /// <summary>
        /// Uninitializes this instance.
        /// </summary>
        public void Uninitialize()
        {
        }
    }
}