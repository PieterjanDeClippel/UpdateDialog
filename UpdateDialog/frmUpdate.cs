using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using UpdateDialog.Properties;

namespace UpdateDialog
{
	public partial class frmUpdate : Form
	{
		public frmUpdate()
		{
			InitializeComponent();
			try
			{
				res_frm = new ComponentResourceManager(typeof(frmUpdate));

				depl = ApplicationDeployment.CurrentDeployment;
				depl.CheckForUpdateProgressChanged += depl_CheckForUpdateProgressChanged;
				depl.CheckForUpdateCompleted += CurrentDeployment_CheckForUpdateCompleted;
				depl.UpdateProgressChanged += depl_UpdateProgressChanged;
				depl.UpdateCompleted += depl_UpdateCompleted;
			}
			catch (Exception)
			{
			}
		}

		ApplicationDeployment depl;
		ComponentResourceManager res_frm;

		private void frmUpdate_Load(object sender, EventArgs e)
		{
			if (depl != null)
				lblHuidigeVersie.Text = Resources.CurrentVersion + ": " + Environment.NewLine + depl.CurrentVersion.ToString();
			ZoekAsync();
		}

		private void btnOpnieuw_Click(object sender, EventArgs e)
		{
			ZoekAsync();
		}

		void ZoekAsync()
		{
			btnUpdate.Enabled = false;
			btnOpnieuw.Enabled = false;
			prev_state = "";
			lblStatus.Text = res_frm.GetString("lblStatus.Text");
			if (depl != null) depl.CheckForUpdateAsync();
		}

		string prev_state = "";
		void depl_CheckForUpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
		{
			progressBar1.Value = e.ProgressPercentage;
			lblProgressPercent.Text = e.ProgressPercentage.ToString() + "%";
			lblProgressBytes.Text = e.BytesCompleted.ToString() + " / " + e.BytesTotal.ToString() + " bytes";
			if (e.State.ToString() != prev_state)
			{
				txtInfo.AppendText(Resources.Checking + ": " + e.State.ToString() + "\r\n");
				prev_state = e.State.ToString();
			}
		}

		void CurrentDeployment_CheckForUpdateCompleted(object sender, CheckForUpdateCompletedEventArgs e)
		{
			btnOpnieuw.Enabled = true;
			if (e.UpdateAvailable)
			{
				lblStatus.Text = Resources.UpdateAvailable;
				lblNieuweVersie.Text = res_frm.GetString("lblNieuweVersie.Text") + Environment.NewLine + e.AvailableVersion.ToString();
				btnUpdate.Enabled = true;
			}
			else
			{
				lblStatus.Text = Resources.UpToDate;
			}
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			btnUpdate.Enabled = false;
			btnOpnieuw.Enabled = false;
			prev_state = "";
			lblStatus.Text = Resources.Updating + "...";
			depl.UpdateAsync();
		}

		void depl_UpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
		{
			progressBar1.Value = e.ProgressPercentage;
			lblProgressPercent.Text = e.ProgressPercentage.ToString() + "%";
			lblProgressBytes.Text = e.BytesCompleted.ToString() + " / " + e.BytesTotal.ToString() + " bytes";
			if (e.State.ToString() != prev_state)
			{
				prev_state = e.State.ToString();
				txtInfo.AppendText(Resources.Updating + ": " + e.State.ToString() + "\r\n");
			}
		}

		void depl_UpdateCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				lblStatus.Text = Resources.UpdateInterrupted;
				txtInfo.AppendText(Resources.UpdateInterrupted + "\r\n");
			}
			else if (e.Error != null)
			{
				lblStatus.Text = Resources.ErrorOccurred;
				txtInfo.AppendText(Resources.ErrorOccurred + ": " + e.Error.Message);
			}
			else
			{
				lblStatus.Text = Resources.UpdateComplete;

				// Update installed, prompt the user to see if they'd like to 
				// nowrestart
				BeginInvoke((Action)delegate
				{
					if (MessageBox.Show(Resources.UpdateCompleteRestart, Resources.Restart + "?" , MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					{
						Application.Restart();
					}
				});
			}
		}

		private void btnSluiten_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
