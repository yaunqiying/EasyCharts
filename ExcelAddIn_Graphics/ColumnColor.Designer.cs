namespace ExcelAddIn_Graphics
{
    partial class ColumnColor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColumnColor));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox_Bandwidth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_OK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.button_ColorSelection = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(30, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(302, 303);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
//            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // textBox_Bandwidth
            // 
            this.textBox_Bandwidth.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Bandwidth.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_Bandwidth.ForeColor = System.Drawing.SystemColors.InfoText;
            this.textBox_Bandwidth.Location = new System.Drawing.Point(139, 330);
            this.textBox_Bandwidth.Name = "textBox_Bandwidth";
            this.textBox_Bandwidth.Size = new System.Drawing.Size(76, 26);
            this.textBox_Bandwidth.TabIndex = 6;
            this.textBox_Bandwidth.Text = "60";
            this.textBox_Bandwidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_Bandwidth.TextChanged += new System.EventHandler(this.textBox_Bandwidth_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.SkyBlue;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(26, 332);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 21);
            this.label1.TabIndex = 5;
            this.label1.Text = "亮度对比参数";
//            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(254, 330);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(81, 60);
            this.button_OK.TabIndex = 4;
            this.button_OK.Text = "确定";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.SkyBlue;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(26, 365);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 21);
            this.label2.TabIndex = 7;
            this.label2.Text = "柱形颜色选择";
            // 
            // button_ColorSelection
            // 
            this.button_ColorSelection.BackColor = System.Drawing.Color.LightSkyBlue;
            this.button_ColorSelection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_ColorSelection.ForeColor = System.Drawing.Color.LightSkyBlue;
            this.button_ColorSelection.Location = new System.Drawing.Point(140, 362);
            this.button_ColorSelection.Name = "button_ColorSelection";
            this.button_ColorSelection.Size = new System.Drawing.Size(75, 28);
            this.button_ColorSelection.TabIndex = 8;
            this.button_ColorSelection.Text = "颜色选择";
            this.button_ColorSelection.UseVisualStyleBackColor = false;
            this.button_ColorSelection.Click += new System.EventHandler(this.button1_Click);
            // 
            // ColumnColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 411);
            this.Controls.Add(this.button_ColorSelection);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_Bandwidth);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ColumnColor";
            this.Text = "ColorColumn";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox_Bandwidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button_ColorSelection;

    }
}