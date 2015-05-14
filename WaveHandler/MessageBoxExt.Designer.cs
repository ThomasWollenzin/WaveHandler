namespace WaveHandler
{
	partial class MessageBoxExt
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageBoxExt));
			this.messageboxextokbutton1 = new System.Windows.Forms.Button();
			this.messageboxextlabel1 = new System.Windows.Forms.Label();
			this.messageboxextcancelbutton1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// messageboxextokbutton1
			// 
			this.messageboxextokbutton1.BackColor = System.Drawing.Color.Goldenrod;
			this.messageboxextokbutton1.Location = new System.Drawing.Point(138, 163);
			this.messageboxextokbutton1.Name = "messageboxextokbutton1";
			this.messageboxextokbutton1.Size = new System.Drawing.Size(75, 23);
			this.messageboxextokbutton1.TabIndex = 3;
			this.messageboxextokbutton1.Text = "Ok";
			this.messageboxextokbutton1.UseVisualStyleBackColor = false;
			this.messageboxextokbutton1.Click += new System.EventHandler(this.messageboxextokbutton1_Click);
			// 
			// messageboxextlabel1
			// 
			this.messageboxextlabel1.ForeColor = System.Drawing.Color.Silver;
			this.messageboxextlabel1.Location = new System.Drawing.Point(12, 9);
			this.messageboxextlabel1.Name = "messageboxextlabel1";
			this.messageboxextlabel1.Size = new System.Drawing.Size(424, 151);
			this.messageboxextlabel1.TabIndex = 4;
			this.messageboxextlabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// messageboxextcancelbutton1
			// 
			this.messageboxextcancelbutton1.BackColor = System.Drawing.Color.Goldenrod;
			this.messageboxextcancelbutton1.Location = new System.Drawing.Point(235, 163);
			this.messageboxextcancelbutton1.Name = "messageboxextcancelbutton1";
			this.messageboxextcancelbutton1.Size = new System.Drawing.Size(75, 23);
			this.messageboxextcancelbutton1.TabIndex = 5;
			this.messageboxextcancelbutton1.Text = "Cancel";
			this.messageboxextcancelbutton1.UseVisualStyleBackColor = false;
			this.messageboxextcancelbutton1.Click += new System.EventHandler(this.messageboxextcancelbutton1_Click);
			// 
			// MessageBoxExt
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.ClientSize = new System.Drawing.Size(448, 198);
			this.ControlBox = false;
			this.Controls.Add(this.messageboxextcancelbutton1);
			this.Controls.Add(this.messageboxextlabel1);
			this.Controls.Add(this.messageboxextokbutton1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MessageBoxExt";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button messageboxextokbutton1;
		private System.Windows.Forms.Label messageboxextlabel1;
		private System.Windows.Forms.Button messageboxextcancelbutton1;
	}
}