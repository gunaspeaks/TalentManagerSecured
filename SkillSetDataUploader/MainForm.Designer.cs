namespace SkillSetDataUploader
{
    partial class MainForm
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
            this.btnUploadData = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblStatusMsg = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dgRawData = new System.Windows.Forms.DataGridView();
            this.btnReadCsvFile = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgRawData)).BeginInit();
            this.SuspendLayout();
            // 
            // btnUploadData
            // 
            this.btnUploadData.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadData.Location = new System.Drawing.Point(875, 390);
            this.btnUploadData.Name = "btnUploadData";
            this.btnUploadData.Size = new System.Drawing.Size(119, 26);
            this.btnUploadData.TabIndex = 0;
            this.btnUploadData.Text = "Upload Data";
            this.btnUploadData.UseVisualStyleBackColor = true;
            this.btnUploadData.Click += new System.EventHandler(this.UploadDataButtonClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Raw Skillset CSV File";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(171, 23);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(309, 23);
            this.txtFilePath.TabIndex = 2;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.Location = new System.Drawing.Point(486, 23);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 28);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.BrowseButtonClick);
            // 
            // lblStatusMsg
            // 
            this.lblStatusMsg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblStatusMsg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatusMsg.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblStatusMsg.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.lblStatusMsg.Location = new System.Drawing.Point(0, 419);
            this.lblStatusMsg.Name = "lblStatusMsg";
            this.lblStatusMsg.Size = new System.Drawing.Size(1006, 19);
            this.lblStatusMsg.TabIndex = 4;
            this.lblStatusMsg.Text = "Select Raw Skillset CSV File";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "*.csv|";
            this.openFileDialog1.FileName = "Raw CSV File";
            this.openFileDialog1.Filter = "*.csv|";
            this.openFileDialog1.Title = "Please select raw CSV file";
            // 
            // dgRawData
            // 
            this.dgRawData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgRawData.Location = new System.Drawing.Point(12, 62);
            this.dgRawData.Name = "dgRawData";
            this.dgRawData.Size = new System.Drawing.Size(982, 322);
            this.dgRawData.TabIndex = 5;
            // 
            // btnReadCsvFile
            // 
            this.btnReadCsvFile.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReadCsvFile.Location = new System.Drawing.Point(567, 22);
            this.btnReadCsvFile.Name = "btnReadCsvFile";
            this.btnReadCsvFile.Size = new System.Drawing.Size(119, 29);
            this.btnReadCsvFile.TabIndex = 6;
            this.btnReadCsvFile.Text = "Read CSV File";
            this.btnReadCsvFile.UseVisualStyleBackColor = true;
            this.btnReadCsvFile.Click += new System.EventHandler(this.ReadCsvFileButtonClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 438);
            this.Controls.Add(this.btnReadCsvFile);
            this.Controls.Add(this.dgRawData);
            this.Controls.Add(this.lblStatusMsg);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnUploadData);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Skillset Data Uploader";
            ((System.ComponentModel.ISupportInitialize)(this.dgRawData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUploadData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblStatusMsg;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridView dgRawData;
        private System.Windows.Forms.Button btnReadCsvFile;
    }
}

