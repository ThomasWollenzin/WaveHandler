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
	public partial class ProgressBar : Form
	{
		public ProgressBar()
		{
			InitializeComponent();
		}

		public void SetProgress(double fProgress)
		{
			double fClampedProgress = Clamp(fProgress, 0.0, 100.0);
			progressBar1.Value = Convert.ToInt32(fClampedProgress);
		}

		public void SetText(string sText)
		{
			label1.Text = sText;
		}

		private double Clamp(double fValue, double fMin, double fMax)
		{
			return (fValue < fMin) ? fMin : (fValue > fMax) ? fMax : fValue;
		}

		public void CenterDialogToParent()
		{
			CenterToParent();
		}
	}
}
