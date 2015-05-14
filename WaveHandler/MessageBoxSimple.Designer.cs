namespace WaveHandler
{
	partial class MessageBoxSimple
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageBoxSimple));
			this.messageboxsimplelabel1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// messageboxsimplelabel1
			// 
			this.messageboxsimplelabel1.ForeColor = System.Drawing.Color.Silver;
			this.messageboxsimplelabel1.Location = new System.Drawing.Point(12, 9);
			this.messageboxsimplelabel1.Name = "messageboxsimplelabel1";
			this.messageboxsimplelabel1.Size = new System.Drawing.Size(424, 180);
			this.messageboxsimplelabel1.TabIndex = 5;
			this.messageboxsimplelabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// MessageBoxSimple
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.ClientSize = new System.Drawing.Size(448, 198);
			this.ControlBox = false;
			this.Controls.Add(this.messageboxsimplelabel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MessageBoxSimple";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label messageboxsimplelabel1;
	}
}