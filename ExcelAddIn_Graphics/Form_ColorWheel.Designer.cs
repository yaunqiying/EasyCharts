namespace ExcelAddIn_Graphics
{
    partial class Form_ColorWheel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ColorWheel));
            this.tabControl_ColorWheel = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnGetColor = new System.Windows.Forms.Button();
            this.picColor = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtARGB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRGB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl_ColorWheel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picColor)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl_ColorWheel
            // 
            this.tabControl_ColorWheel.Controls.Add(this.tabPage1);
            this.tabControl_ColorWheel.Controls.Add(this.tabPage2);
            this.tabControl_ColorWheel.Controls.Add(this.tabPage3);
            this.tabControl_ColorWheel.Location = new System.Drawing.Point(12, 12);
            this.tabControl_ColorWheel.Name = "tabControl_ColorWheel";
            this.tabControl_ColorWheel.SelectedIndex = 0;
            this.tabControl_ColorWheel.Size = new System.Drawing.Size(410, 425);
            this.tabControl_ColorWheel.TabIndex = 1;
            this.tabControl_ColorWheel.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_ColorWheel_Selected);
            // 
            // tabPage1
            // 
            this.tabPage1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabPage1.BackgroundImage")));
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(402, 399);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Tag = "1";
            this.tabPage1.Text = "12色5轮色轮";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabPage2.BackgroundImage")));
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(402, 399);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Tag = "2";
            this.tabPage2.Text = "24色7轮色轮";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(402, 399);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Tag = "3";
            this.tabPage3.Text = "颜色搭配指南";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnGetColor
            // 
            this.btnGetColor.Location = new System.Drawing.Point(353, 450);
            this.btnGetColor.Name = "btnGetColor";
            this.btnGetColor.Size = new System.Drawing.Size(65, 44);
            this.btnGetColor.TabIndex = 10;
            this.btnGetColor.Text = "颜色拾取";
            this.btnGetColor.UseVisualStyleBackColor = true;
            this.btnGetColor.Click += new System.EventHandler(this.btnGetColor_Click);
            // 
            // picColor
            // 
            this.picColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picColor.Location = new System.Drawing.Point(300, 450);
            this.picColor.Name = "picColor";
            this.picColor.Size = new System.Drawing.Size(47, 44);
            this.picColor.TabIndex = 9;
            this.picColor.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtARGB);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtRGB);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 443);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(282, 51);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "颜色显示";
            // 
            // txtARGB
            // 
            this.txtARGB.Location = new System.Drawing.Point(181, 18);
            this.txtARGB.Name = "txtARGB";
            this.txtARGB.Size = new System.Drawing.Size(81, 21);
            this.txtARGB.TabIndex = 3;
            this.txtARGB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(140, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "ARGB：";
            // 
            // txtRGB
            // 
            this.txtRGB.Location = new System.Drawing.Point(47, 18);
            this.txtRGB.Name = "txtRGB";
            this.txtRGB.Size = new System.Drawing.Size(81, 21);
            this.txtRGB.TabIndex = 0;
            this.txtRGB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "RGB：";
            // 
            // Form_ColorWheel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 506);
            this.Controls.Add(this.btnGetColor);
            this.Controls.Add(this.picColor);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.tabControl_ColorWheel);
            this.MaximizeBox = false;
            this.Name = "Form_ColorWheel";
            this.Text = "色轮";
            this.tabControl_ColorWheel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picColor)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl_ColorWheel;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnGetColor;
        private System.Windows.Forms.PictureBox picColor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtARGB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRGB;
        private System.Windows.Forms.Label label5;
    }
}