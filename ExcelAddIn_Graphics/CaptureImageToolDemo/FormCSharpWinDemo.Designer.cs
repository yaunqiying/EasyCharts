namespace CaptureImageToolDemo
{
    partial class FormCSharpWinDemo
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panelBottom = new System.Windows.Forms.Panel();
            this.checkBoxCursor = new System.Windows.Forms.CheckBox();
            this.checkBoxHide = new System.Windows.Forms.CheckBox();
            this.checkBoxColorTable = new System.Windows.Forms.CheckBox();
            this.buttonCaptureImage = new System.Windows.Forms.Button();
            this.buttonAbout = new System.Windows.Forms.Button();
            this.linkLabelCSharpWin = new System.Windows.Forms.LinkLabel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.panelBottom.SuspendLayout();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.checkBoxCursor);
            this.panelBottom.Controls.Add(this.checkBoxHide);
            this.panelBottom.Controls.Add(this.checkBoxColorTable);
            this.panelBottom.Controls.Add(this.buttonCaptureImage);
            this.panelBottom.Controls.Add(this.buttonAbout);
            this.panelBottom.Controls.Add(this.linkLabelCSharpWin);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 191);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(425, 50);
            this.panelBottom.TabIndex = 0;
            // 
            // checkBoxCursor
            // 
            this.checkBoxCursor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxCursor.AutoSize = true;
            this.checkBoxCursor.Location = new System.Drawing.Point(228, 6);
            this.checkBoxCursor.Name = "checkBoxCursor";
            this.checkBoxCursor.Size = new System.Drawing.Size(96, 16);
            this.checkBoxCursor.TabIndex = 20;
            this.checkBoxCursor.Text = "更换鼠标样式";
            this.checkBoxCursor.UseVisualStyleBackColor = true;
            // 
            // checkBoxHide
            // 
            this.checkBoxHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxHide.AutoSize = true;
            this.checkBoxHide.Checked = true;
            this.checkBoxHide.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHide.Location = new System.Drawing.Point(12, 6);
            this.checkBoxHide.Name = "checkBoxHide";
            this.checkBoxHide.Size = new System.Drawing.Size(108, 16);
            this.checkBoxHide.TabIndex = 19;
            this.checkBoxHide.Text = "截图时隐藏窗体";
            this.checkBoxHide.UseVisualStyleBackColor = true;
            // 
            // checkBoxColorTable
            // 
            this.checkBoxColorTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxColorTable.AutoSize = true;
            this.checkBoxColorTable.Location = new System.Drawing.Point(126, 6);
            this.checkBoxColorTable.Name = "checkBoxColorTable";
            this.checkBoxColorTable.Size = new System.Drawing.Size(96, 16);
            this.checkBoxColorTable.TabIndex = 18;
            this.checkBoxColorTable.Text = "更换颜色样式";
            this.checkBoxColorTable.UseVisualStyleBackColor = true;
            // 
            // buttonCaptureImage
            // 
            this.buttonCaptureImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCaptureImage.Location = new System.Drawing.Point(338, 2);
            this.buttonCaptureImage.Name = "buttonCaptureImage";
            this.buttonCaptureImage.Size = new System.Drawing.Size(75, 23);
            this.buttonCaptureImage.TabIndex = 17;
            this.buttonCaptureImage.Text = "截图";
            this.buttonCaptureImage.UseVisualStyleBackColor = true;
            // 
            // buttonAbout
            // 
            this.buttonAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAbout.FlatAppearance.BorderSize = 0;
            this.buttonAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAbout.Location = new System.Drawing.Point(338, 24);
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size(75, 23);
            this.buttonAbout.TabIndex = 16;
            this.buttonAbout.Text = "关于...";
            this.buttonAbout.UseVisualStyleBackColor = true;
            // 
            // linkLabelCSharpWin
            // 
            this.linkLabelCSharpWin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelCSharpWin.AutoSize = true;
            this.linkLabelCSharpWin.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabelCSharpWin.Location = new System.Drawing.Point(74, 27);
            this.linkLabelCSharpWin.Name = "linkLabelCSharpWin";
            this.linkLabelCSharpWin.Size = new System.Drawing.Size(258, 14);
            this.linkLabelCSharpWin.TabIndex = 15;
            this.linkLabelCSharpWin.TabStop = true;
            this.linkLabelCSharpWin.Text = "www.csharpwin.com(CS 程序员之窗)";
            // 
            // panelTop
            // 
            this.panelTop.AutoScroll = true;
            this.panelTop.Controls.Add(this.pictureBox);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(425, 191);
            this.panelTop.TabIndex = 1;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(422, 171);
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            // 
            // FormCSharpWinDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 241);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelBottom);
            this.Name = "FormCSharpWinDemo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "完整功能的截图控件";
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.LinkLabel linkLabelCSharpWin;
        private System.Windows.Forms.Button buttonAbout;
        private System.Windows.Forms.Button buttonCaptureImage;
        private System.Windows.Forms.CheckBox checkBoxColorTable;
        private System.Windows.Forms.CheckBox checkBoxHide;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.CheckBox checkBoxCursor;
    }
}

