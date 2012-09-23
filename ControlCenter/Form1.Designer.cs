namespace ControlCenter
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
            this.btnPopulateSql = new System.Windows.Forms.Button();
            this.btnClearQueue = new System.Windows.Forms.Button();
            this.btnPopulateCloud = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnRefreshCounts = new System.Windows.Forms.Button();
            this.txtPopulateSql = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPopulateSqlLoremIpsum = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPopulateCloud = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnPopulateSql
            // 
            this.btnPopulateSql.Location = new System.Drawing.Point(13, 70);
            this.btnPopulateSql.Name = "btnPopulateSql";
            this.btnPopulateSql.Size = new System.Drawing.Size(192, 23);
            this.btnPopulateSql.TabIndex = 0;
            this.btnPopulateSql.Text = "Request Sql Data Population";
            this.btnPopulateSql.UseVisualStyleBackColor = true;
            this.btnPopulateSql.Click += new System.EventHandler(this.btnPopulateSql_Click);
            // 
            // btnClearQueue
            // 
            this.btnClearQueue.Location = new System.Drawing.Point(13, 12);
            this.btnClearQueue.Name = "btnClearQueue";
            this.btnClearQueue.Size = new System.Drawing.Size(75, 23);
            this.btnClearQueue.TabIndex = 1;
            this.btnClearQueue.Text = "Clear Queue";
            this.btnClearQueue.UseVisualStyleBackColor = true;
            this.btnClearQueue.Click += new System.EventHandler(this.btnClearQueue_Click);
            // 
            // btnPopulateCloud
            // 
            this.btnPopulateCloud.Location = new System.Drawing.Point(223, 70);
            this.btnPopulateCloud.Name = "btnPopulateCloud";
            this.btnPopulateCloud.Size = new System.Drawing.Size(192, 23);
            this.btnPopulateCloud.TabIndex = 4;
            this.btnPopulateCloud.Text = "Request Cloud Data Population";
            this.btnPopulateCloud.UseVisualStyleBackColor = true;
            this.btnPopulateCloud.Click += new System.EventHandler(this.btnPopulateCloud_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(13, 41);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset Data";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // button1
            // 
            this.btnRefreshCounts.Location = new System.Drawing.Point(12, 235);
            this.btnRefreshCounts.Name = "btnRefreshCounts";
            this.btnRefreshCounts.Size = new System.Drawing.Size(192, 23);
            this.btnRefreshCounts.TabIndex = 6;
            this.btnRefreshCounts.Text = "Refresh Counts";
            this.btnRefreshCounts.UseVisualStyleBackColor = true;
            this.btnRefreshCounts.Click += new System.EventHandler(this.btnRefreshCounts_Click);
            // 
            // txtPopulateSql
            // 
            this.txtPopulateSql.Enabled = false;
            this.txtPopulateSql.Location = new System.Drawing.Point(104, 99);
            this.txtPopulateSql.Name = "txtPopulateSql";
            this.txtPopulateSql.Size = new System.Drawing.Size(100, 20);
            this.txtPopulateSql.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "PopulateSql";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "PopulateSqlLI";
            // 
            // txtPopulateSqlLoremIpsum
            // 
            this.txtPopulateSqlLoremIpsum.Enabled = false;
            this.txtPopulateSqlLoremIpsum.Location = new System.Drawing.Point(104, 125);
            this.txtPopulateSqlLoremIpsum.Name = "txtPopulateSqlLoremIpsum";
            this.txtPopulateSqlLoremIpsum.Size = new System.Drawing.Size(100, 20);
            this.txtPopulateSqlLoremIpsum.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "PopulateCloud";
            // 
            // txtPopulateCloud
            // 
            this.txtPopulateCloud.Enabled = false;
            this.txtPopulateCloud.Location = new System.Drawing.Point(104, 151);
            this.txtPopulateCloud.Name = "txtPopulateCloud";
            this.txtPopulateCloud.Size = new System.Drawing.Size(100, 20);
            this.txtPopulateCloud.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 330);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPopulateCloud);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPopulateSqlLoremIpsum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPopulateSql);
            this.Controls.Add(this.btnRefreshCounts);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnPopulateCloud);
            this.Controls.Add(this.btnClearQueue);
            this.Controls.Add(this.btnPopulateSql);
            this.Name = "Form1";
            this.Text = "ControlCenter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPopulateSql;
        private System.Windows.Forms.Button btnClearQueue;
        private System.Windows.Forms.Button btnPopulateCloud;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnRefreshCounts;
        private System.Windows.Forms.TextBox txtPopulateSql;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPopulateSqlLoremIpsum;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPopulateCloud;
    }
}

