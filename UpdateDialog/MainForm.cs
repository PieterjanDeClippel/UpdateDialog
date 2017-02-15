using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpdateDialog
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void btnCheckUpdate_Click(object sender, EventArgs e)
		{
			frmUpdate upd = new frmUpdate();
			upd.ShowDialog();
		}
	}
}
