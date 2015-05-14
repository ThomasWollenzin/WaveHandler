using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WaveHandler
{
	public partial class ConversionSuccess : Form
	{
		string sFolderPath = String.Empty;

		public ConversionSuccess()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Simply centers this dialog to its parent.
		/// </summary>
		public void CenterDialogToParent()
		{
			CenterToParent();
		}

		/// <summary>
		/// Set a custom caption text.
		/// </summary>
		/// <param name="sCaption"></param>
		public void SetCaption(string sCaption)
		{
			this.Text = sCaption;
		}

		/// <summary>
		/// Set the text this dialog displays.
		/// </summary>
		/// <param name="sText"></param>
		public void SetText(string sText)
		{
			this.conversionsuccesslabel1.Text = sText;
		}

		/// <summary>
		/// Set the path to the folder to open.
		/// </summary>
		/// <param name="sPath"></param>
		public void SetFolderPath(string sPath)
		{
			sFolderPath = sPath;
		}

		/// <summary>
		/// When this button is clicked just set the result to OK and close it.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void conversionsuccessbutton1_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// When this button is clicked set the result to OK as well but open an Explorer instance and navigate to the target folder.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void conversionsuccessbutton2_Click(object sender, EventArgs e)
		{
			if (!String.IsNullOrEmpty(sFolderPath))
			{
				string sWinDir = Environment.GetEnvironmentVariable("WINDIR");
				Process oProcess = new Process();
				oProcess.StartInfo.FileName = sWinDir + @"\explorer.exe";
				oProcess.StartInfo.Arguments = sFolderPath;
				oProcess.Start();
			}

			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}