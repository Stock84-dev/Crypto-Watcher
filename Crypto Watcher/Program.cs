using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CryptoWatcher.Launcher;


namespace CryptoWatcher
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
			LauncherForm launcherForm = new LauncherForm();
			launcherForm.Visible = false;
			Application.Run(launcherForm);
		}
    }
}
