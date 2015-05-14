using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WaveHandler
{
	public partial class GeneralMessageBox : Form
	{
		public GeneralMessageBox()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 
		/// </summary>
		public void CenterDialogToParent()
		{
			CenterToParent();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sCaption"></param>
		public void SetCaption(string sCaption)
		{
			this.Text = sCaption;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sText"></param>
		public void SetText(string sText)
		{
			messageboxlabel1.Text = sText;
		}

		public void SetSize(int nWidth, int nHeight)
		{
			Width = nWidth;
			Height = nHeight;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void messageboxbutton1_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
