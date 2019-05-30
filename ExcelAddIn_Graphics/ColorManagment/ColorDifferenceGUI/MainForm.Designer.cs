namespace ColorDifferenceGUI
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
            this.SettingsBox = new System.Windows.Forms.GroupBox();
            this.FromColorValueBox = new System.Windows.Forms.TextBox();
            this.FromColorChannelUpDo = new System.Windows.Forms.NumericUpDown();
            this.ColorFromDroDo = new System.Windows.Forms.ComboBox();
            this.ColorToDroDo = new System.Windows.Forms.ComboBox();
            this.ICCYCbCrToBox = new System.Windows.Forms.TextBox();
            this.ICCboxTo = new System.Windows.Forms.TextBox();
            this.YCbCrSpaceFromDroDo = new System.Windows.Forms.ComboBox();
            this.CompareButton = new System.Windows.Forms.Button();
            this.ColorspaceFromDroDo = new System.Windows.Forms.ComboBox();
            this.ICCYCbCrFromBox = new System.Windows.Forms.TextBox();
            this.ICCboxFrom = new System.Windows.Forms.TextBox();
            this.RefWhiteFromDroDo = new System.Windows.Forms.ComboBox();
            this.GenYCbCrDroDo = new System.Windows.Forms.ComboBox();
            this.GenColorspaceDroDo = new System.Windows.Forms.ComboBox();
            this.RenderIntentDroDo = new System.Windows.Forms.ComboBox();
            this.ChromAdaptDroDo = new System.Windows.Forms.ComboBox();
            this.RefWhiteToDroDo = new System.Windows.Forms.ComboBox();
            this.YCbCrSpaceToDroDo = new System.Windows.Forms.ComboBox();
            this.ColorspaceToDroDo = new System.Windows.Forms.ComboBox();
            this.ToColorChannelUpDo = new System.Windows.Forms.NumericUpDown();
            this.ToColorValueBox = new System.Windows.Forms.TextBox();
            this.ResultBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.CIEDE2000_E = new System.Windows.Forms.TextBox();
            this.CIEDE2000_H = new System.Windows.Forms.TextBox();
            this.CIEDE2000_C = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.CIE94t_E = new System.Windows.Forms.TextBox();
            this.CIE94t_H = new System.Windows.Forms.TextBox();
            this.CIE94t_C = new System.Windows.Forms.TextBox();
            this.CIE94g_E = new System.Windows.Forms.TextBox();
            this.CIE94g_H = new System.Windows.Forms.TextBox();
            this.CIE94g_C = new System.Windows.Forms.TextBox();
            this.CIE76_E = new System.Windows.Forms.TextBox();
            this.CIE76_H = new System.Windows.Forms.TextBox();
            this.CIE76_C = new System.Windows.Forms.TextBox();
            this.DIN99d_E = new System.Windows.Forms.TextBox();
            this.DIN99d_H = new System.Windows.Forms.TextBox();
            this.DIN99d_C = new System.Windows.Forms.TextBox();
            this.DIN99c_E = new System.Windows.Forms.TextBox();
            this.DIN99c_H = new System.Windows.Forms.TextBox();
            this.DIN99c_C = new System.Windows.Forms.TextBox();
            this.DIN99b_E = new System.Windows.Forms.TextBox();
            this.DIN99_E = new System.Windows.Forms.TextBox();
            this.DIN99b_H = new System.Windows.Forms.TextBox();
            this.DIN99b_C = new System.Windows.Forms.TextBox();
            this.DIN99_H = new System.Windows.Forms.TextBox();
            this.DIN99_C = new System.Windows.Forms.TextBox();
            this.CMC21_E = new System.Windows.Forms.TextBox();
            this.CMC21_H = new System.Windows.Forms.TextBox();
            this.CMC11_E = new System.Windows.Forms.TextBox();
            this.CMC21_C = new System.Windows.Forms.TextBox();
            this.CMC11_H = new System.Windows.Forms.TextBox();
            this.CMC11_C = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.iccOpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.SettingsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FromColorChannelUpDo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToColorChannelUpDo)).BeginInit();
            this.ResultBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SettingsBox
            // 
            this.SettingsBox.Controls.Add(this.ToColorValueBox);
            this.SettingsBox.Controls.Add(this.ToColorChannelUpDo);
            this.SettingsBox.Controls.Add(this.FromColorValueBox);
            this.SettingsBox.Controls.Add(this.FromColorChannelUpDo);
            this.SettingsBox.Controls.Add(this.ColorFromDroDo);
            this.SettingsBox.Controls.Add(this.ColorToDroDo);
            this.SettingsBox.Controls.Add(this.ICCYCbCrToBox);
            this.SettingsBox.Controls.Add(this.ICCboxTo);
            this.SettingsBox.Controls.Add(this.YCbCrSpaceFromDroDo);
            this.SettingsBox.Controls.Add(this.CompareButton);
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
            this.SettingsBox.Size = new System.Drawing.Size(431, 234);
            this.SettingsBox.TabIndex = 6;
            this.SettingsBox.TabStop = false;
            this.SettingsBox.Text = "Settings";
            // 
            // FromColorValueBox
            // 
            this.FromColorValueBox.Location = new System.Drawing.Point(6, 178);
            this.FromColorValueBox.Name = "FromColorValueBox";
            this.FromColorValueBox.Size = new System.Drawing.Size(128, 20);
            this.FromColorValueBox.TabIndex = 8;
            this.FromColorValueBox.TextChanged += new System.EventHandler(this.FromColorValueBox_TextChanged);
            // 
            // FromColorChannelUpDo
            // 
            this.FromColorChannelUpDo.Location = new System.Drawing.Point(6, 204);
            this.FromColorChannelUpDo.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.FromColorChannelUpDo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FromColorChannelUpDo.Name = "FromColorChannelUpDo";
            this.FromColorChannelUpDo.Size = new System.Drawing.Size(128, 20);
            this.FromColorChannelUpDo.TabIndex = 7;
            this.FromColorChannelUpDo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FromColorChannelUpDo.ValueChanged += new System.EventHandler(this.FromColorChannelUpDo_ValueChanged);
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
            // ICCYCbCrToBox
            // 
            this.ICCYCbCrToBox.Location = new System.Drawing.Point(151, 100);
            this.ICCYCbCrToBox.Name = "ICCYCbCrToBox";
            this.ICCYCbCrToBox.ReadOnly = true;
            this.ICCYCbCrToBox.Size = new System.Drawing.Size(128, 20);
            this.ICCYCbCrToBox.TabIndex = 3;
            // 
            // ICCboxTo
            // 
            this.ICCboxTo.Location = new System.Drawing.Point(151, 153);
            this.ICCboxTo.Name = "ICCboxTo";
            this.ICCboxTo.ReadOnly = true;
            this.ICCboxTo.Size = new System.Drawing.Size(128, 20);
            this.ICCboxTo.TabIndex = 3;
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
            // CompareButton
            // 
            this.CompareButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CompareButton.Location = new System.Drawing.Point(296, 179);
            this.CompareButton.Name = "CompareButton";
            this.CompareButton.Size = new System.Drawing.Size(128, 45);
            this.CompareButton.TabIndex = 6;
            this.CompareButton.Text = "Compare";
            this.CompareButton.UseVisualStyleBackColor = true;
            this.CompareButton.Click += new System.EventHandler(this.CompareButton_Click);
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
            // ICCYCbCrFromBox
            // 
            this.ICCYCbCrFromBox.Location = new System.Drawing.Point(6, 100);
            this.ICCYCbCrFromBox.Name = "ICCYCbCrFromBox";
            this.ICCYCbCrFromBox.ReadOnly = true;
            this.ICCYCbCrFromBox.Size = new System.Drawing.Size(128, 20);
            this.ICCYCbCrFromBox.TabIndex = 3;
            // 
            // ICCboxFrom
            // 
            this.ICCboxFrom.Location = new System.Drawing.Point(6, 153);
            this.ICCboxFrom.Name = "ICCboxFrom";
            this.ICCboxFrom.ReadOnly = true;
            this.ICCboxFrom.Size = new System.Drawing.Size(128, 20);
            this.ICCboxFrom.TabIndex = 3;
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
            // GenYCbCrDroDo
            // 
            this.GenYCbCrDroDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GenYCbCrDroDo.FormattingEnabled = true;
            this.GenYCbCrDroDo.Location = new System.Drawing.Point(296, 99);
            this.GenYCbCrDroDo.Name = "GenYCbCrDroDo";
            this.GenYCbCrDroDo.Size = new System.Drawing.Size(128, 21);
            this.GenYCbCrDroDo.TabIndex = 2;
            this.GenYCbCrDroDo.SelectedIndexChanged += new System.EventHandler(this.GenYCbCrDroDo_SelectedIndexChanged);
            // 
            // GenColorspaceDroDo
            // 
            this.GenColorspaceDroDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GenColorspaceDroDo.FormattingEnabled = true;
            this.GenColorspaceDroDo.Location = new System.Drawing.Point(296, 73);
            this.GenColorspaceDroDo.Name = "GenColorspaceDroDo";
            this.GenColorspaceDroDo.Size = new System.Drawing.Size(128, 21);
            this.GenColorspaceDroDo.TabIndex = 2;
            this.GenColorspaceDroDo.SelectedIndexChanged += new System.EventHandler(this.GenColorspaceDroDo_SelectedIndexChanged);
            // 
            // RenderIntentDroDo
            // 
            this.RenderIntentDroDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RenderIntentDroDo.FormattingEnabled = true;
            this.RenderIntentDroDo.Location = new System.Drawing.Point(296, 46);
            this.RenderIntentDroDo.Name = "RenderIntentDroDo";
            this.RenderIntentDroDo.Size = new System.Drawing.Size(128, 21);
            this.RenderIntentDroDo.TabIndex = 2;
            this.RenderIntentDroDo.SelectedIndexChanged += new System.EventHandler(this.RenderIntentDroDo_SelectedIndexChanged);
            // 
            // ChromAdaptDroDo
            // 
            this.ChromAdaptDroDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChromAdaptDroDo.FormattingEnabled = true;
            this.ChromAdaptDroDo.Location = new System.Drawing.Point(296, 19);
            this.ChromAdaptDroDo.Name = "ChromAdaptDroDo";
            this.ChromAdaptDroDo.Size = new System.Drawing.Size(128, 21);
            this.ChromAdaptDroDo.TabIndex = 2;
            this.ChromAdaptDroDo.SelectedIndexChanged += new System.EventHandler(this.ChromAdaptDroDo_SelectedIndexChanged);
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
            // ToColorChannelUpDo
            // 
            this.ToColorChannelUpDo.Location = new System.Drawing.Point(151, 204);
            this.ToColorChannelUpDo.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.ToColorChannelUpDo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ToColorChannelUpDo.Name = "ToColorChannelUpDo";
            this.ToColorChannelUpDo.Size = new System.Drawing.Size(128, 20);
            this.ToColorChannelUpDo.TabIndex = 7;
            this.ToColorChannelUpDo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ToColorChannelUpDo.ValueChanged += new System.EventHandler(this.ToColorChannelUpDo_ValueChanged);
            // 
            // ToColorValueBox
            // 
            this.ToColorValueBox.Location = new System.Drawing.Point(151, 178);
            this.ToColorValueBox.Name = "ToColorValueBox";
            this.ToColorValueBox.Size = new System.Drawing.Size(128, 20);
            this.ToColorValueBox.TabIndex = 8;
            this.ToColorValueBox.TextChanged += new System.EventHandler(this.ToColorValueBox_TextChanged);
            // 
            // ResultBox
            // 
            this.ResultBox.Controls.Add(this.DIN99_C);
            this.ResultBox.Controls.Add(this.DIN99_H);
            this.ResultBox.Controls.Add(this.DIN99c_C);
            this.ResultBox.Controls.Add(this.DIN99b_C);
            this.ResultBox.Controls.Add(this.DIN99c_H);
            this.ResultBox.Controls.Add(this.DIN99b_H);
            this.ResultBox.Controls.Add(this.DIN99d_C);
            this.ResultBox.Controls.Add(this.DIN99d_H);
            this.ResultBox.Controls.Add(this.CIE76_C);
            this.ResultBox.Controls.Add(this.CIE76_H);
            this.ResultBox.Controls.Add(this.DIN99_E);
            this.ResultBox.Controls.Add(this.CIE94g_C);
            this.ResultBox.Controls.Add(this.DIN99c_E);
            this.ResultBox.Controls.Add(this.DIN99b_E);
            this.ResultBox.Controls.Add(this.CIE94g_H);
            this.ResultBox.Controls.Add(this.DIN99d_E);
            this.ResultBox.Controls.Add(this.CMC11_C);
            this.ResultBox.Controls.Add(this.CIE94t_C);
            this.ResultBox.Controls.Add(this.CIE76_E);
            this.ResultBox.Controls.Add(this.CMC11_H);
            this.ResultBox.Controls.Add(this.CIE94t_H);
            this.ResultBox.Controls.Add(this.CIE94g_E);
            this.ResultBox.Controls.Add(this.CMC21_C);
            this.ResultBox.Controls.Add(this.CMC11_E);
            this.ResultBox.Controls.Add(this.CIEDE2000_C);
            this.ResultBox.Controls.Add(this.CMC21_H);
            this.ResultBox.Controls.Add(this.CIE94t_E);
            this.ResultBox.Controls.Add(this.CMC21_E);
            this.ResultBox.Controls.Add(this.CIEDE2000_H);
            this.ResultBox.Controls.Add(this.CIEDE2000_E);
            this.ResultBox.Controls.Add(this.label10);
            this.ResultBox.Controls.Add(this.label8);
            this.ResultBox.Controls.Add(this.label7);
            this.ResultBox.Controls.Add(this.label9);
            this.ResultBox.Controls.Add(this.label6);
            this.ResultBox.Controls.Add(this.label5);
            this.ResultBox.Controls.Add(this.label4);
            this.ResultBox.Controls.Add(this.label3);
            this.ResultBox.Controls.Add(this.label2);
            this.ResultBox.Controls.Add(this.label13);
            this.ResultBox.Controls.Add(this.label12);
            this.ResultBox.Controls.Add(this.label11);
            this.ResultBox.Controls.Add(this.label1);
            this.ResultBox.Location = new System.Drawing.Point(12, 252);
            this.ResultBox.Name = "ResultBox";
            this.ResultBox.Size = new System.Drawing.Size(431, 303);
            this.ResultBox.TabIndex = 7;
            this.ResultBox.TabStop = false;
            this.ResultBox.Text = "Results";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "DIN99:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "DIN99b:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "DIN99c:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "DIN99d:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "CIE76:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 170);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 16);
            this.label6.TabIndex = 0;
            this.label6.Text = "CIE94 (graphics):";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 222);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 16);
            this.label7.TabIndex = 0;
            this.label7.Text = "CIEDE2000:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 248);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 16);
            this.label8.TabIndex = 0;
            this.label8.Text = "CMC 1:1:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(6, 196);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 16);
            this.label9.TabIndex = 0;
            this.label9.Text = "CIE94 (textiles):";
            // 
            // CIEDE2000_E
            // 
            this.CIEDE2000_E.Location = new System.Drawing.Point(124, 221);
            this.CIEDE2000_E.Name = "CIEDE2000_E";
            this.CIEDE2000_E.ReadOnly = true;
            this.CIEDE2000_E.Size = new System.Drawing.Size(96, 20);
            this.CIEDE2000_E.TabIndex = 1;
            // 
            // CIEDE2000_H
            // 
            this.CIEDE2000_H.Location = new System.Drawing.Point(226, 221);
            this.CIEDE2000_H.Name = "CIEDE2000_H";
            this.CIEDE2000_H.ReadOnly = true;
            this.CIEDE2000_H.Size = new System.Drawing.Size(96, 20);
            this.CIEDE2000_H.TabIndex = 1;
            // 
            // CIEDE2000_C
            // 
            this.CIEDE2000_C.Location = new System.Drawing.Point(328, 221);
            this.CIEDE2000_C.Name = "CIEDE2000_C";
            this.CIEDE2000_C.ReadOnly = true;
            this.CIEDE2000_C.Size = new System.Drawing.Size(96, 20);
            this.CIEDE2000_C.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(6, 274);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 16);
            this.label10.TabIndex = 0;
            this.label10.Text = "CMC 2:1:";
            // 
            // CIE94t_E
            // 
            this.CIE94t_E.Location = new System.Drawing.Point(124, 195);
            this.CIE94t_E.Name = "CIE94t_E";
            this.CIE94t_E.ReadOnly = true;
            this.CIE94t_E.Size = new System.Drawing.Size(96, 20);
            this.CIE94t_E.TabIndex = 1;
            // 
            // CIE94t_H
            // 
            this.CIE94t_H.Location = new System.Drawing.Point(226, 195);
            this.CIE94t_H.Name = "CIE94t_H";
            this.CIE94t_H.ReadOnly = true;
            this.CIE94t_H.Size = new System.Drawing.Size(96, 20);
            this.CIE94t_H.TabIndex = 1;
            // 
            // CIE94t_C
            // 
            this.CIE94t_C.Location = new System.Drawing.Point(328, 195);
            this.CIE94t_C.Name = "CIE94t_C";
            this.CIE94t_C.ReadOnly = true;
            this.CIE94t_C.Size = new System.Drawing.Size(96, 20);
            this.CIE94t_C.TabIndex = 1;
            // 
            // CIE94g_E
            // 
            this.CIE94g_E.Location = new System.Drawing.Point(124, 169);
            this.CIE94g_E.Name = "CIE94g_E";
            this.CIE94g_E.ReadOnly = true;
            this.CIE94g_E.Size = new System.Drawing.Size(96, 20);
            this.CIE94g_E.TabIndex = 1;
            // 
            // CIE94g_H
            // 
            this.CIE94g_H.Location = new System.Drawing.Point(226, 169);
            this.CIE94g_H.Name = "CIE94g_H";
            this.CIE94g_H.ReadOnly = true;
            this.CIE94g_H.Size = new System.Drawing.Size(96, 20);
            this.CIE94g_H.TabIndex = 1;
            // 
            // CIE94g_C
            // 
            this.CIE94g_C.Location = new System.Drawing.Point(328, 169);
            this.CIE94g_C.Name = "CIE94g_C";
            this.CIE94g_C.ReadOnly = true;
            this.CIE94g_C.Size = new System.Drawing.Size(96, 20);
            this.CIE94g_C.TabIndex = 1;
            // 
            // CIE76_E
            // 
            this.CIE76_E.Location = new System.Drawing.Point(124, 143);
            this.CIE76_E.Name = "CIE76_E";
            this.CIE76_E.ReadOnly = true;
            this.CIE76_E.Size = new System.Drawing.Size(96, 20);
            this.CIE76_E.TabIndex = 1;
            // 
            // CIE76_H
            // 
            this.CIE76_H.Location = new System.Drawing.Point(226, 143);
            this.CIE76_H.Name = "CIE76_H";
            this.CIE76_H.ReadOnly = true;
            this.CIE76_H.Size = new System.Drawing.Size(96, 20);
            this.CIE76_H.TabIndex = 1;
            // 
            // CIE76_C
            // 
            this.CIE76_C.Location = new System.Drawing.Point(328, 143);
            this.CIE76_C.Name = "CIE76_C";
            this.CIE76_C.ReadOnly = true;
            this.CIE76_C.Size = new System.Drawing.Size(96, 20);
            this.CIE76_C.TabIndex = 1;
            // 
            // DIN99d_E
            // 
            this.DIN99d_E.Location = new System.Drawing.Point(124, 117);
            this.DIN99d_E.Name = "DIN99d_E";
            this.DIN99d_E.ReadOnly = true;
            this.DIN99d_E.Size = new System.Drawing.Size(96, 20);
            this.DIN99d_E.TabIndex = 1;
            // 
            // DIN99d_H
            // 
            this.DIN99d_H.Location = new System.Drawing.Point(226, 117);
            this.DIN99d_H.Name = "DIN99d_H";
            this.DIN99d_H.ReadOnly = true;
            this.DIN99d_H.Size = new System.Drawing.Size(96, 20);
            this.DIN99d_H.TabIndex = 1;
            // 
            // DIN99d_C
            // 
            this.DIN99d_C.Location = new System.Drawing.Point(328, 117);
            this.DIN99d_C.Name = "DIN99d_C";
            this.DIN99d_C.ReadOnly = true;
            this.DIN99d_C.Size = new System.Drawing.Size(96, 20);
            this.DIN99d_C.TabIndex = 1;
            // 
            // DIN99c_E
            // 
            this.DIN99c_E.Location = new System.Drawing.Point(125, 91);
            this.DIN99c_E.Name = "DIN99c_E";
            this.DIN99c_E.ReadOnly = true;
            this.DIN99c_E.Size = new System.Drawing.Size(96, 20);
            this.DIN99c_E.TabIndex = 1;
            // 
            // DIN99c_H
            // 
            this.DIN99c_H.Location = new System.Drawing.Point(227, 91);
            this.DIN99c_H.Name = "DIN99c_H";
            this.DIN99c_H.ReadOnly = true;
            this.DIN99c_H.Size = new System.Drawing.Size(96, 20);
            this.DIN99c_H.TabIndex = 1;
            // 
            // DIN99c_C
            // 
            this.DIN99c_C.Location = new System.Drawing.Point(329, 91);
            this.DIN99c_C.Name = "DIN99c_C";
            this.DIN99c_C.ReadOnly = true;
            this.DIN99c_C.Size = new System.Drawing.Size(96, 20);
            this.DIN99c_C.TabIndex = 1;
            // 
            // DIN99b_E
            // 
            this.DIN99b_E.Location = new System.Drawing.Point(124, 65);
            this.DIN99b_E.Name = "DIN99b_E";
            this.DIN99b_E.ReadOnly = true;
            this.DIN99b_E.Size = new System.Drawing.Size(96, 20);
            this.DIN99b_E.TabIndex = 1;
            // 
            // DIN99_E
            // 
            this.DIN99_E.Location = new System.Drawing.Point(125, 39);
            this.DIN99_E.Name = "DIN99_E";
            this.DIN99_E.ReadOnly = true;
            this.DIN99_E.Size = new System.Drawing.Size(96, 20);
            this.DIN99_E.TabIndex = 1;
            // 
            // DIN99b_H
            // 
            this.DIN99b_H.Location = new System.Drawing.Point(226, 65);
            this.DIN99b_H.Name = "DIN99b_H";
            this.DIN99b_H.ReadOnly = true;
            this.DIN99b_H.Size = new System.Drawing.Size(96, 20);
            this.DIN99b_H.TabIndex = 1;
            // 
            // DIN99b_C
            // 
            this.DIN99b_C.Location = new System.Drawing.Point(328, 65);
            this.DIN99b_C.Name = "DIN99b_C";
            this.DIN99b_C.ReadOnly = true;
            this.DIN99b_C.Size = new System.Drawing.Size(96, 20);
            this.DIN99b_C.TabIndex = 1;
            // 
            // DIN99_H
            // 
            this.DIN99_H.Location = new System.Drawing.Point(227, 39);
            this.DIN99_H.Name = "DIN99_H";
            this.DIN99_H.ReadOnly = true;
            this.DIN99_H.Size = new System.Drawing.Size(96, 20);
            this.DIN99_H.TabIndex = 1;
            // 
            // DIN99_C
            // 
            this.DIN99_C.Location = new System.Drawing.Point(329, 39);
            this.DIN99_C.Name = "DIN99_C";
            this.DIN99_C.ReadOnly = true;
            this.DIN99_C.Size = new System.Drawing.Size(96, 20);
            this.DIN99_C.TabIndex = 1;
            // 
            // CMC21_E
            // 
            this.CMC21_E.Location = new System.Drawing.Point(125, 273);
            this.CMC21_E.Name = "CMC21_E";
            this.CMC21_E.ReadOnly = true;
            this.CMC21_E.Size = new System.Drawing.Size(96, 20);
            this.CMC21_E.TabIndex = 1;
            // 
            // CMC21_H
            // 
            this.CMC21_H.Location = new System.Drawing.Point(227, 273);
            this.CMC21_H.Name = "CMC21_H";
            this.CMC21_H.ReadOnly = true;
            this.CMC21_H.Size = new System.Drawing.Size(96, 20);
            this.CMC21_H.TabIndex = 1;
            // 
            // CMC11_E
            // 
            this.CMC11_E.Location = new System.Drawing.Point(125, 247);
            this.CMC11_E.Name = "CMC11_E";
            this.CMC11_E.ReadOnly = true;
            this.CMC11_E.Size = new System.Drawing.Size(96, 20);
            this.CMC11_E.TabIndex = 1;
            // 
            // CMC21_C
            // 
            this.CMC21_C.Location = new System.Drawing.Point(329, 273);
            this.CMC21_C.Name = "CMC21_C";
            this.CMC21_C.ReadOnly = true;
            this.CMC21_C.Size = new System.Drawing.Size(96, 20);
            this.CMC21_C.TabIndex = 1;
            // 
            // CMC11_H
            // 
            this.CMC11_H.Location = new System.Drawing.Point(227, 247);
            this.CMC11_H.Name = "CMC11_H";
            this.CMC11_H.ReadOnly = true;
            this.CMC11_H.Size = new System.Drawing.Size(96, 20);
            this.CMC11_H.TabIndex = 1;
            // 
            // CMC11_C
            // 
            this.CMC11_C.Location = new System.Drawing.Point(329, 247);
            this.CMC11_C.Name = "CMC11_C";
            this.CMC11_C.ReadOnly = true;
            this.CMC11_C.Size = new System.Drawing.Size(96, 20);
            this.CMC11_C.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(122, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 16);
            this.label11.TabIndex = 0;
            this.label11.Text = "Delta E:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(223, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 16);
            this.label12.TabIndex = 0;
            this.label12.Text = "Delta H:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(325, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(55, 16);
            this.label13.TabIndex = 0;
            this.label13.Text = "Delta C:";
            // 
            // iccOpenDialog
            // 
            this.iccOpenDialog.DefaultExt = "*.icc";
            this.iccOpenDialog.FileName = "ICC_Profile.icc";
            this.iccOpenDialog.Filter = "ICC-Profile | *.icc";
            this.iccOpenDialog.Title = "Select ICC Profile";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 565);
            this.Controls.Add(this.ResultBox);
            this.Controls.Add(this.SettingsBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Color Difference";
            this.SettingsBox.ResumeLayout(false);
            this.SettingsBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FromColorChannelUpDo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToColorChannelUpDo)).EndInit();
            this.ResultBox.ResumeLayout(false);
            this.ResultBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox SettingsBox;
        private System.Windows.Forms.TextBox ToColorValueBox;
        private System.Windows.Forms.NumericUpDown ToColorChannelUpDo;
        private System.Windows.Forms.TextBox FromColorValueBox;
        private System.Windows.Forms.NumericUpDown FromColorChannelUpDo;
        private System.Windows.Forms.ComboBox ColorFromDroDo;
        private System.Windows.Forms.ComboBox ColorToDroDo;
        private System.Windows.Forms.TextBox ICCYCbCrToBox;
        private System.Windows.Forms.TextBox ICCboxTo;
        private System.Windows.Forms.ComboBox YCbCrSpaceFromDroDo;
        private System.Windows.Forms.Button CompareButton;
        private System.Windows.Forms.ComboBox ColorspaceFromDroDo;
        private System.Windows.Forms.TextBox ICCYCbCrFromBox;
        private System.Windows.Forms.TextBox ICCboxFrom;
        private System.Windows.Forms.ComboBox RefWhiteFromDroDo;
        private System.Windows.Forms.ComboBox GenYCbCrDroDo;
        private System.Windows.Forms.ComboBox GenColorspaceDroDo;
        private System.Windows.Forms.ComboBox RenderIntentDroDo;
        private System.Windows.Forms.ComboBox ChromAdaptDroDo;
        private System.Windows.Forms.ComboBox RefWhiteToDroDo;
        private System.Windows.Forms.ComboBox YCbCrSpaceToDroDo;
        private System.Windows.Forms.ComboBox ColorspaceToDroDo;
        private System.Windows.Forms.GroupBox ResultBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox DIN99_C;
        private System.Windows.Forms.TextBox DIN99_H;
        private System.Windows.Forms.TextBox DIN99c_C;
        private System.Windows.Forms.TextBox DIN99b_C;
        private System.Windows.Forms.TextBox DIN99c_H;
        private System.Windows.Forms.TextBox DIN99b_H;
        private System.Windows.Forms.TextBox DIN99d_C;
        private System.Windows.Forms.TextBox DIN99d_H;
        private System.Windows.Forms.TextBox CIE76_C;
        private System.Windows.Forms.TextBox CIE76_H;
        private System.Windows.Forms.TextBox DIN99_E;
        private System.Windows.Forms.TextBox CIE94g_C;
        private System.Windows.Forms.TextBox DIN99c_E;
        private System.Windows.Forms.TextBox DIN99b_E;
        private System.Windows.Forms.TextBox CIE94g_H;
        private System.Windows.Forms.TextBox DIN99d_E;
        private System.Windows.Forms.TextBox CMC11_C;
        private System.Windows.Forms.TextBox CIE94t_C;
        private System.Windows.Forms.TextBox CIE76_E;
        private System.Windows.Forms.TextBox CMC11_H;
        private System.Windows.Forms.TextBox CIE94t_H;
        private System.Windows.Forms.TextBox CIE94g_E;
        private System.Windows.Forms.TextBox CMC21_C;
        private System.Windows.Forms.TextBox CMC11_E;
        private System.Windows.Forms.TextBox CIEDE2000_C;
        private System.Windows.Forms.TextBox CMC21_H;
        private System.Windows.Forms.TextBox CIE94t_E;
        private System.Windows.Forms.TextBox CMC21_E;
        private System.Windows.Forms.TextBox CIEDE2000_H;
        private System.Windows.Forms.TextBox CIEDE2000_E;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.OpenFileDialog iccOpenDialog;
    }
}

