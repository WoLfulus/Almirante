using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Almirante.Engine.Core;
using Almirante.Engine.Core.Windows;

namespace Tests.Windows
{
    public partial class MainWindow : Form
    {
        // private AlmiranteEngine

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            this.preview.Controls.Add(new AlmiranteControl<MainScene>());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}