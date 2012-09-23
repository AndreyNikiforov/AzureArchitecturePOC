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
            this.SuspendLayout();
            // 
            // btnPopulateSql
            // 
            this.btnPopulateSql.Location = new System.Drawing.Point(13, 41);
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
            this.btnPopulateCloud.Location = new System.Drawing.Point(220, 41);
            this.btnPopulateCloud.Name = "btnPopulateCloud";
            this.btnPopulateCloud.Size = new System.Drawing.Size(192, 23);
            this.btnPopulateCloud.TabIndex = 4;
            this.btnPopulateCloud.Text = "Request Cloud Data Population";
            this.btnPopulateCloud.UseVisualStyleBackColor = true;
            this.btnPopulateCloud.Click += new System.EventHandler(this.btnPopulateCloud_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(220, 12);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset Data";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 330);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnPopulateCloud);
            this.Controls.Add(this.btnClearQueue);
            this.Controls.Add(this.btnPopulateSql);
            this.Name = "Form1";
            this.Text = "ControlCenter";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPopulateSql;
        private System.Windows.Forms.Button btnClearQueue;
        private System.Windows.Forms.Button btnPopulateCloud;
        private System.Windows.Forms.Button btnReset;
    }
}

