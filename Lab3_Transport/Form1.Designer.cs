namespace Lab3_Transport
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.dataGridView_Solve = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_New = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_Clear = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_SolveTransport = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_MyVar = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Solve)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView_Solve
            // 
            this.dataGridView_Solve.AllowUserToAddRows = false;
            this.dataGridView_Solve.AllowUserToDeleteRows = false;
            this.dataGridView_Solve.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Solve.Location = new System.Drawing.Point(13, 35);
            this.dataGridView_Solve.Name = "dataGridView_Solve";
            this.dataGridView_Solve.Size = new System.Drawing.Size(903, 383);
            this.dataGridView_Solve.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_New,
            this.toolStripSeparator1,
            this.toolStripButton_Clear,
            this.toolStripButton_SolveTransport,
            this.toolStripButton_MyVar});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(928, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_New
            // 
            this.toolStripButton_New.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_New.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_New.Image")));
            this.toolStripButton_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_New.Name = "toolStripButton_New";
            this.toolStripButton_New.Size = new System.Drawing.Size(35, 22);
            this.toolStripButton_New.Text = "New";
            this.toolStripButton_New.Click += new System.EventHandler(this.toolStripButton_New_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_Clear
            // 
            this.toolStripButton_Clear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_Clear.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Clear.Image")));
            this.toolStripButton_Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Clear.Name = "toolStripButton_Clear";
            this.toolStripButton_Clear.Size = new System.Drawing.Size(38, 22);
            this.toolStripButton_Clear.Text = "Clear";
            this.toolStripButton_Clear.Click += new System.EventHandler(this.toolStripButton_Clear_Click);
            // 
            // toolStripButton_SolveTransport
            // 
            this.toolStripButton_SolveTransport.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton_SolveTransport.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.toolStripButton_SolveTransport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_SolveTransport.Enabled = false;
            this.toolStripButton_SolveTransport.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_SolveTransport.Image")));
            this.toolStripButton_SolveTransport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_SolveTransport.Margin = new System.Windows.Forms.Padding(0, 5, 25, 1);
            this.toolStripButton_SolveTransport.Name = "toolStripButton_SolveTransport";
            this.toolStripButton_SolveTransport.Size = new System.Drawing.Size(39, 19);
            this.toolStripButton_SolveTransport.Text = "Solve";
            this.toolStripButton_SolveTransport.Click += new System.EventHandler(this.toolStripButton_SolveTransport_Click);
            // 
            // toolStripButton_MyVar
            // 
            this.toolStripButton_MyVar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_MyVar.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_MyVar.Image")));
            this.toolStripButton_MyVar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_MyVar.Name = "toolStripButton_MyVar";
            this.toolStripButton_MyVar.Size = new System.Drawing.Size(67, 22);
            this.toolStripButton_MyVar.Text = "My Variant";
            this.toolStripButton_MyVar.Click += new System.EventHandler(this.toolStripButton_MyVar_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(928, 430);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dataGridView_Solve);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "Transport solution finder";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Solve)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_Solve;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_New;
        private System.Windows.Forms.ToolStripButton toolStripButton_Clear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton_SolveTransport;
        private System.Windows.Forms.ToolStripButton toolStripButton_MyVar;
    }
}

