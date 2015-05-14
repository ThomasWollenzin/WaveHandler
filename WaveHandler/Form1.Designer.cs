namespace WaveHandler
{
	partial class Form1
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.button8 = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.ProjectFilesListBox = new ExtendedListBox.ExtendedListBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.AvailableFilesListBox = new ExtendedListBox.ExtendedListBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.button7 = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.LoadedProjectLabel = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.Color.Goldenrod;
			this.button1.Location = new System.Drawing.Point(91, 341);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(114, 37);
			this.button1.TabIndex = 1;
			this.button1.Text = "Merge";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.BackColor = System.Drawing.Color.Goldenrod;
			this.button2.Location = new System.Drawing.Point(91, 225);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(114, 37);
			this.button2.TabIndex = 2;
			this.button2.Text = "Load Project";
			this.button2.UseVisualStyleBackColor = false;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.BackColor = System.Drawing.Color.Goldenrod;
			this.button3.Location = new System.Drawing.Point(91, 399);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(114, 37);
			this.button3.TabIndex = 4;
			this.button3.Text = "Split";
			this.button3.UseVisualStyleBackColor = false;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.BackColor = System.Drawing.Color.Goldenrod;
			this.button4.Location = new System.Drawing.Point(605, 357);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(94, 37);
			this.button4.TabIndex = 5;
			this.button4.Text = "Add";
			this.button4.UseVisualStyleBackColor = false;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// button5
			// 
			this.button5.BackColor = System.Drawing.Color.Goldenrod;
			this.button5.Location = new System.Drawing.Point(605, 415);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(94, 37);
			this.button5.TabIndex = 6;
			this.button5.Text = "Remove";
			this.button5.UseVisualStyleBackColor = false;
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// button6
			// 
			this.button6.BackColor = System.Drawing.Color.Goldenrod;
			this.button6.Location = new System.Drawing.Point(91, 457);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(114, 37);
			this.button6.TabIndex = 7;
			this.button6.Text = "Search Directory";
			this.button6.UseVisualStyleBackColor = false;
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.Location = new System.Drawing.Point(91, 517);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(135, 17);
			this.checkBox1.TabIndex = 8;
			this.checkBox1.Text = "Search Sub Directories";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// button8
			// 
			this.button8.BackColor = System.Drawing.Color.Goldenrod;
			this.button8.ForeColor = System.Drawing.SystemColors.ControlText;
			this.button8.Location = new System.Drawing.Point(91, 617);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(114, 37);
			this.button8.TabIndex = 14;
			this.button8.Text = "Remove Mismatches";
			this.toolTip1.SetToolTip(this.button8, "Click here when removing several mismatching project files at once.");
			this.button8.UseVisualStyleBackColor = false;
			this.button8.Click += new System.EventHandler(this.button8_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.ProjectFilesListBox);
			this.groupBox1.ForeColor = System.Drawing.SystemColors.ButtonShadow;
			this.groupBox1.Location = new System.Drawing.Point(299, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(300, 712);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Project Files";
			// 
			// ProjectFilesListBox
			// 
			this.ProjectFilesListBox.AllowDrop = true;
			this.ProjectFilesListBox.BackColor = System.Drawing.SystemColors.InfoText;
			this.ProjectFilesListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.ProjectFilesListBox.ExtDisplayMember = "";
			this.ProjectFilesListBox.ExtValueMember = "";
			this.ProjectFilesListBox.ForeColor = System.Drawing.Color.Lime;
			this.ProjectFilesListBox.FormattingEnabled = true;
			this.ProjectFilesListBox.HorizontalScrollbar = true;
			this.ProjectFilesListBox.Location = new System.Drawing.Point(6, 24);
			this.ProjectFilesListBox.Name = "ProjectFilesListBox";
			this.ProjectFilesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.ProjectFilesListBox.Size = new System.Drawing.Size(288, 674);
			this.ProjectFilesListBox.TabIndex = 4;
			this.ProjectFilesListBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.ProjectFilesListBox_DragDrop);
			this.ProjectFilesListBox.DragOver += new System.Windows.Forms.DragEventHandler(this.ProjectFilesListBox_DragOver);
			this.ProjectFilesListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProjectFilesListBox_KeyDown);
			this.ProjectFilesListBox.MouseHover += new System.EventHandler(this.ProjectFilesListBox_MouseHover);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.AvailableFilesListBox);
			this.groupBox2.ForeColor = System.Drawing.SystemColors.ButtonShadow;
			this.groupBox2.Location = new System.Drawing.Point(706, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(300, 712);
			this.groupBox2.TabIndex = 10;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Available Files";
			// 
			// AvailableFilesListBox
			// 
			this.AvailableFilesListBox.BackColor = System.Drawing.SystemColors.InfoText;
			this.AvailableFilesListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.AvailableFilesListBox.ExtDisplayMember = "";
			this.AvailableFilesListBox.ExtValueMember = "";
			this.AvailableFilesListBox.ForeColor = System.Drawing.Color.Lime;
			this.AvailableFilesListBox.FormattingEnabled = true;
			this.AvailableFilesListBox.HorizontalScrollbar = true;
			this.AvailableFilesListBox.Location = new System.Drawing.Point(6, 24);
			this.AvailableFilesListBox.Name = "AvailableFilesListBox";
			this.AvailableFilesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.AvailableFilesListBox.Size = new System.Drawing.Size(288, 674);
			this.AvailableFilesListBox.TabIndex = 1;
			this.AvailableFilesListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AvailableFilesListBox_KeyDown);
			this.AvailableFilesListBox.MouseHover += new System.EventHandler(this.AvailableFilesListBox_MouseHover);
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new System.Drawing.Point(91, 540);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.checkBox2.Size = new System.Drawing.Size(102, 17);
			this.checkBox2.TabIndex = 11;
			this.checkBox2.Text = "Show File Paths";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// button7
			// 
			this.button7.BackColor = System.Drawing.Color.Goldenrod;
			this.button7.Location = new System.Drawing.Point(91, 283);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(114, 37);
			this.button7.TabIndex = 12;
			this.button7.Text = "Merge Settings";
			this.button7.UseVisualStyleBackColor = false;
			this.button7.Click += new System.EventHandler(this.button7_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.LoadedProjectLabel);
			this.groupBox3.ForeColor = System.Drawing.SystemColors.ButtonShadow;
			this.groupBox3.Location = new System.Drawing.Point(12, 134);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(270, 46);
			this.groupBox3.TabIndex = 13;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Loaded Project";
			// 
			// LoadedProjectLabel
			// 
			this.LoadedProjectLabel.AutoSize = true;
			this.LoadedProjectLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LoadedProjectLabel.ForeColor = System.Drawing.Color.DimGray;
			this.LoadedProjectLabel.Location = new System.Drawing.Point(6, 19);
			this.LoadedProjectLabel.Name = "LoadedProjectLabel";
			this.LoadedProjectLabel.Size = new System.Drawing.Size(105, 13);
			this.LoadedProjectLabel.TabIndex = 14;
			this.LoadedProjectLabel.Text = "NoProjectLoaded";
			this.LoadedProjectLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.ClientSize = new System.Drawing.Size(1018, 736);
			this.Controls.Add(this.button8);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.checkBox2);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "WaveHandler v1.71 by Thomas Wollenzin";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Button button7;
		private ExtendedListBox.ExtendedListBox AvailableFilesListBox;
		private ExtendedListBox.ExtendedListBox ProjectFilesListBox;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label LoadedProjectLabel;
		private System.Windows.Forms.Button button8;
	}
}

