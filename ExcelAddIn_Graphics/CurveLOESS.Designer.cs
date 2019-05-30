namespace ExcelAddIn_Graphics
{
    partial class CurveLOESS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CurveLOESS));
            this.textBox_Bandwidth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_OK = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox_Bandwidth
            // 
            this.textBox_Bandwidth.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Bandwidth.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_Bandwidth.ForeColor = System.Drawing.SystemColors.InfoText;
            this.textBox_Bandwidth.Location = new System.Drawing.Point(155, 334);
            this.textBox_Bandwidth.Name = "textBox_Bandwidth";
            this.textBox_Bandwidth.Size = new System.Drawing.Size(76, 26);
            this.textBox_Bandwidth.TabIndex = 10;
            this.textBox_Bandwidth.Text = "10";
            this.textBox_Bandwidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_Bandwidth.TextChanged += new System.EventHandler(this.textBox_Bandwidth_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.SkyBlue;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(27, 336);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 21);
            this.label1.TabIndex = 9;
            this.label1.Text = "LOESS平滑宽度";
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(252, 334);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(81, 26);
            this.button_OK.TabIndex = 8;
            this.button_OK.Text = "确定";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(31, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(302, 303);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // CurveLOESS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 381);
            this.Controls.Add(this.textBox_Bandwidth);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.pictureBox1);
            this.MaximizeBox = false;
            this.Name = "CurveLOESS";
            this.Text = "CurveLOESS";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_Bandwidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}