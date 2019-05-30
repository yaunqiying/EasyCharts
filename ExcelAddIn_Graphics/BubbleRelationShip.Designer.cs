namespace ExcelAddIn_Graphics
{
    partial class BubbleRelationShip
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BubbleRelationShip));
            this.textBox_Bandwidth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_OK = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_ColorSelection = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.comboBox_FourierMethod = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox_Bandwidth
            // 
            this.textBox_Bandwidth.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_Bandwidth.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_Bandwidth.ForeColor = System.Drawing.SystemColors.InfoText;
            this.textBox_Bandwidth.Location = new System.Drawing.Point(139, 338);
            this.textBox_Bandwidth.Name = "textBox_Bandwidth";
            this.textBox_Bandwidth.Size = new System.Drawing.Size(93, 26);
            this.textBox_Bandwidth.TabIndex = 15;
            this.textBox_Bandwidth.Text = "21";
            this.textBox_Bandwidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_Bandwidth.TextChanged += new System.EventHandler(this.textBox_Bandwidth_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.SkyBlue;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(27, 340);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 21);
            this.label1.TabIndex = 14;
            this.label1.Text = "方形最大宽度";
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(238, 340);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(98, 91);
            this.button_OK.TabIndex = 13;
            this.button_OK.Text = "确定";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(31, 17);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(305, 301);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // button_ColorSelection
            // 
            this.button_ColorSelection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_ColorSelection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_ColorSelection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_ColorSelection.Location = new System.Drawing.Point(140, 370);
            this.button_ColorSelection.Name = "button_ColorSelection";
            this.button_ColorSelection.Size = new System.Drawing.Size(92, 28);
            this.button_ColorSelection.TabIndex = 17;
            this.button_ColorSelection.Text = "颜色选择";
            this.button_ColorSelection.UseVisualStyleBackColor = false;
            this.button_ColorSelection.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.SkyBlue;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(26, 373);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 21);
            this.label2.TabIndex = 16;
            this.label2.Text = "气泡颜色选择";
            // 
            // comboBox_FourierMethod
            // 
            this.comboBox_FourierMethod.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_FourierMethod.FormattingEnabled = true;
            this.comboBox_FourierMethod.Items.AddRange(new object[] {
            "Circle",
            "Square"});
            this.comboBox_FourierMethod.Location = new System.Drawing.Point(139, 406);
            this.comboBox_FourierMethod.Name = "comboBox_FourierMethod";
            this.comboBox_FourierMethod.Size = new System.Drawing.Size(93, 25);
            this.comboBox_FourierMethod.TabIndex = 18;
            this.comboBox_FourierMethod.SelectedIndexChanged += new System.EventHandler(this.comboBox_FourierMethod_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.SkyBlue;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(26, 410);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 21);
            this.label3.TabIndex = 19;
            this.label3.Text = "气泡类型选择";
            // 
            // BubbleRelationShip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 440);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_FourierMethod);
            this.Controls.Add(this.button_ColorSelection);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_Bandwidth);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.pictureBox1);
            this.Name = "BubbleRelationShip";
            this.Text = "BubbleRelationShip";
            this.Load += new System.EventHandler(this.BubbleRelationShip_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_Bandwidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button_ColorSelection;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ComboBox comboBox_FourierMethod;
        private System.Windows.Forms.Label label3;
    }
}