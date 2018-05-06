/* Order of items within a class, struct or interface: (SA1201 and SA1203)

Constant Fields
Fields
Constructors
Finalizers (Destructors)
Delegates
Events
Enums
Interfaces
Properties
Indexers
Methods
Structs
Classes

Within each of these groups order by access: (SA1202)
public
internal
protected internal
protected
private

Within each of the access groups, order by static, then non-static: (SA1204)
static
non-static

Within each of the static/non-static groups of fields, order by readonly, then non-readonly : (SA1214 and SA1215)
readonly
non-readonly
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CryptoWatcher.Launcher;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Globalization;

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
			
			Application.SetCompatibleTextRenderingDefault(false);
			LauncherForm launcherForm = new LauncherForm();
			Application.Run(launcherForm);
			if (launcherForm.ShowMainForm)
				Application.Run(new MainForm());
		}
	}
}
