using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpdateDialog
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			#region Default
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			#endregion
			#region Set UI Culture
			string cult = CultureInfo.CurrentCulture.Name;
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(cult);
			Thread.CurrentThread.CurrentCulture = new CultureInfo(cult);
			#endregion
			Application.Run(new MainForm());
		}
	}
}
