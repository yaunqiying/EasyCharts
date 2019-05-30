namespace ExcelAddIn_Graphics
{
    partial class Form_Color_Matrix
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
            this.button_OK = new System.Windows.Forms.Button();
            this.label_Size = new System.Windows.Forms.Label();
            this.textBox_height = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(219, 46);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(55, 26);
            this.button_OK.TabIndex = 0;
            this.button_OK.Text = "确定";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // label_Size
            // 
            this.label_Size.AutoSize = true;
            this.label_Size.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Size.Location = new System.Drawing.Point(27, 49);
            this.label_Size.Name = "label_Size";
            this.label_Size.Size = new System.Drawing.Size(40, 16);
            this.label_Size.TabIndex = 1;
            this.label_Size.Text = "高度";
            // 
            // textBox_height
            // 
            this.textBox_height.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_height.Location = new System.Drawing.Point(85, 46);
            this.textBox_height.Name = "textBox_height";
            this.textBox_height.Size = new System.Drawing.Size(100, 26);
            this.textBox_height.TabIndex = 2;
            this.textBox_height.Text = "10";
            // 
            // Color_Matrix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 115);
            this.Controls.Add(this.textBox_height);
            this.Controls.Add(this.label_Size);
            this.Controls.Add(this.button_OK);
            this.Name = "Color_Matrix";
            this.Text = "Color_Matrix";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Label label_Size;
        private System.Windows.Forms.TextBox textBox_height;
    }
}