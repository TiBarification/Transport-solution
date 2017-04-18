namespace Lab3_Transport
{
    partial class FormInput
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.maskedTextBox_ACount = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBox_BCount = new System.Windows.Forms.MaskedTextBox();
            this.button_Click = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "A count:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "B count:";
            // 
            // maskedTextBox_ACount
            // 
            this.maskedTextBox_ACount.Location = new System.Drawing.Point(63, 10);
            this.maskedTextBox_ACount.Mask = "00000";
            this.maskedTextBox_ACount.Name = "maskedTextBox_ACount";
            this.maskedTextBox_ACount.Size = new System.Drawing.Size(47, 20);
            this.maskedTextBox_ACount.TabIndex = 2;
            this.maskedTextBox_ACount.ValidatingType = typeof(int);
            // 
            // maskedTextBox_BCount
            // 
            this.maskedTextBox_BCount.Location = new System.Drawing.Point(63, 38);
            this.maskedTextBox_BCount.Mask = "00000";
            this.maskedTextBox_BCount.Name = "maskedTextBox_BCount";
            this.maskedTextBox_BCount.Size = new System.Drawing.Size(47, 20);
            this.maskedTextBox_BCount.TabIndex = 3;
            this.maskedTextBox_BCount.ValidatingType = typeof(int);
            // 
            // button_Click
            // 
            this.button_Click.Location = new System.Drawing.Point(13, 64);
            this.button_Click.Name = "button_Click";
            this.button_Click.Size = new System.Drawing.Size(97, 23);
            this.button_Click.TabIndex = 4;
            this.button_Click.Text = ">CLICK<";
            this.button_Click.UseVisualStyleBackColor = true;
            this.button_Click.Click += new System.EventHandler(this.button_Click_Click);
            // 
            // FormInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(120, 99);
            this.Controls.Add(this.button_Click);
            this.Controls.Add(this.maskedTextBox_BCount);
            this.Controls.Add(this.maskedTextBox_ACount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormInput";
            this.ShowIcon = false;
            this.Text = "Data settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox maskedTextBox_ACount;
        private System.Windows.Forms.MaskedTextBox maskedTextBox_BCount;
        private System.Windows.Forms.Button button_Click;
    }
}