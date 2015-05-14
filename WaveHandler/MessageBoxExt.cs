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
	public partial class MessageBoxExt : Form
	{
		public MessageBoxExt()
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
			messageboxextlabel1.Text = sText;
		}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sText"></param>
    public void SetOkButtonText(string sText)
    {
      this.messageboxextokbutton1.Text = sText;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sText"></param>
    public void SetCancelButtonText(string sText)
    {
      this.messageboxextcancelbutton1.Text = sText;
    }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void messageboxextokbutton1_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void messageboxextcancelbutton1_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
