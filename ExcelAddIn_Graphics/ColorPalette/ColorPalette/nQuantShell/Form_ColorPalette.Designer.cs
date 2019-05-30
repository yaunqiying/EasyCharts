namespace nQuant
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
            this.button_ReadImage = new System.Windows.Forms.Button();
            this.pictureBox_Image = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Image)).BeginInit();
            this.SuspendLayout();
            // 
            // button_ReadImage
            // 
            this.button_ReadImage.Location = new System.Drawing.Point(748, 476);
            this.button_ReadImage.Name = "button_ReadImage";
            this.button_ReadImage.Size = new System.Drawing.Size(160, 38);
            this.button_ReadImage.TabIndex = 3;
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
            this.pictureBox_Image.Size = new System.Drawing.Size(720, 547);
            this.pictureBox_Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Image.TabIndex = 2;
            this.pictureBox_Image.TabStop = false;
            // 
            // Form_ColorPalette
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 580);
            this.Controls.Add(this.button_ReadImage);
            this.Controls.Add(this.pictureBox_Image);
            this.Name = "Form_ColorPalette";
            this.Text = "Color Palette";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Image)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_ReadImage;
        private System.Windows.Forms.PictureBox pictureBox_Image;
    }
}