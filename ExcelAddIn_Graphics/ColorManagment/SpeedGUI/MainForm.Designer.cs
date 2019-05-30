namespace SpeedGUI
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ColorFromDroDo = new System.Windows.Forms.ComboBox();
            this.ColorToDroDo = new System.Windows.Forms.ComboBox();
            this.IterationUpDo = new System.Windows.Forms.NumericUpDown();
            this.ColorspaceFromDroDo = new System.Windows.Forms.ComboBox();
            this.ColorspaceToDroDo = new System.Windows.Forms.ComboBox();
            this.ICCboxFrom = new System.Windows.Forms.TextBox();
            this.ICCboxTo = new System.Windows.Forms.TextBox();
            this.RefWhiteFromDroDo = new System.Windows.Forms.ComboBox();
            this.RefWhiteToDroDo = new System.Windows.Forms.ComboBox();
            this.ChromAdaptDroDo = new System.Windows.Forms.ComboBox();
            this.RenderIntentDroDo = new System.Windows.Forms.ComboBox();
            this.MainWorker = new System.ComponentModel.BackgroundWorker();
            this.ResultBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TimeIterationLabel = new System.Windows.Forms.Label();
            this.TimeTotalLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.StartStopButton = new System.Windows.Forms.Button();
            this.MainProgressBar = new System.Windows.Forms.ProgressBar();
            this.SettingsBox = new System.Windows.Forms.GroupBox();
            this.AutoThreadChBox = new System.Windows.Forms.CheckBox();
            this.ThreadUpDo = new System.Windows.Forms.NumericUpDown();
            this.ICCYCbCrToBox = new System.Windows.Forms.TextBox();
            this.YCbCrSpaceFromDroDo = new System.Windows.Forms.ComboBox();
            this.ICCYCbCrFromBox = new System.Windows.Forms.TextBox();
            this.GenYCbCrDroDo = new System.Windows.Forms.ComboBox();
            this.GenColorspaceDroDo = new System.Windows.Forms.ComboBox();
            this.YCbCrSpaceToDroDo = new System.Windows.Forms.ComboBox();
            this.iccOpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.FastChBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.IterationUpDo)).BeginInit();
            this.ResultBox.SuspendLayout();
            this.SettingsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadUpDo)).BeginInit();
            this.SuspendLayout();
            // 
            // ColorFromDroDo
            // 
            this.ColorFromDroDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ColorFromDroDo.FormattingEnabled = true;
            this.ColorFromDroDo.Location = new System.Drawing.Point(6, 19);
            this.ColorFromDroDo.Name = "ColorFromDroDo";
            this.ColorFromDroDo.Size = new System.Drawing.Size(128, 21);
            this.ColorFromDroDo.TabIndex = 0;
            this.ColorFromDroDo.SelectedIndexChanged += new System.EventHandler(this.ColorFromDroDo_SelectedIndexChanged);
            // 
            // ColorToDroDo
            // 
            this.ColorToDroDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ColorToDroDo.FormattingEnabled = true;
            this.ColorToDroDo.Location = new System.Drawing.Point(151, 19);
            this.ColorToDroDo.Name = "ColorToDroDo";
            this.ColorToDroDo.Size = new System.Drawing.Size(128, 21);
            this.ColorToDroDo.TabIndex = 0;
            this.ColorToDroDo.SelectedIndexChanged += new System.EventHandler(this.ColorToDroDo_SelectedIndexChanged);
            // 
            // IterationUpDo
            // 
            this.IterationUpDo.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.IterationUpDo.Location = new System.Drawing.Point(296, 20);
            this.IterationUpDo.Maximum = new decimal(new int[] {
            1316134912,
            2328,
            0,
            0});
            this.IterationUpDo.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.IterationUpDo.Name = "IterationUpDo";
            this.IterationUpDo.Size = new System.Drawing.Size(128, 20);
            this.IterationUpDo.TabIndex = 1;
            this.IterationUpDo.ThousandsSeparator = true;
            this.IterationUpDo.Value = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            // 
            // ColorspaceFromDroDo
            // 
            this.ColorspaceFromDroDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ColorspaceFromDroDo.FormattingEnabled = true;
            this.ColorspaceFromDroDo.Location = new System.Drawing.Point(6, 126);
            this.ColorspaceFromDroDo.Name = "ColorspaceFromDroDo";
            this.ColorspaceFromDroDo.Size = new System.Drawing.Size(128, 21);
            this.ColorspaceFromDroDo.TabIndex = 2;
            this.ColorspaceFromDroDo.SelectedIndexChanged += new System.EventHandler(this.ColorspaceFromDroDo_SelectedIndexChanged);
            // 
            // ColorspaceToDroDo
            // 
            this.ColorspaceToDroDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ColorspaceToDroDo.FormattingEnabled = true;
            this.ColorspaceToDroDo.Location = new System.Drawing.Point(151, 126);
            this.ColorspaceToDroDo.Name = "ColorspaceToDroDo";
            this.ColorspaceToDroDo.Size = new System.Drawing.Size(128, 21);
            this.ColorspaceToDroDo.TabIndex = 2;
            this.ColorspaceToDroDo.SelectedIndexChanged += new System.EventHandler(this.ColorspaceToDroDo_SelectedIndexChanged);
            // 
            // ICCboxFrom
            // 
            this.ICCboxFrom.Location = new System.Drawing.Point(6, 153);
            this.ICCboxFrom.Name = "ICCboxFrom";
            this.ICCboxFrom.ReadOnly = true;
            this.ICCboxFrom.Size = new System.Drawing.Size(128, 20);
            this.ICCboxFrom.TabIndex = 3;
            // 
            // ICCboxTo
            // 
            this.ICCboxTo.Location = new System.Drawing.Point(151, 153);
            this.ICCboxTo.Name = "ICCboxTo";
            this.ICCboxTo.ReadOnly = true;
            this.ICCboxTo.Size = new System.Drawing.Size(128, 20);
            this.ICCboxTo.TabIndex = 3;
            // 
            // RefWhiteFromDroDo
            // 
            this.RefWhiteFromDroDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RefWhiteFromDroDo.FormattingEnabled = true;
            this.RefWhiteFromDroDo.Location = new System.Drawing.Point(6, 46);
            this.RefWhiteFromDroDo.Name = "RefWhiteFromDroDo";
            this.RefWhiteFromDroDo.Size = new System.Drawing.Size(128, 21);
            this.RefWhiteFromDroDo.TabIndex = 2;
            // 
            // RefWhiteToDroDo
            // 
            this.RefWhiteToDroDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RefWhiteToDroDo.FormattingEnabled = true;
            this.RefWhiteToDroDo.Location = new System.Drawing.Point(151, 46);
            this.RefWhiteToDroDo.Name = "RefWhiteToDroDo";
            this.RefWhiteToDroDo.Size = new System.Drawing.Size(128, 21);
            this.RefWhiteToDroDo.TabIndex = 2;
            // 
            // ChromAdaptDroDo
            // 
            this.ChromAdaptDroDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChromAdaptDroDo.FormattingEnabled = true;
            this.ChromAdaptDroDo.Location = new System.Drawing.Point(296, 46);
            this.ChromAdaptDroDo.Name = "ChromAdaptDroDo";
            this.ChromAdaptDroDo.Size = new System.Drawing.Size(128, 21);
            this.ChromAdaptDroDo.TabIndex = 2;
            this.ChromAdaptDroDo.SelectedIndexChanged += new System.EventHandler(this.ChromAdaptDroDo_SelectedIndexChanged);
            // 
            // RenderIntentDroDo
            // 
            this.RenderIntentDroDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RenderIntentDroDo.FormattingEnabled = true;
            this.RenderIntentDroDo.Location = new System.Drawing.Point(296, 73);
            this.RenderIntentDroDo.Name = "RenderIntentDroDo";
            this.RenderIntentDroDo.Size = new System.Drawing.Size(128, 21);
            this.RenderIntentDroDo.TabIndex = 2;
            this.RenderIntentDroDo.SelectedIndexChanged += new System.EventHandler(this.RenderIntentDroDo_SelectedIndexChanged);
            // 
            // MainWorker
            // 
            this.MainWorker.WorkerReportsProgress = true;
            this.MainWorker.WorkerSupportsCancellation = true;
            this.MainWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.MainWorker_DoWork);
            this.MainWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.MainWorker_ProgressChanged);
            this.MainWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.MainWorker_RunWorkerCompleted);
            // 
            // ResultBox
            // 
            this.ResultBox.Controls.Add(this.label2);
            this.ResultBox.Controls.Add(this.TimeIterationLabel);
            this.ResultBox.Controls.Add(this.TimeTotalLabel);
            this.ResultBox.Controls.Add(this.label1);
            this.ResultBox.Controls.Add(this.StartStopButton);
            this.ResultBox.Controls.Add(this.MainProgressBar);
            this.ResultBox.Location = new System.Drawing.Point(12, 220);
            this.ResultBox.Name = "ResultBox";
            this.ResultBox.Size = new System.Drawing.Size(431, 111);
            this.ResultBox.TabIndex = 4;
            this.ResultBox.TabStop = false;
            this.ResultBox.Text = "Results";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Time per iteration:";
            // 
            // TimeIterationLabel
            // 
            this.TimeIterationLabel.AutoSize = true;
            this.TimeIterationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimeIterationLabel.Location = new System.Drawing.Point(131, 51);
            this.TimeIterationLabel.Name = "TimeIterationLabel";
            this.TimeIterationLabel.Size = new System.Drawing.Size(29, 16);
            this.TimeIterationLabel.TabIndex = 7;
            this.TimeIterationLabel.Text = "0µs";
            // 
            // TimeTotalLabel
            // 
            this.TimeTotalLabel.AutoSize = true;
            this.TimeTotalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimeTotalLabel.Location = new System.Drawing.Point(131, 27);
            this.TimeTotalLabel.Name = "TimeTotalLabel";
            this.TimeTotalLabel.Size = new System.Drawing.Size(33, 16);
            this.TimeTotalLabel.TabIndex = 7;
            this.TimeTotalLabel.Text = "0ms";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "Time total:";
            // 
            // StartStopButton
            // 
            this.StartStopButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartStopButton.Location = new System.Drawing.Point(6, 79);
            this.StartStopButton.Name = "StartStopButton";
            this.StartStopButton.Size = new System.Drawing.Size(75, 23);
            this.StartStopButton.TabIndex = 6;
            this.StartStopButton.Text = "Start";
            this.StartStopButton.UseVisualStyleBackColor = true;
            this.StartStopButton.Click += new System.EventHandler(this.StartStopButton_Click);
            // 
            // MainProgressBar
            // 
            this.MainProgressBar.Location = new System.Drawing.Point(87, 79);
            this.MainProgressBar.Name = "MainProgressBar";
            this.MainProgressBar.Size = new System.Drawing.Size(337, 23);
            this.MainProgressBar.TabIndex = 5;
            // 
            // SettingsBox
            // 
            this.SettingsBox.Controls.Add(this.FastChBox);
            this.SettingsBox.Controls.Add(this.AutoThreadChBox);
            this.SettingsBox.Controls.Add(this.ThreadUpDo);
            this.SettingsBox.Controls.Add(this.ColorFromDroDo);
            this.SettingsBox.Controls.Add(this.ColorToDroDo);
            this.SettingsBox.Controls.Add(this.IterationUpDo);
            this.SettingsBox.Controls.Add(this.ICCYCbCrToBox);
            this.SettingsBox.Controls.Add(this.ICCboxTo);
            this.SettingsBox.Controls.Add(this.YCbCrSpaceFromDroDo);
            this.SettingsBox.Controls.Add(this.ColorspaceFromDroDo);
            this.SettingsBox.Controls.Add(this.ICCYCbCrFromBox);
            this.SettingsBox.Controls.Add(this.ICCboxFrom);
            this.SettingsBox.Controls.Add(this.RefWhiteFromDroDo);
            this.SettingsBox.Controls.Add(this.GenYCbCrDroDo);
            this.SettingsBox.Controls.Add(this.GenColorspaceDroDo);
            this.SettingsBox.Controls.Add(this.RenderIntentDroDo);
            this.SettingsBox.Controls.Add(this.ChromAdaptDroDo);
            this.SettingsBox.Controls.Add(this.RefWhiteToDroDo);
            this.SettingsBox.Controls.Add(this.YCbCrSpaceToDroDo);
            this.SettingsBox.Controls.Add(this.ColorspaceToDroDo);
            this.SettingsBox.Location = new System.Drawing.Point(12, 12);
            this.SettingsBox.Name = "SettingsBox";
            this.SettingsBox.Size = new System.Drawing.Size(431, 202);
            this.SettingsBox.TabIndex = 4;
            this.SettingsBox.TabStop = false;
            this.SettingsBox.Text = "Settings";
            // 
            // AutoThreadChBox
            // 
            this.AutoThreadChBox.AutoSize = true;
            this.AutoThreadChBox.Location = new System.Drawing.Point(296, 155);
            this.AutoThreadChBox.Name = "AutoThreadChBox";
            this.AutoThreadChBox.Size = new System.Drawing.Size(83, 17);
            this.AutoThreadChBox.TabIndex = 5;
            this.AutoThreadChBox.Text = "Autothreads";
            this.AutoThreadChBox.UseVisualStyleBackColor = true;
            this.AutoThreadChBox.CheckedChanged += new System.EventHandler(this.AutoThreadChBox_CheckedChanged);
            // 
            // ThreadUpDo
            // 
            this.ThreadUpDo.Location = new System.Drawing.Point(381, 154);
            this.ThreadUpDo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ThreadUpDo.Name = "ThreadUpDo";
            this.ThreadUpDo.Size = new System.Drawing.Size(43, 20);
            this.ThreadUpDo.TabIndex = 4;
            this.ThreadUpDo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ThreadUpDo.ValueChanged += new System.EventHandler(this.ThreadUpDo_ValueChanged);
            // 
            // ICCYCbCrToBox
            // 
            this.ICCYCbCrToBox.Location = new System.Drawing.Point(151, 100);
            this.ICCYCbCrToBox.Name = "ICCYCbCrToBox";
            this.ICCYCbCrToBox.ReadOnly = true;
            this.ICCYCbCrToBox.Size = new System.Drawing.Size(128, 20);
            this.ICCYCbCrToBox.TabIndex = 3;
            // 
            // YCbCrSpaceFromDroDo
            // 
            this.YCbCrSpaceFromDroDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.YCbCrSpaceFromDroDo.FormattingEnabled = true;
            this.YCbCrSpaceFromDroDo.Location = new System.Drawing.Point(6, 73);
            this.YCbCrSpaceFromDroDo.Name = "YCbCrSpaceFromDroDo";
            this.YCbCrSpaceFromDroDo.Size = new System.Drawing.Size(128, 21);
            this.YCbCrSpaceFromDroDo.TabIndex = 2;
            this.YCbCrSpaceFromDroDo.SelectedIndexChanged += new System.EventHandler(this.YCbCrSpaceFromDroDo_SelectedIndexChanged);
            // 
            // ICCYCbCrFromBox
            // 
            this.ICCYCbCrFromBox.Location = new System.Drawing.Point(6, 100);
            this.ICCYCbCrFromBox.Name = "ICCYCbCrFromBox";
            this.ICCYCbCrFromBox.ReadOnly = true;
            this.ICCYCbCrFromBox.Size = new System.Drawing.Size(128, 20);
            this.ICCYCbCrFromBox.TabIndex = 3;
            // 
            // GenYCbCrDroDo
            // 
            this.GenYCbCrDroDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GenYCbCrDroDo.FormattingEnabled = true;
            this.GenYCbCrDroDo.Location = new System.Drawing.Point(296, 126);
            this.GenYCbCrDroDo.Name = "GenYCbCrDroDo";
            this.GenYCbCrDroDo.Size = new System.Drawing.Size(128, 21);
            this.GenYCbCrDroDo.TabIndex = 2;
            this.GenYCbCrDroDo.SelectedIndexChanged += new System.EventHandler(this.GenYCbCrDroDo_SelectedIndexChanged);
            // 
            // GenColorspaceDroDo
            // 
            this.GenColorspaceDroDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GenColorspaceDroDo.FormattingEnabled = true;
            this.GenColorspaceDroDo.Location = new System.Drawing.Point(296, 100);
            this.GenColorspaceDroDo.Name = "GenColorspaceDroDo";
            this.GenColorspaceDroDo.Size = new System.Drawing.Size(128, 21);
            this.GenColorspaceDroDo.TabIndex = 2;
            this.GenColorspaceDroDo.SelectedIndexChanged += new System.EventHandler(this.GenColorspaceDroDo_SelectedIndexChanged);
            // 
            // YCbCrSpaceToDroDo
            // 
            this.YCbCrSpaceToDroDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.YCbCrSpaceToDroDo.FormattingEnabled = true;
            this.YCbCrSpaceToDroDo.Location = new System.Drawing.Point(151, 73);
            this.YCbCrSpaceToDroDo.Name = "YCbCrSpaceToDroDo";
            this.YCbCrSpaceToDroDo.Size = new System.Drawing.Size(128, 21);
            this.YCbCrSpaceToDroDo.TabIndex = 2;
            this.YCbCrSpaceToDroDo.SelectedIndexChanged += new System.EventHandler(this.YCbCrSpaceToDroDo_SelectedIndexChanged);
            // 
            // iccOpenDialog
            // 
            this.iccOpenDialog.DefaultExt = "*.icc";
            this.iccOpenDialog.FileName = "ICC_Profile.icc";
            this.iccOpenDialog.Filter = "ICC-Profile | *.icc";
            this.iccOpenDialog.Title = "Select ICC Profile";
            // 
            // FastChBox
            // 
            this.FastChBox.AutoSize = true;
            this.FastChBox.Location = new System.Drawing.Point(296, 178);
            this.FastChBox.Name = "FastChBox";
            this.FastChBox.Size = new System.Drawing.Size(46, 17);
            this.FastChBox.TabIndex = 6;
            this.FastChBox.Text = "Fast";
            this.FastChBox.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 340);
            this.Controls.Add(this.SettingsBox);
            this.Controls.Add(this.ResultBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Color Conversion Speed";
            ((System.ComponentModel.ISupportInitialize)(this.IterationUpDo)).EndInit();
            this.ResultBox.ResumeLayout(false);
            this.ResultBox.PerformLayout();
            this.SettingsBox.ResumeLayout(false);
            this.SettingsBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadUpDo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox ColorFromDroDo;
        private System.Windows.Forms.ComboBox ColorToDroDo;
        private System.Windows.Forms.NumericUpDown IterationUpDo;
        private System.Windows.Forms.ComboBox ColorspaceFromDroDo;
        private System.Windows.Forms.ComboBox ColorspaceToDroDo;
        private System.Windows.Forms.TextBox ICCboxFrom;
        private System.Windows.Forms.TextBox ICCboxTo;
        private System.Windows.Forms.ComboBox RefWhiteFromDroDo;
        private System.Windows.Forms.ComboBox RefWhiteToDroDo;
        private System.Windows.Forms.ComboBox ChromAdaptDroDo;
        private System.Windows.Forms.ComboBox RenderIntentDroDo;
        private System.ComponentModel.BackgroundWorker MainWorker;
        private System.Windows.Forms.GroupBox ResultBox;
        private System.Windows.Forms.Button StartStopButton;
        private System.Windows.Forms.ProgressBar MainProgressBar;
        private System.Windows.Forms.GroupBox SettingsBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label TimeIterationLabel;
        private System.Windows.Forms.Label TimeTotalLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox YCbCrSpaceFromDroDo;
        private System.Windows.Forms.ComboBox YCbCrSpaceToDroDo;
        private System.Windows.Forms.OpenFileDialog iccOpenDialog;
        private System.Windows.Forms.TextBox ICCYCbCrToBox;
        private System.Windows.Forms.TextBox ICCYCbCrFromBox;
        private System.Windows.Forms.ComboBox GenYCbCrDroDo;
        private System.Windows.Forms.ComboBox GenColorspaceDroDo;
        private System.Windows.Forms.CheckBox AutoThreadChBox;
        private System.Windows.Forms.NumericUpDown ThreadUpDo;
        private System.Windows.Forms.CheckBox FastChBox;
    }
}

