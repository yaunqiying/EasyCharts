namespace PdfToImage
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtSingleFile = new System.Windows.Forms.TextBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.txtDirectory = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnBrowse2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comboFormat = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numQuality = new System.Windows.Forms.NumericUpDown();
            this.checkFitTopage = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtExtension = new System.Windows.Forms.TextBox();
            this.radioSingleFile = new System.Windows.Forms.RadioButton();
            this.radioDirectory = new System.Windows.Forms.RadioButton();
            this.checkSingleFile = new System.Windows.Forms.CheckBox();
            this.fileSystemWatcher = new System.IO.FileSystemWatcher();
            this.lblDllInfo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtArguments = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numericFirstPage = new System.Windows.Forms.NumericUpDown();
            this.numericLastPage = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.pictureOutput = new System.Windows.Forms.PictureBox();
            this.checkRedirect = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numericThreads = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numericGraphSampling = new System.Windows.Forms.NumericUpDown();
            this.numericTextSampling = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numQuality)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFirstPage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLastPage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureOutput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericThreads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericGraphSampling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericTextSampling)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSingleFile
            // 
            this.txtSingleFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSingleFile.Location = new System.Drawing.Point(185, 11);
            this.txtSingleFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSingleFile.Name = "txtSingleFile";
            this.txtSingleFile.Size = new System.Drawing.Size(364, 22);
            this.txtSingleFile.TabIndex = 4;
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(185, 74);
            this.btnConvert.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(100, 28);
            this.btnConvert.TabIndex = 5;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.ConvertToImage);
            // 
            // txtDirectory
            // 
            this.txtDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDirectory.Location = new System.Drawing.Point(185, 42);
            this.txtDirectory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDirectory.Name = "txtDirectory";
            this.txtDirectory.Size = new System.Drawing.Size(364, 22);
            this.txtDirectory.TabIndex = 6;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(559, 9);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(33, 28);
            this.btnBrowse.TabIndex = 7;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.FindFile);
            // 
            // btnBrowse2
            // 
            this.btnBrowse2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse2.Location = new System.Drawing.Point(559, 39);
            this.btnBrowse2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowse2.Name = "btnBrowse2";
            this.btnBrowse2.Size = new System.Drawing.Size(33, 28);
            this.btnBrowse2.TabIndex = 8;
            this.btnBrowse2.Text = "...";
            this.btnBrowse2.UseVisualStyleBackColor = true;
            this.btnBrowse2.Click += new System.EventHandler(this.FindDirectory);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(63, 117);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Output format";
            // 
            // comboFormat
            // 
            this.comboFormat.FormattingEnabled = true;
            this.comboFormat.Items.AddRange(new object[] {
            "tifflzw",
            "jpeg",
            "pnggray",
            "png256",
            "png16",
            "png16m"});
            this.comboFormat.Location = new System.Drawing.Point(165, 113);
            this.comboFormat.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboFormat.Name = "comboFormat";
            this.comboFormat.Size = new System.Drawing.Size(160, 24);
            this.comboFormat.TabIndex = 10;
            this.comboFormat.Text = "tifflzw";
            this.comboFormat.SelectedIndexChanged += new System.EventHandler(this.FormatChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(335, 117);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(238, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "look on Ghostscript website for more";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(297, 153);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "Quality";
            // 
            // numQuality
            // 
            this.numQuality.Location = new System.Drawing.Point(357, 150);
            this.numQuality.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numQuality.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numQuality.Name = "numQuality";
            this.numQuality.Size = new System.Drawing.Size(60, 22);
            this.numQuality.TabIndex = 13;
            this.numQuality.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // checkFitTopage
            // 
            this.checkFitTopage.AutoSize = true;
            this.checkFitTopage.Location = new System.Drawing.Point(425, 156);
            this.checkFitTopage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkFitTopage.Name = "checkFitTopage";
            this.checkFitTopage.Size = new System.Drawing.Size(81, 21);
            this.checkFitTopage.TabIndex = 14;
            this.checkFitTopage.Text = "Fit page";
            this.checkFitTopage.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(44, 153);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(115, 17);
            this.label7.TabIndex = 15;
            this.label7.Text = "Output extension";
            // 
            // txtExtension
            // 
            this.txtExtension.Location = new System.Drawing.Point(165, 149);
            this.txtExtension.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtExtension.Name = "txtExtension";
            this.txtExtension.Size = new System.Drawing.Size(119, 22);
            this.txtExtension.TabIndex = 16;
            this.txtExtension.Text = ".tif";
            // 
            // radioSingleFile
            // 
            this.radioSingleFile.AutoSize = true;
            this.radioSingleFile.Checked = true;
            this.radioSingleFile.Location = new System.Drawing.Point(16, 12);
            this.radioSingleFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioSingleFile.Name = "radioSingleFile";
            this.radioSingleFile.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radioSingleFile.Size = new System.Drawing.Size(161, 21);
            this.radioSingleFile.TabIndex = 17;
            this.radioSingleFile.TabStop = true;
            this.radioSingleFile.Text = "Single File to convert";
            this.radioSingleFile.UseVisualStyleBackColor = true;
            // 
            // radioDirectory
            // 
            this.radioDirectory.AutoSize = true;
            this.radioDirectory.Location = new System.Drawing.Point(8, 43);
            this.radioDirectory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioDirectory.Name = "radioDirectory";
            this.radioDirectory.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radioDirectory.Size = new System.Drawing.Size(171, 21);
            this.radioDirectory.TabIndex = 18;
            this.radioDirectory.Text = "Directory to Monitorize";
            this.radioDirectory.UseVisualStyleBackColor = true;
            // 
            // checkSingleFile
            // 
            this.checkSingleFile.AutoSize = true;
            this.checkSingleFile.Location = new System.Drawing.Point(425, 137);
            this.checkSingleFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkSingleFile.Name = "checkSingleFile";
            this.checkSingleFile.Size = new System.Drawing.Size(137, 21);
            this.checkSingleFile.TabIndex = 19;
            this.checkSingleFile.Text = "1 Image for page";
            this.checkSingleFile.UseVisualStyleBackColor = true;
            // 
            // fileSystemWatcher
            // 
            this.fileSystemWatcher.EnableRaisingEvents = true;
            this.fileSystemWatcher.Filter = "*.pdf";
            this.fileSystemWatcher.SynchronizingObject = this;
            this.fileSystemWatcher.Created += new System.IO.FileSystemEventHandler(this.NewPDFCreated);
            // 
            // lblDllInfo
            // 
            this.lblDllInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDllInfo.Location = new System.Drawing.Point(13, 266);
            this.lblDllInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDllInfo.Name = "lblDllInfo";
            this.lblDllInfo.Size = new System.Drawing.Size(579, 65);
            this.lblDllInfo.TabIndex = 20;
            this.lblDllInfo.Text = "To made this program work you MUST copy gsdll32.dll from Ghostscript installation" +
                " in this program directory";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(293, 70);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 17);
            this.label2.TabIndex = 21;
            this.label2.Text = "Status:";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.Location = new System.Drawing.Point(293, 86);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(101, 17);
            this.lblInfo.TabIndex = 22;
            this.lblInfo.Text = "Place Holder";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 218);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 17);
            this.label1.TabIndex = 23;
            this.label1.Text = "Arguments Used";
            // 
            // txtArguments
            // 
            this.txtArguments.Location = new System.Drawing.Point(17, 238);
            this.txtArguments.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtArguments.Name = "txtArguments";
            this.txtArguments.ReadOnly = true;
            this.txtArguments.Size = new System.Drawing.Size(560, 22);
            this.txtArguments.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 180);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(130, 17);
            this.label8.TabIndex = 25;
            this.label8.Text = "1st page to convert";
            // 
            // numericFirstPage
            // 
            this.numericFirstPage.Location = new System.Drawing.Point(167, 177);
            this.numericFirstPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericFirstPage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericFirstPage.Name = "numericFirstPage";
            this.numericFirstPage.Size = new System.Drawing.Size(59, 22);
            this.numericFirstPage.TabIndex = 26;
            this.numericFirstPage.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericFirstPage.ValueChanged += new System.EventHandler(this.NumericValueChanged);
            // 
            // numericLastPage
            // 
            this.numericLastPage.Location = new System.Drawing.Point(273, 177);
            this.numericLastPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericLastPage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericLastPage.Name = "numericLastPage";
            this.numericLastPage.Size = new System.Drawing.Size(59, 22);
            this.numericLastPage.TabIndex = 27;
            this.numericLastPage.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericLastPage.ValueChanged += new System.EventHandler(this.NumericValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(233, 181);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 17);
            this.label9.TabIndex = 28;
            this.label9.Text = "last";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(12, 80);
            this.lblVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(56, 17);
            this.lblVersion.TabIndex = 29;
            this.lblVersion.Text = "Version";
            // 
            // pictureOutput
            // 
            this.pictureOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureOutput.Location = new System.Drawing.Point(609, 86);
            this.pictureOutput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureOutput.Name = "pictureOutput";
            this.pictureOutput.Size = new System.Drawing.Size(0, 230);
            this.pictureOutput.TabIndex = 30;
            this.pictureOutput.TabStop = false;
            // 
            // checkRedirect
            // 
            this.checkRedirect.AutoSize = true;
            this.checkRedirect.Checked = true;
            this.checkRedirect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkRedirect.Location = new System.Drawing.Point(425, 176);
            this.checkRedirect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkRedirect.Name = "checkRedirect";
            this.checkRedirect.Size = new System.Drawing.Size(153, 21);
            this.checkRedirect.TabIndex = 31;
            this.checkRedirect.Text = "Redirect to Memory";
            this.checkRedirect.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(161, 208);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 17);
            this.label3.TabIndex = 32;
            this.label3.Text = "N° Threads";
            // 
            // numericThreads
            // 
            this.numericThreads.Location = new System.Drawing.Point(251, 204);
            this.numericThreads.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericThreads.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericThreads.Name = "numericThreads";
            this.numericThreads.Size = new System.Drawing.Size(59, 22);
            this.numericThreads.TabIndex = 33;
            this.numericThreads.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(313, 208);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(110, 17);
            this.label10.TabIndex = 34;
            this.label10.Text = "Sampling Graph";
            // 
            // numericGraphSampling
            // 
            this.numericGraphSampling.Location = new System.Drawing.Point(427, 204);
            this.numericGraphSampling.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericGraphSampling.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericGraphSampling.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericGraphSampling.Name = "numericGraphSampling";
            this.numericGraphSampling.Size = new System.Drawing.Size(45, 22);
            this.numericGraphSampling.TabIndex = 35;
            this.numericGraphSampling.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // numericTextSampling
            // 
            this.numericTextSampling.Location = new System.Drawing.Point(527, 204);
            this.numericTextSampling.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericTextSampling.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericTextSampling.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numericTextSampling.Name = "numericTextSampling";
            this.numericTextSampling.Size = new System.Drawing.Size(45, 22);
            this.numericTextSampling.TabIndex = 36;
            this.numericTextSampling.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(487, 207);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(30, 17);
            this.label11.TabIndex = 37;
            this.label11.Text = "text";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 331);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.numericTextSampling);
            this.Controls.Add(this.numericGraphSampling);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.numericThreads);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkRedirect);
            this.Controls.Add(this.pictureOutput);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.numericLastPage);
            this.Controls.Add(this.numericFirstPage);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtArguments);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkSingleFile);
            this.Controls.Add(this.radioDirectory);
            this.Controls.Add(this.radioSingleFile);
            this.Controls.Add(this.txtExtension);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.checkFitTopage);
            this.Controls.Add(this.numQuality);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboFormat);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnBrowse2);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtDirectory);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.txtSingleFile);
            this.Controls.Add(this.lblDllInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "How to convert a PDF to image";
            ((System.ComponentModel.ISupportInitialize)(this.numQuality)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericFirstPage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLastPage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureOutput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericThreads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericGraphSampling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericTextSampling)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSingleFile;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.TextBox txtDirectory;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnBrowse2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboFormat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numQuality;
        private System.Windows.Forms.CheckBox checkFitTopage;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtExtension;
        private System.Windows.Forms.RadioButton radioSingleFile;
        private System.Windows.Forms.RadioButton radioDirectory;
        private System.Windows.Forms.CheckBox checkSingleFile;
        private System.IO.FileSystemWatcher fileSystemWatcher;
        private System.Windows.Forms.Label lblDllInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TextBox txtArguments;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericLastPage;
        private System.Windows.Forms.NumericUpDown numericFirstPage;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.CheckBox checkRedirect;
        private System.Windows.Forms.PictureBox pictureOutput;
        private System.Windows.Forms.NumericUpDown numericThreads;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numericTextSampling;
        private System.Windows.Forms.NumericUpDown numericGraphSampling;
        private System.Windows.Forms.Label label10;
    }
}

