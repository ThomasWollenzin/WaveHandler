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
	public partial class MessageBoxSimple : Form
	{
		public MessageBoxSimple()
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
			messageboxsimplelabel1.Text = sText;
		}
	}
}
