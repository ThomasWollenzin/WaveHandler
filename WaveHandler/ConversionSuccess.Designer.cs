namespace WaveHandler
{
	partial class ConversionSuccess
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConversionSuccess));
			this.conversionsuccesslabel1 = new System.Windows.Forms.Label();
			this.conversionsuccessbutton1 = new System.Windows.Forms.Button();
			this.conversionsuccessbutton2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// conversionsuccesslabel1
			// 
			this.conversionsuccesslabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.conversionsuccesslabel1.ForeColor = System.Drawing.Color.Silver;
			this.conversionsuccesslabel1.Location = new System.Drawing.Point(12, 9);
			this.conversionsuccesslabel1.Name = "conversionsuccesslabel1";
			this.conversionsuccesslabel1.Size = new System.Drawing.Size(268, 98);
			this.conversionsuccesslabel1.TabIndex = 0;
			this.conversionsuccesslabel1.Text = "non-set";
			this.conversionsuccesslabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// conversionsuccessbutton1
			// 
			this.conversionsuccessbutton1.BackColor = System.Drawing.Color.Goldenrod;
			this.conversionsuccessbutton1.Location = new System.Drawing.Point(68, 110);
			this.conversionsuccessbutton1.Name = "conversionsuccessbutton1";
			this.conversionsuccessbutton1.Size = new System.Drawing.Size(75, 23);
			this.conversionsuccessbutton1.TabIndex = 1;
			this.conversionsuccessbutton1.Text = "Ok";
			this.conversionsuccessbutton1.UseVisualStyleBackColor = false;
			this.conversionsuccessbutton1.Click += new System.EventHandler(this.conversionsuccessbutton1_Click);
			// 
			// conversionsuccessbutton2
			// 
			this.conversionsuccessbutton2.BackColor = System.Drawing.Color.Goldenrod;
			this.conversionsuccessbutton2.Location = new System.Drawing.Point(149, 110);
			this.conversionsuccessbutton2.Name = "conversionsuccessbutton2";
			this.conversionsuccessbutton2.Size = new System.Drawing.Size(75, 23);
			this.conversionsuccessbutton2.TabIndex = 2;
			this.conversionsuccessbutton2.Text = "Open Folder";
			this.conversionsuccessbutton2.UseVisualStyleBackColor = false;
			this.conversionsuccessbutton2.Click += new System.EventHandler(this.conversionsuccessbutton2_Click);
			// 
			// ConversionSuccess
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.ClientSize = new System.Drawing.Size(292, 166);
			this.ControlBox = false;
			this.Controls.Add(this.conversionsuccessbutton2);
			this.Controls.Add(this.conversionsuccessbutton1);
			this.Controls.Add(this.conversionsuccesslabel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConversionSuccess";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label conversionsuccesslabel1;
		private System.Windows.Forms.Button conversionsuccessbutton1;
		private System.Windows.Forms.Button conversionsuccessbutton2;
	}
}