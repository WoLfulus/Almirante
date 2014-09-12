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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Almirante.Editor.Controls;
using Almirante.Editor.Plugins;
using Almirante.Editor.Services;

namespace Almirante.Editor.Forms
{
    /// <summary>
    ///
    /// </summary>
    public partial class MainWindow : Form
    {
        /// <summary>
        /// The timer
        /// </summary>
        private Timer timer = new Timer();

        /// <summary>
        /// The splash
        /// </summary>
        private SplashWindow splash;

        /// <summary>
        /// The panel
        /// </summary>
        private GradientPanel panel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.panel = new GradientPanel();
            this.Controls.Add(panel);
        }

        /// <summary>
        /// Handles the Load event of the MainWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MainWindow_Load(object sender, EventArgs e)
        {
            EditorService.Window = this;
            EditorService.Plugins.Initialize();
            EditorService.Project.Initialize();

            this.timer.Interval = 3000;
            this.timer.Tick += (s, ev) =>
            {
                this.splash.Close();
                this.timer.Stop();
            };

            this.timer.Start();

            this.splash = new SplashWindow();
            this.splash.ShowDialog();
        }

        /// <summary>
        /// Handles the Shown event of the MainWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MainWindow_Shown(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the Click event of the exitToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}