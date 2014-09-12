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

namespace Almirante.Editor.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;
    using Almirante.Editor.Common;
    using Almirante.Editor.Common.Services.Modules;
    using Almirante.Editor.Forms;

    /// <summary>
    /// Plugin repository class.
    /// </summary>
    public class PluginService : IModuleManager
    {
        /// <summary>
        /// Gets or sets the plugins.
        /// </summary>
        /// <value>
        /// The plugins.
        /// </value>
        [ImportMany(typeof(IModule))]
        public IEnumerable<IModule> All
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginService"/> class.
        /// </summary>
        public PluginService()
        {
        }

        /// <summary>
        /// Initializes the plugins.
        /// </summary>
        /// <param name="editor">The editor.</param>
        public void Initialize()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            catalog.Catalogs.Add(new DirectoryCatalog(Path.GetDirectoryName(assembly.Location)));

            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);

            foreach (var plugin in this.All)
            {
                plugin.Initialize();

                var pluginMenu = EditorService.Window.menuPlugins;
                foreach (var menu in plugin.Menu)
                {
                    var root = new ToolStripMenuItem()
                    {
                        Text = menu.Text
                    };

                    root.Click += (s, e) => menu.Execute(null);

                    if (menu.Children != null)
                    {
                        foreach (var child in menu.Children)
                        {
                            var item = new ToolStripMenuItem()
                            {
                                Text = child
                            };
                            item.Click += (s, e) => menu.Execute(child);
                            root.DropDownItems.Add(item);
                        }
                    }

                    pluginMenu.DropDownItems.Add(root);
                }
            }
        }
    }
}