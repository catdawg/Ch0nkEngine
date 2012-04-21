using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Ch0nkEngine
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm mainForm = new MainForm();

            mainForm.Show();

            while (mainForm.Created)
            {
                Application.DoEvents();
                mainForm.Render();
            }
        }
    }
}
