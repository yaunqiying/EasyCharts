namespace ColorPalette
{
    partial class Form_ColorPalette
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_ReadImage = new System.Windows.Forms.Button();
            this.pictureBox_Image = new System.Windows.Forms.PictureBox();
            this.button_ColorPalette = new System.Windows.Forms.Button();
            this.button_ColorOutput = new System.Windows.Forms.Button();
            this.dataGridView_Color = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Color = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RGB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Image)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Color)).BeginInit();
            this.SuspendLayout();
            // 
            // button_ReadImage
            // 
            this.button_ReadImage.Location = new System.Drawing.Point(718, 420);
            this.button_ReadImage.Name = "button_ReadImage";
            this.button_ReadImage.Size = new System.Drawing.Size(160, 38);
            this.button_ReadImage.TabIndex = 5;
            this.button_ReadImage.Text = "导入图像";
            this.button_ReadImage.UseVisualStyleBackColor = true;
            this.button_ReadImage.Click += new System.EventHandler(this.button_ReadImage_Click);
            // 
            // pictureBox_Image
            // 
            this.pictureBox_Image.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox_Image.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_Image.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pictureBox_Image.Location = new System.Drawing.Point(12, 1);
            this.pictureBox_Image.Name = "pictureBox_Image";
            this.pictureBox_Image.Size = new System.Drawing.Size(689, 547);
            this.pictureBox_Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Image.TabIndex = 4;
            this.pictureBox_Image.TabStop = false;
            // 
            // button_ColorPalette
            // 
            this.button_ColorPalette.Location = new System.Drawing.Point(718, 464);
            this.button_ColorPalette.Name = "button_ColorPalette";
            this.button_ColorPalette.Size = new System.Drawing.Size(160, 38);
            this.button_ColorPalette.TabIndex = 6;
            this.button_ColorPalette.Text = "颜色提取";
            this.button_ColorPalette.UseVisualStyleBackColor = true;
            this.button_ColorPalette.Click += new System.EventHandler(this.button_ColorPalette_Click);
            // 
            // button_ColorOutput
            // 
            this.button_ColorOutput.Location = new System.Drawing.Point(718, 507);
            this.button_ColorOutput.Name = "button_ColorOutput";
            this.button_ColorOutput.Size = new System.Drawing.Size(160, 38);
            this.button_ColorOutput.TabIndex = 7;
            this.button_ColorOutput.Text = "颜色导出";
            this.button_ColorOutput.UseVisualStyleBackColor = true;
            this.button_ColorOutput.Click += new System.EventHandler(this.button_ColorOutput_Click);
            // 
            // dataGridView_Color
            // 
            this.dataGridView_Color.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_Color.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Color.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Color,
            this.RGB});
            this.dataGridView_Color.Location = new System.Drawing.Point(718, 13);
            this.dataGridView_Color.Name = "dataGridView_Color";
            this.dataGridView_Color.RowHeadersVisible = false;
            this.dataGridView_Color.RowHeadersWidth = 5;
            this.dataGridView_Color.RowTemplate.Height = 23;
            this.dataGridView_Color.Size = new System.Drawing.Size(160, 401);
            this.dataGridView_Color.TabIndex = 8;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Width = 30;
            // 
            // Color
            // 
            this.Color.HeaderText = "Color";
            this.Color.Name = "Color";
            this.Color.Width = 50;
            // 
            // RGB
            // 
            this.RGB.HeaderText = "RGB";
            this.RGB.Name = "RGB";
            this.RGB.Width = 80;
            // 
            // Form_ColorPalette
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 573);
            this.Controls.Add(this.dataGridView_Color);
            this.Controls.Add(this.button_ColorOutput);
            this.Controls.Add(this.button_ColorPalette);
            this.Controls.Add(this.button_ReadImage);
            this.Controls.Add(this.pictureBox_Image);
            this.Name = "Form_ColorPalette";
            this.Text = "Color Palette";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Image)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Color)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_ReadImage;
        private System.Windows.Forms.PictureBox pictureBox_Image;
        private System.Windows.Forms.Button button_ColorPalette;
        private System.Windows.Forms.Button button_ColorOutput;
        private System.Windows.Forms.DataGridView dataGridView_Color;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Color;
        private System.Windows.Forms.DataGridViewTextBoxColumn RGB;
    }
}

