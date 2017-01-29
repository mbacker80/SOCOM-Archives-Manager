namespace SOCOM_Archives
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
            this.lstListings = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtDetails = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.txtBrowsePath = new System.Windows.Forms.TextBox();
            this.txtArchive = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFileCount = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.optTypeFC = new System.Windows.Forms.RadioButton();
            this.optTypeC8 = new System.Windows.Forms.RadioButton();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstListings
            // 
            this.lstListings.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstListings.FormattingEnabled = true;
            this.lstListings.ItemHeight = 16;
            this.lstListings.Location = new System.Drawing.Point(11, 66);
            this.lstListings.Name = "lstListings";
            this.lstListings.Size = new System.Drawing.Size(338, 356);
            this.lstListings.TabIndex = 0;
            this.lstListings.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.lstListings.DoubleClick += new System.EventHandler(this.lstListings_DoubleClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(547, 461);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 27);
            this.button1.TabIndex = 1;
            this.button1.Text = "Open Archive";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtDetails
            // 
            this.txtDetails.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetails.Location = new System.Drawing.Point(355, 66);
            this.txtDetails.Multiline = true;
            this.txtDetails.Name = "txtDetails";
            this.txtDetails.Size = new System.Drawing.Size(309, 356);
            this.txtDetails.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(11, 459);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 27);
            this.button2.TabIndex = 3;
            this.button2.Text = "Extract File";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(101, 428);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(84, 27);
            this.button3.TabIndex = 4;
            this.button3.Text = "New Folder";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtBrowsePath
            // 
            this.txtBrowsePath.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBrowsePath.Location = new System.Drawing.Point(12, 40);
            this.txtBrowsePath.Name = "txtBrowsePath";
            this.txtBrowsePath.ReadOnly = true;
            this.txtBrowsePath.Size = new System.Drawing.Size(652, 22);
            this.txtBrowsePath.TabIndex = 5;
            // 
            // txtArchive
            // 
            this.txtArchive.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtArchive.Location = new System.Drawing.Point(90, 12);
            this.txtArchive.Name = "txtArchive";
            this.txtArchive.ReadOnly = true;
            this.txtArchive.Size = new System.Drawing.Size(573, 22);
            this.txtArchive.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "Archive:";
            // 
            // lblFileCount
            // 
            this.lblFileCount.AutoSize = true;
            this.lblFileCount.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileCount.Location = new System.Drawing.Point(352, 428);
            this.lblFileCount.Name = "lblFileCount";
            this.lblFileCount.Size = new System.Drawing.Size(112, 16);
            this.lblFileCount.TabIndex = 8;
            this.lblFileCount.Text = "File Count: 0";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(101, 461);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(84, 27);
            this.button4.TabIndex = 9;
            this.button4.Text = "Delete Folder";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(352, 472);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "Archive Type:";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(547, 494);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(117, 27);
            this.button5.TabIndex = 11;
            this.button5.Text = "Save Archive";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // optTypeFC
            // 
            this.optTypeFC.AutoSize = true;
            this.optTypeFC.Location = new System.Drawing.Point(470, 461);
            this.optTypeFC.Name = "optTypeFC";
            this.optTypeFC.Size = new System.Drawing.Size(34, 17);
            this.optTypeFC.TabIndex = 12;
            this.optTypeFC.TabStop = true;
            this.optTypeFC.Text = "fc";
            this.optTypeFC.UseVisualStyleBackColor = true;
            this.optTypeFC.CheckedChanged += new System.EventHandler(this.optTypeFC_CheckedChanged);
            // 
            // optTypeC8
            // 
            this.optTypeC8.AutoSize = true;
            this.optTypeC8.Checked = true;
            this.optTypeC8.Location = new System.Drawing.Point(470, 484);
            this.optTypeC8.Name = "optTypeC8";
            this.optTypeC8.Size = new System.Drawing.Size(37, 17);
            this.optTypeC8.TabIndex = 13;
            this.optTypeC8.TabStop = true;
            this.optTypeC8.Text = "c8";
            this.optTypeC8.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(191, 461);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(84, 27);
            this.button6.TabIndex = 14;
            this.button6.Text = "Delete File";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(11, 492);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(84, 27);
            this.button7.TabIndex = 15;
            this.button7.Text = "Extract All";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(191, 428);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(84, 27);
            this.button8.TabIndex = 16;
            this.button8.Text = "Add File";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(11, 428);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(84, 27);
            this.button9.TabIndex = 17;
            this.button9.Text = "Extract Folder";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(546, 428);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(117, 27);
            this.button10.TabIndex = 18;
            this.button10.Text = "New Archive";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(355, 494);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(109, 27);
            this.button11.TabIndex = 19;
            this.button11.Text = "Fix Checksums";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(498, 208);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(64, 45);
            this.button12.TabIndex = 20;
            this.button12.Text = "patch file extract";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Visible = false;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(101, 492);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(84, 27);
            this.button13.TabIndex = 21;
            this.button13.Text = "Compress";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(191, 492);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(84, 27);
            this.button14.TabIndex = 22;
            this.button14.Text = "Decompress";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 529);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.optTypeC8);
            this.Controls.Add(this.optTypeFC);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.lblFileCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtArchive);
            this.Controls.Add(this.txtBrowsePath);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtDetails);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lstListings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "SOCOM Archive Manager";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstListings;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtDetails;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox txtBrowsePath;
        private System.Windows.Forms.TextBox txtArchive;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFileCount;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.RadioButton optTypeFC;
        private System.Windows.Forms.RadioButton optTypeC8;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
    }
}

