using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Almirante.Engine.Core;

namespace Tests.Windows
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            AlmiranteEngine.StartWindows();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}