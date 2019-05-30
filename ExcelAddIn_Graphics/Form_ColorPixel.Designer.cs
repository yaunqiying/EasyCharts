namespace ExcelAddIn_Graphics
{
    partial class Form_ColorPixel
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
            this.btnGetColor = new System.Windows.Forms.Button();
            this.picColor = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtARGB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRGB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox_HSV = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_Lab = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picColor)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGetColor
            // 
            this.btnGetColor.Location = new System.Drawing.Point(147, 170);
            this.btnGetColor.Name = "btnGetColor";
            this.btnGetColor.Size = new System.Drawing.Size(39, 57);
            this.btnGetColor.TabIndex = 7;
            this.btnGetColor.Text = "颜色拾取";
            this.btnGetColor.UseVisualStyleBackColor = true;
            this.btnGetColor.Click += new System.EventHandler(this.btnGetColor_Click);
            // 
            // picColor
            // 
            this.picColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picColor.Location = new System.Drawing.Point(97, 170);
            this.picColor.Name = "picColor";
            this.picColor.Size = new System.Drawing.Size(44, 57);
            this.picColor.TabIndex = 6;
            this.picColor.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox_Lab);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBox_HSV);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtARGB);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtRGB);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(15, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(171, 141);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "颜色显示";
            // 
            // txtARGB
            // 
            this.txtARGB.Location = new System.Drawing.Point(54, 52);
            this.txtARGB.Name = "txtARGB";
            this.txtARGB.Size = new System.Drawing.Size(101, 21);
            this.txtARGB.TabIndex = 3;
            this.txtARGB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "ARGB：";
            // 
            // txtRGB
            // 
            this.txtRGB.Location = new System.Drawing.Point(55, 21);
            this.txtRGB.Name = "txtRGB";
            this.txtRGB.Size = new System.Drawing.Size(101, 21);
            this.txtRGB.TabIndex = 0;
            this.txtRGB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "RGB：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Location = new System.Drawing.Point(12, 163);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(79, 64);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "鼠标位置";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(41, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "lblY";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 42);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "Y：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(40, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "lblX";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(23, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "X：";
            // 
            // textBox_HSV
            // 
            this.textBox_HSV.Location = new System.Drawing.Point(54, 79);
            this.textBox_HSV.Name = "textBox_HSV";
            this.textBox_HSV.Size = new System.Drawing.Size(101, 21);
            this.textBox_HSV.TabIndex = 5;
            this.textBox_HSV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "HSV：";
            // 
            // textBox_Lab
            // 
            this.textBox_Lab.Location = new System.Drawing.Point(54, 106);
            this.textBox_Lab.Name = "textBox_Lab";
            this.textBox_Lab.Size = new System.Drawing.Size(101, 21);
            this.textBox_Lab.TabIndex = 7;
            this.textBox_Lab.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Lab：";
            // 
            // Form_ColorPixel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(208, 237);
            this.Controls.Add(this.btnGetColor);
            this.Controls.Add(this.picColor);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.MaximizeBox = false;
            this.Name = "Form_ColorPixel";
            this.Text = "颜色拾取";
            ((System.ComponentModel.ISupportInitialize)(this.picColor)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGetColor;
        private System.Windows.Forms.PictureBox picColor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtARGB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRGB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox_Lab;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_HSV;
        private System.Windows.Forms.Label label1;
    }
}