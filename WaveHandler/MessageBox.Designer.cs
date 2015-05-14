namespace WaveHandler
{
	partial class GeneralMessageBox
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneralMessageBox));
			this.messageboxlabel1 = new System.Windows.Forms.Label();
			this.messageboxbutton1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// messageboxlabel1
			// 
			this.messageboxlabel1.ForeColor = System.Drawing.Color.Silver;
			this.messageboxlabel1.Location = new System.Drawing.Point(12, 9);
			this.messageboxlabel1.Name = "messageboxlabel1";
			this.messageboxlabel1.Size = new System.Drawing.Size(424, 151);
			this.messageboxlabel1.TabIndex = 1;
			this.messageboxlabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// messageboxbutton1
			// 
			this.messageboxbutton1.BackColor = System.Drawing.Color.Goldenrod;
			this.messageboxbutton1.Location = new System.Drawing.Point(187, 163);
			this.messageboxbutton1.Name = "messageboxbutton1";
			this.messageboxbutton1.Size = new System.Drawing.Size(75, 23);
			this.messageboxbutton1.TabIndex = 2;
			this.messageboxbutton1.Text = "Ok";
			this.messageboxbutton1.UseVisualStyleBackColor = false;
			this.messageboxbutton1.Click += new System.EventHandler(this.messageboxbutton1_Click);
			// 
			// GeneralMessageBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.ClientSize = new System.Drawing.Size(448, 198);
			this.ControlBox = false;
			this.Controls.Add(this.messageboxbutton1);
			this.Controls.Add(this.messageboxlabel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "GeneralMessageBox";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label messageboxlabel1;
		private System.Windows.Forms.Button messageboxbutton1;
	}
}