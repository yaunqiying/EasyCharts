namespace ExcelAddIn_Graphics
{
    partial class Form_ColorPalette
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
            this.dataGridView_Color = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Color = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RGB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_ColorOutput = new System.Windows.Forms.Button();
            this.button_ColorPalette = new System.Windows.Forms.Button();
            this.button_ReadImage = new System.Windows.Forms.Button();
            this.pictureBox_Image = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Color)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Image)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_Color
            // 
            this.dataGridView_Color.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_Color.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Color.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Color,
            this.RGB});
            this.dataGridView_Color.Location = new System.Drawing.Point(718, 12);
            this.dataGridView_Color.Name = "dataGridView_Color";
            this.dataGridView_Color.RowHeadersVisible = false;
            this.dataGridView_Color.RowHeadersWidth = 5;
            this.dataGridView_Color.RowTemplate.Height = 23;
            this.dataGridView_Color.Size = new System.Drawing.Size(160, 401);
            this.dataGridView_Color.TabIndex = 13;
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
            // button_ColorOutput
            // 
            this.button_ColorOutput.Location = new System.Drawing.Point(718, 518);
            this.button_ColorOutput.Name = "button_ColorOutput";
            this.button_ColorOutput.Size = new System.Drawing.Size(160, 38);
            this.button_ColorOutput.TabIndex = 12;
            this.button_ColorOutput.Text = "数据导出";
            this.button_ColorOutput.UseVisualStyleBackColor = true;
            this.button_ColorOutput.Click += new System.EventHandler(this.button_ColorOutput_Click);
            // 
            // button_ColorPalette
            // 
            this.button_ColorPalette.Location = new System.Drawing.Point(718, 475);
            this.button_ColorPalette.Name = "button_ColorPalette";
            this.button_ColorPalette.Size = new System.Drawing.Size(160, 38);
            this.button_ColorPalette.TabIndex = 11;
            this.button_ColorPalette.Text = "颜色提取";
            this.button_ColorPalette.UseVisualStyleBackColor = true;
            this.button_ColorPalette.Click += new System.EventHandler(this.button_ColorPalette_Click);
            // 
            // button_ReadImage
            // 
            this.button_ReadImage.Location = new System.Drawing.Point(718, 431);
            this.button_ReadImage.Name = "button_ReadImage";
            this.button_ReadImage.Size = new System.Drawing.Size(160, 38);
            this.button_ReadImage.TabIndex = 10;
            this.button_ReadImage.Text = "导入图像";
            this.button_ReadImage.UseVisualStyleBackColor = true;
            this.button_ReadImage.Click += new System.EventHandler(this.button_ReadImage_Click);
            // 
            // pictureBox_Image
            // 
            this.pictureBox_Image.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox_Image.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_Image.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pictureBox_Image.Location = new System.Drawing.Point(12, 12);
            this.pictureBox_Image.Name = "pictureBox_Image";
            this.pictureBox_Image.Size = new System.Drawing.Size(689, 547);
            this.pictureBox_Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Image.TabIndex = 9;
            this.pictureBox_Image.TabStop = false;
            // 
            // Form_ColorPalette
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 577);
            this.Controls.Add(this.dataGridView_Color);
            this.Controls.Add(this.button_ColorOutput);
            this.Controls.Add(this.button_ColorPalette);
            this.Controls.Add(this.button_ReadImage);
            this.Controls.Add(this.pictureBox_Image);
            this.Name = "Form_ColorPalette";
            this.Text = "图片颜色方案提取";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Color)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Image)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_Color;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Color;
        private System.Windows.Forms.DataGridViewTextBoxColumn RGB;
        private System.Windows.Forms.Button button_ColorOutput;
        private System.Windows.Forms.Button button_ColorPalette;
        private System.Windows.Forms.Button button_ReadImage;
        private System.Windows.Forms.PictureBox pictureBox_Image;
    }
}