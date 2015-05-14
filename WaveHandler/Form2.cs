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
	public partial class Form2 : Form
	{
		public Form2()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Returns the project name entered by the user
		/// </summary>
		/// <returns>User entered project name.</returns>
		public string GetText()
		{
			return textBox1.Text;
		}

		/// <summary>
		/// Simply sets the dialog result and closes this form again
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// Simply sets the dialog result and closes this form again
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
