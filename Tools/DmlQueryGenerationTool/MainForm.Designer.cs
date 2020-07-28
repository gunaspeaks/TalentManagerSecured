namespace Agilisium.TalentManager.Tools.DmlQueryGenerationTool
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
            this.migrateDataButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.clbTablesList = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnButton = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // migrateDataButton
            // 
            this.migrateDataButton.BackColor = System.Drawing.Color.RoyalBlue;
            this.migrateDataButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.migrateDataButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.migrateDataButton.Location = new System.Drawing.Point(224, 278);
            this.migrateDataButton.Name = "migrateDataButton";
            this.migrateDataButton.Size = new System.Drawing.Size(213, 31);
            this.migrateDataButton.TabIndex = 1;
            this.migrateDataButton.Text = "Generate DML Queries";
            this.migrateDataButton.UseVisualStyleBackColor = false;
            this.migrateDataButton.Click += new System.EventHandler(this.MigrateDataButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 382);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(454, 30);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = false;
            this.statusLabel.BackColor = System.Drawing.Color.LightGreen;
            this.statusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Adjust;
            this.statusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusLabel.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(439, 25);
            this.statusLabel.Spring = true;
            this.statusLabel.Text = "Ready";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // clbTablesList
            // 
            this.clbTablesList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clbTablesList.CheckOnClick = true;
            this.clbTablesList.ColumnWidth = 200;
            this.clbTablesList.FormattingEnabled = true;
            this.clbTablesList.Items.AddRange(new object[] {
            "BuLevel",
            "Certification",
            "DropDownCategory",
            "DropDownSubCategory",
            "EmpAssetDetail",
            "EmpCertification",
            "Employee",
            "EmployeeSkill",
            "Project",
            "ProjectAccount",
            "ProjectAllocation",
            "ResourceLevel",
            "TechSkill",
            "TechSkillCategory"});
            this.clbTablesList.Location = new System.Drawing.Point(15, 37);
            this.clbTablesList.Margin = new System.Windows.Forms.Padding(10);
            this.clbTablesList.MultiColumn = true;
            this.clbTablesList.Name = "clbTablesList";
            this.clbTablesList.Size = new System.Drawing.Size(422, 146);
            this.clbTablesList.Sorted = true;
            this.clbTablesList.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(225, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Tables to Generate DML Script";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 206);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(219, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Path to Generate DML Scripts";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(15, 225);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(336, 23);
            this.txtPath.TabIndex = 6;
            // 
            // btnButton
            // 
            this.btnButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnButton.Location = new System.Drawing.Point(362, 225);
            this.btnButton.Name = "btnButton";
            this.btnButton.Size = new System.Drawing.Size(75, 23);
            this.btnButton.TabIndex = 7;
            this.btnButton.Text = "&Browse";
            this.btnButton.UseVisualStyleBackColor = true;
            this.btnButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 412);
            this.Controls.Add(this.btnButton);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.clbTablesList);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.migrateDataButton);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DML Script Generation Tool";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button migrateDataButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.CheckedListBox clbTablesList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button btnButton;
    }
}

