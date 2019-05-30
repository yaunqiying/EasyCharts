namespace ConversionGUI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.RGB_R = new System.Windows.Forms.TextBox();
            this.RGB_G = new System.Windows.Forms.TextBox();
            this.RGB_B = new System.Windows.Forms.TextBox();
            this.Lab_L = new System.Windows.Forms.TextBox();
            this.Lab_a = new System.Windows.Forms.TextBox();
            this.Lab_b = new System.Windows.Forms.TextBox();
            this.XYZ_X = new System.Windows.Forms.TextBox();
            this.XYZ_Y = new System.Windows.Forms.TextBox();
            this.XYZ_Z = new System.Windows.Forms.TextBox();
            this.LCHab_L = new System.Windows.Forms.TextBox();
            this.LCHab_C = new System.Windows.Forms.TextBox();
            this.LCHab_H = new System.Windows.Forms.TextBox();
            this.LCHuv_L = new System.Windows.Forms.TextBox();
            this.LCHuv_C = new System.Windows.Forms.TextBox();
            this.LCHuv_H = new System.Windows.Forms.TextBox();
            this.Luv_L = new System.Windows.Forms.TextBox();
            this.Luv_u = new System.Windows.Forms.TextBox();
            this.Luv_v = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.RGB_Button = new System.Windows.Forms.Button();
            this.Lab_Button = new System.Windows.Forms.Button();
            this.XYZ_Button = new System.Windows.Forms.Button();
            this.LCHab_Button = new System.Windows.Forms.Button();
            this.LCHuv_Button = new System.Windows.Forms.Button();
            this.Luv_Button = new System.Windows.Forms.Button();
            this.General_SpaceDrDo = new System.Windows.Forms.ComboBox();
            this.ClearButton = new System.Windows.Forms.Button();
            this.Yxy_Y = new System.Windows.Forms.TextBox();
            this.Yxy_x = new System.Windows.Forms.TextBox();
            this.Yxy_sy = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Yxy_Button = new System.Windows.Forms.Button();
            this.HSV_H = new System.Windows.Forms.TextBox();
            this.HSV_S = new System.Windows.Forms.TextBox();
            this.HSV_V = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.HSV_Button = new System.Windows.Forms.Button();
            this.HSL_H = new System.Windows.Forms.TextBox();
            this.HSL_S = new System.Windows.Forms.TextBox();
            this.HSL_L = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.HSL_Button = new System.Windows.Forms.Button();
            this.ColorPanel = new System.Windows.Forms.Panel();
            this.RGB_Label = new System.Windows.Forms.Label();
            this.Hex_Label = new System.Windows.Forms.Label();
            this.RefWhiteDrDo = new System.Windows.Forms.ComboBox();
            this.ColorSelectDialog = new System.Windows.Forms.ColorDialog();
            this.iccOpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.CMY_C = new System.Windows.Forms.TextBox();
            this.CMY_M = new System.Windows.Forms.TextBox();
            this.CMY_Y = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.CMY_Button = new System.Windows.Forms.Button();
            this.CMYK_C = new System.Windows.Forms.TextBox();
            this.CMYK_M = new System.Windows.Forms.TextBox();
            this.CMYK_Y = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.CMYK_Button = new System.Windows.Forms.Button();
            this.CMYK_K = new System.Windows.Forms.TextBox();
            this.CMY_ICCbox = new System.Windows.Forms.TextBox();
            this.CMYK_ICCbox = new System.Windows.Forms.TextBox();
            this.YCbCr_Y = new System.Windows.Forms.TextBox();
            this.YCbCr_Cb = new System.Windows.Forms.TextBox();
            this.YCbCr_Cr = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.YCbCr_Button = new System.Windows.Forms.Button();
            this.YCbCr_ICCbox = new System.Windows.Forms.TextBox();
            this.Gray_G = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.Gray_Button = new System.Windows.Forms.Button();
            this.Gray_ICCbox = new System.Windows.Forms.TextBox();
            this.XColor_X = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.XColor_Button = new System.Windows.Forms.Button();
            this.XColor_ICCbox = new System.Windows.Forms.TextBox();
            this.XColor_ChannelUpDo = new System.Windows.Forms.NumericUpDown();
            this.HSV_CoBox = new System.Windows.Forms.ComboBox();
            this.HSV_ICCbox = new System.Windows.Forms.TextBox();
            this.HSL_CoBox = new System.Windows.Forms.ComboBox();
            this.HSL_ICCbox = new System.Windows.Forms.TextBox();
            this.CMY_ChICC = new System.Windows.Forms.Button();
            this.CMYK_ChICC = new System.Windows.Forms.Button();
            this.Gray_ChICC = new System.Windows.Forms.Button();
            this.ColorX_ChICC = new System.Windows.Forms.Button();
            this.Lab_CoBox = new System.Windows.Forms.ComboBox();
            this.LCHab_CoBox = new System.Windows.Forms.ComboBox();
            this.LCHuv_CoBox = new System.Windows.Forms.ComboBox();
            this.Luv_CoBox = new System.Windows.Forms.ComboBox();
            this.Yxy_CoBox = new System.Windows.Forms.ComboBox();
            this.XYZ_CoBox = new System.Windows.Forms.ComboBox();
            this.RGB_CoBox = new System.Windows.Forms.ComboBox();
            this.RGB_ICCbox = new System.Windows.Forms.TextBox();
            this.XColor_Channel = new System.Windows.Forms.TextBox();
            this.YCbCr_CoBox = new System.Windows.Forms.ComboBox();
            this.RenderIntentCoBox = new System.Windows.Forms.ComboBox();
            this.ChroAdaptCoBox = new System.Windows.Forms.ComboBox();
            this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.LCH99_L = new System.Windows.Forms.TextBox();
            this.LCH99_C = new System.Windows.Forms.TextBox();
            this.LCH99_H = new System.Windows.Forms.TextBox();
            this.LCH99_Button = new System.Windows.Forms.Button();
            this.LCH99b_L = new System.Windows.Forms.TextBox();
            this.LCH99b_C = new System.Windows.Forms.TextBox();
            this.LCH99b_H = new System.Windows.Forms.TextBox();
            this.LCH99b_Button = new System.Windows.Forms.Button();
            this.LCH99c_L = new System.Windows.Forms.TextBox();
            this.LCH99c_C = new System.Windows.Forms.TextBox();
            this.LCH99c_H = new System.Windows.Forms.TextBox();
            this.LCH99c_Button = new System.Windows.Forms.Button();
            this.LCH99d_L = new System.Windows.Forms.TextBox();
            this.LCH99d_C = new System.Windows.Forms.TextBox();
            this.LCH99d_H = new System.Windows.Forms.TextBox();
            this.LCH99d_Button = new System.Windows.Forms.Button();
            this.DescriptionBox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.DEF_D = new System.Windows.Forms.TextBox();
            this.DEF_E = new System.Windows.Forms.TextBox();
            this.DEF_F = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.DEF_Button = new System.Windows.Forms.Button();
            this.Bef_B = new System.Windows.Forms.TextBox();
            this.Bef_e = new System.Windows.Forms.TextBox();
            this.Bef_f = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.Bef_Button = new System.Windows.Forms.Button();
            this.BCH_B = new System.Windows.Forms.TextBox();
            this.BCH_C = new System.Windows.Forms.TextBox();
            this.BCH_H = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.BCH_Button = new System.Windows.Forms.Button();
            this.DEF_CoBox = new System.Windows.Forms.ComboBox();
            this.Bef_CoBox = new System.Windows.Forms.ComboBox();
            this.BCH_CoBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.XColor_ChannelUpDo)).BeginInit();
            this.SuspendLayout();
            // 
            // RGB_R
            // 
            this.RGB_R.Location = new System.Drawing.Point(74, 273);
            this.RGB_R.Name = "RGB_R";
            this.RGB_R.Size = new System.Drawing.Size(86, 20);
            this.RGB_R.TabIndex = 0;
            this.MainToolTip.SetToolTip(this.RGB_R, "R-Channel");
            // 
            // RGB_G
            // 
            this.RGB_G.Location = new System.Drawing.Point(166, 273);
            this.RGB_G.Name = "RGB_G";
            this.RGB_G.Size = new System.Drawing.Size(86, 20);
            this.RGB_G.TabIndex = 1;
            this.MainToolTip.SetToolTip(this.RGB_G, "G-Channel");
            // 
            // RGB_B
            // 
            this.RGB_B.Location = new System.Drawing.Point(258, 273);
            this.RGB_B.Name = "RGB_B";
            this.RGB_B.Size = new System.Drawing.Size(86, 20);
            this.RGB_B.TabIndex = 2;
            this.MainToolTip.SetToolTip(this.RGB_B, "B-Channel");
            // 
            // Lab_L
            // 
            this.Lab_L.Location = new System.Drawing.Point(74, 65);
            this.Lab_L.Name = "Lab_L";
            this.Lab_L.Size = new System.Drawing.Size(86, 20);
            this.Lab_L.TabIndex = 4;
            this.MainToolTip.SetToolTip(this.Lab_L, "L-Channel");
            // 
            // Lab_a
            // 
            this.Lab_a.Location = new System.Drawing.Point(166, 65);
            this.Lab_a.Name = "Lab_a";
            this.Lab_a.Size = new System.Drawing.Size(86, 20);
            this.Lab_a.TabIndex = 5;
            this.MainToolTip.SetToolTip(this.Lab_a, "a-Channel");
            // 
            // Lab_b
            // 
            this.Lab_b.Location = new System.Drawing.Point(258, 65);
            this.Lab_b.Name = "Lab_b";
            this.Lab_b.Size = new System.Drawing.Size(86, 20);
            this.Lab_b.TabIndex = 6;
            this.MainToolTip.SetToolTip(this.Lab_b, "b-Channel");
            // 
            // XYZ_X
            // 
            this.XYZ_X.Location = new System.Drawing.Point(74, 13);
            this.XYZ_X.Name = "XYZ_X";
            this.XYZ_X.Size = new System.Drawing.Size(86, 20);
            this.XYZ_X.TabIndex = 8;
            this.MainToolTip.SetToolTip(this.XYZ_X, "X-Channel");
            // 
            // XYZ_Y
            // 
            this.XYZ_Y.Location = new System.Drawing.Point(166, 13);
            this.XYZ_Y.Name = "XYZ_Y";
            this.XYZ_Y.Size = new System.Drawing.Size(86, 20);
            this.XYZ_Y.TabIndex = 9;
            this.MainToolTip.SetToolTip(this.XYZ_Y, "Y-Channel");
            // 
            // XYZ_Z
            // 
            this.XYZ_Z.Location = new System.Drawing.Point(258, 13);
            this.XYZ_Z.Name = "XYZ_Z";
            this.XYZ_Z.Size = new System.Drawing.Size(86, 20);
            this.XYZ_Z.TabIndex = 10;
            this.MainToolTip.SetToolTip(this.XYZ_Z, "Z-Channel");
            // 
            // LCHab_L
            // 
            this.LCHab_L.Location = new System.Drawing.Point(74, 117);
            this.LCHab_L.Name = "LCHab_L";
            this.LCHab_L.Size = new System.Drawing.Size(86, 20);
            this.LCHab_L.TabIndex = 12;
            this.MainToolTip.SetToolTip(this.LCHab_L, "L-Channel");
            // 
            // LCHab_C
            // 
            this.LCHab_C.Location = new System.Drawing.Point(166, 117);
            this.LCHab_C.Name = "LCHab_C";
            this.LCHab_C.Size = new System.Drawing.Size(86, 20);
            this.LCHab_C.TabIndex = 13;
            this.MainToolTip.SetToolTip(this.LCHab_C, "C-Channel");
            // 
            // LCHab_H
            // 
            this.LCHab_H.Location = new System.Drawing.Point(258, 117);
            this.LCHab_H.Name = "LCHab_H";
            this.LCHab_H.Size = new System.Drawing.Size(86, 20);
            this.LCHab_H.TabIndex = 14;
            this.MainToolTip.SetToolTip(this.LCHab_H, "H-Channel");
            // 
            // LCHuv_L
            // 
            this.LCHuv_L.Location = new System.Drawing.Point(74, 143);
            this.LCHuv_L.Name = "LCHuv_L";
            this.LCHuv_L.Size = new System.Drawing.Size(86, 20);
            this.LCHuv_L.TabIndex = 16;
            this.MainToolTip.SetToolTip(this.LCHuv_L, "L-Channel");
            // 
            // LCHuv_C
            // 
            this.LCHuv_C.Location = new System.Drawing.Point(166, 143);
            this.LCHuv_C.Name = "LCHuv_C";
            this.LCHuv_C.Size = new System.Drawing.Size(86, 20);
            this.LCHuv_C.TabIndex = 17;
            this.MainToolTip.SetToolTip(this.LCHuv_C, "C-Channel");
            // 
            // LCHuv_H
            // 
            this.LCHuv_H.Location = new System.Drawing.Point(258, 143);
            this.LCHuv_H.Name = "LCHuv_H";
            this.LCHuv_H.Size = new System.Drawing.Size(86, 20);
            this.LCHuv_H.TabIndex = 18;
            this.MainToolTip.SetToolTip(this.LCHuv_H, "H-Channel");
            // 
            // Luv_L
            // 
            this.Luv_L.Location = new System.Drawing.Point(74, 91);
            this.Luv_L.Name = "Luv_L";
            this.Luv_L.Size = new System.Drawing.Size(86, 20);
            this.Luv_L.TabIndex = 20;
            this.MainToolTip.SetToolTip(this.Luv_L, "L-Channel");
            // 
            // Luv_u
            // 
            this.Luv_u.Location = new System.Drawing.Point(166, 91);
            this.Luv_u.Name = "Luv_u";
            this.Luv_u.Size = new System.Drawing.Size(86, 20);
            this.Luv_u.TabIndex = 21;
            this.MainToolTip.SetToolTip(this.Luv_u, "u-Channel");
            // 
            // Luv_v
            // 
            this.Luv_v.Location = new System.Drawing.Point(258, 91);
            this.Luv_v.Name = "Luv_v";
            this.Luv_v.Size = new System.Drawing.Size(86, 20);
            this.Luv_v.TabIndex = 22;
            this.MainToolTip.SetToolTip(this.Luv_v, "v-Channel");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(31, 275);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "RGB";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(37, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Lab";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(35, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "XYZ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(18, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "LCHab";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(20, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 16);
            this.label5.TabIndex = 1;
            this.label5.Text = "LCHuv";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(39, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 16);
            this.label6.TabIndex = 1;
            this.label6.Text = "Luv";
            // 
            // RGB_Button
            // 
            this.RGB_Button.Location = new System.Drawing.Point(355, 273);
            this.RGB_Button.Name = "RGB_Button";
            this.RGB_Button.Size = new System.Drawing.Size(56, 20);
            this.RGB_Button.TabIndex = 3;
            this.RGB_Button.Text = "Convert";
            this.MainToolTip.SetToolTip(this.RGB_Button, "Convert this color to all other colors");
            this.RGB_Button.UseVisualStyleBackColor = true;
            this.RGB_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // Lab_Button
            // 
            this.Lab_Button.Location = new System.Drawing.Point(355, 65);
            this.Lab_Button.Name = "Lab_Button";
            this.Lab_Button.Size = new System.Drawing.Size(56, 20);
            this.Lab_Button.TabIndex = 7;
            this.Lab_Button.Text = "Convert";
            this.MainToolTip.SetToolTip(this.Lab_Button, "Convert this color to all other colors");
            this.Lab_Button.UseVisualStyleBackColor = true;
            this.Lab_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // XYZ_Button
            // 
            this.XYZ_Button.Location = new System.Drawing.Point(355, 13);
            this.XYZ_Button.Name = "XYZ_Button";
            this.XYZ_Button.Size = new System.Drawing.Size(56, 20);
            this.XYZ_Button.TabIndex = 11;
            this.XYZ_Button.Text = "Convert";
            this.MainToolTip.SetToolTip(this.XYZ_Button, "Convert this color to all other colors");
            this.XYZ_Button.UseVisualStyleBackColor = true;
            this.XYZ_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // LCHab_Button
            // 
            this.LCHab_Button.Location = new System.Drawing.Point(355, 117);
            this.LCHab_Button.Name = "LCHab_Button";
            this.LCHab_Button.Size = new System.Drawing.Size(56, 20);
            this.LCHab_Button.TabIndex = 15;
            this.LCHab_Button.Text = "Convert";
            this.MainToolTip.SetToolTip(this.LCHab_Button, "Convert this color to all other colors");
            this.LCHab_Button.UseVisualStyleBackColor = true;
            this.LCHab_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // LCHuv_Button
            // 
            this.LCHuv_Button.Location = new System.Drawing.Point(355, 143);
            this.LCHuv_Button.Name = "LCHuv_Button";
            this.LCHuv_Button.Size = new System.Drawing.Size(56, 20);
            this.LCHuv_Button.TabIndex = 19;
            this.LCHuv_Button.Text = "Convert";
            this.MainToolTip.SetToolTip(this.LCHuv_Button, "Convert this color to all other colors");
            this.LCHuv_Button.UseVisualStyleBackColor = true;
            this.LCHuv_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // Luv_Button
            // 
            this.Luv_Button.Location = new System.Drawing.Point(355, 91);
            this.Luv_Button.Name = "Luv_Button";
            this.Luv_Button.Size = new System.Drawing.Size(56, 20);
            this.Luv_Button.TabIndex = 23;
            this.Luv_Button.Text = "Convert";
            this.MainToolTip.SetToolTip(this.Luv_Button, "Convert this color to all other colors");
            this.Luv_Button.UseVisualStyleBackColor = true;
            this.Luv_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // General_SpaceDrDo
            // 
            this.General_SpaceDrDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.General_SpaceDrDo.FormattingEnabled = true;
            this.General_SpaceDrDo.Location = new System.Drawing.Point(706, 351);
            this.General_SpaceDrDo.Name = "General_SpaceDrDo";
            this.General_SpaceDrDo.Size = new System.Drawing.Size(141, 21);
            this.General_SpaceDrDo.TabIndex = 38;
            this.MainToolTip.SetToolTip(this.General_SpaceDrDo, "This sets the colorspace for all compatible colors");
            this.General_SpaceDrDo.SelectedIndexChanged += new System.EventHandler(this.Space_CoBox_SelectedIndexChanged);
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(765, 502);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(82, 51);
            this.ClearButton.TabIndex = 36;
            this.ClearButton.Text = "Clear";
            this.MainToolTip.SetToolTip(this.ClearButton, "Clear all values from all colors");
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // Yxy_Y
            // 
            this.Yxy_Y.Location = new System.Drawing.Point(74, 39);
            this.Yxy_Y.Name = "Yxy_Y";
            this.Yxy_Y.Size = new System.Drawing.Size(86, 20);
            this.Yxy_Y.TabIndex = 24;
            this.MainToolTip.SetToolTip(this.Yxy_Y, "Y-Channel");
            // 
            // Yxy_x
            // 
            this.Yxy_x.Location = new System.Drawing.Point(166, 39);
            this.Yxy_x.Name = "Yxy_x";
            this.Yxy_x.Size = new System.Drawing.Size(86, 20);
            this.Yxy_x.TabIndex = 25;
            this.MainToolTip.SetToolTip(this.Yxy_x, "x-Channel");
            // 
            // Yxy_sy
            // 
            this.Yxy_sy.Location = new System.Drawing.Point(258, 39);
            this.Yxy_sy.Name = "Yxy_sy";
            this.Yxy_sy.Size = new System.Drawing.Size(86, 20);
            this.Yxy_sy.TabIndex = 26;
            this.MainToolTip.SetToolTip(this.Yxy_sy, "y-Channel");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(38, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 16);
            this.label7.TabIndex = 1;
            this.label7.Text = "Yxy";
            // 
            // Yxy_Button
            // 
            this.Yxy_Button.Location = new System.Drawing.Point(355, 39);
            this.Yxy_Button.Name = "Yxy_Button";
            this.Yxy_Button.Size = new System.Drawing.Size(56, 20);
            this.Yxy_Button.TabIndex = 27;
            this.Yxy_Button.Text = "Convert";
            this.MainToolTip.SetToolTip(this.Yxy_Button, "Convert this color to all other colors");
            this.Yxy_Button.UseVisualStyleBackColor = true;
            this.Yxy_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // HSV_H
            // 
            this.HSV_H.Location = new System.Drawing.Point(74, 299);
            this.HSV_H.Name = "HSV_H";
            this.HSV_H.Size = new System.Drawing.Size(86, 20);
            this.HSV_H.TabIndex = 28;
            this.MainToolTip.SetToolTip(this.HSV_H, "H-Channel");
            // 
            // HSV_S
            // 
            this.HSV_S.Location = new System.Drawing.Point(166, 299);
            this.HSV_S.Name = "HSV_S";
            this.HSV_S.Size = new System.Drawing.Size(86, 20);
            this.HSV_S.TabIndex = 29;
            this.MainToolTip.SetToolTip(this.HSV_S, "S-Channel");
            // 
            // HSV_V
            // 
            this.HSV_V.Location = new System.Drawing.Point(258, 299);
            this.HSV_V.Name = "HSV_V";
            this.HSV_V.Size = new System.Drawing.Size(86, 20);
            this.HSV_V.TabIndex = 30;
            this.MainToolTip.SetToolTip(this.HSV_V, "V-Channel");
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(32, 301);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 16);
            this.label8.TabIndex = 1;
            this.label8.Text = "HSV";
            // 
            // HSV_Button
            // 
            this.HSV_Button.Location = new System.Drawing.Point(355, 299);
            this.HSV_Button.Name = "HSV_Button";
            this.HSV_Button.Size = new System.Drawing.Size(56, 20);
            this.HSV_Button.TabIndex = 31;
            this.HSV_Button.Text = "Convert";
            this.MainToolTip.SetToolTip(this.HSV_Button, "Convert this color to all other colors");
            this.HSV_Button.UseVisualStyleBackColor = true;
            this.HSV_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // HSL_H
            // 
            this.HSL_H.Location = new System.Drawing.Point(74, 325);
            this.HSL_H.Name = "HSL_H";
            this.HSL_H.Size = new System.Drawing.Size(86, 20);
            this.HSL_H.TabIndex = 32;
            this.MainToolTip.SetToolTip(this.HSL_H, "H-Channel");
            // 
            // HSL_S
            // 
            this.HSL_S.Location = new System.Drawing.Point(166, 325);
            this.HSL_S.Name = "HSL_S";
            this.HSL_S.Size = new System.Drawing.Size(86, 20);
            this.HSL_S.TabIndex = 33;
            this.MainToolTip.SetToolTip(this.HSL_S, "S-Channel");
            // 
            // HSL_L
            // 
            this.HSL_L.Location = new System.Drawing.Point(258, 325);
            this.HSL_L.Name = "HSL_L";
            this.HSL_L.Size = new System.Drawing.Size(86, 20);
            this.HSL_L.TabIndex = 34;
            this.MainToolTip.SetToolTip(this.HSL_L, "L-Channel");
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(34, 326);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 16);
            this.label9.TabIndex = 1;
            this.label9.Text = "HSL";
            // 
            // HSL_Button
            // 
            this.HSL_Button.Location = new System.Drawing.Point(355, 325);
            this.HSL_Button.Name = "HSL_Button";
            this.HSL_Button.Size = new System.Drawing.Size(56, 20);
            this.HSL_Button.TabIndex = 35;
            this.HSL_Button.Text = "Convert";
            this.MainToolTip.SetToolTip(this.HSL_Button, "Convert this color to all other colors");
            this.HSL_Button.UseVisualStyleBackColor = true;
            this.HSL_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // ColorPanel
            // 
            this.ColorPanel.BackColor = System.Drawing.Color.White;
            this.ColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ColorPanel.Location = new System.Drawing.Point(706, 502);
            this.ColorPanel.Name = "ColorPanel";
            this.ColorPanel.Size = new System.Drawing.Size(50, 50);
            this.ColorPanel.TabIndex = 39;
            this.MainToolTip.SetToolTip(this.ColorPanel, "Click to choose an RGB color");
            this.ColorPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ColorPanel_MouseClick);
            // 
            // RGB_Label
            // 
            this.RGB_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RGB_Label.Location = new System.Drawing.Point(703, 458);
            this.RGB_Label.Name = "RGB_Label";
            this.RGB_Label.Size = new System.Drawing.Size(138, 16);
            this.RGB_Label.TabIndex = 40;
            this.RGB_Label.Text = "R: 0 G: 0 B: 0";
            this.MainToolTip.SetToolTip(this.RGB_Label, "RGB color in the range 0-255");
            // 
            // Hex_Label
            // 
            this.Hex_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Hex_Label.Location = new System.Drawing.Point(703, 483);
            this.Hex_Label.Name = "Hex_Label";
            this.Hex_Label.Size = new System.Drawing.Size(138, 16);
            this.Hex_Label.TabIndex = 40;
            this.Hex_Label.Text = "Hex: #000000";
            this.MainToolTip.SetToolTip(this.Hex_Label, "RGB color in hexadecimal");
            // 
            // RefWhiteDrDo
            // 
            this.RefWhiteDrDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RefWhiteDrDo.FormattingEnabled = true;
            this.RefWhiteDrDo.Location = new System.Drawing.Point(706, 375);
            this.RefWhiteDrDo.Name = "RefWhiteDrDo";
            this.RefWhiteDrDo.Size = new System.Drawing.Size(141, 21);
            this.RefWhiteDrDo.TabIndex = 41;
            this.MainToolTip.SetToolTip(this.RefWhiteDrDo, "This sets the reference white for all compatible colors");
            this.RefWhiteDrDo.SelectedIndexChanged += new System.EventHandler(this.RefWhiteDrDo_SelectedIndexChanged);
            // 
            // iccOpenDialog
            // 
            this.iccOpenDialog.DefaultExt = "*.icc";
            this.iccOpenDialog.FileName = "ICC_Profile.icc";
            this.iccOpenDialog.Filter = "ICC-Profile | *.icc";
            this.iccOpenDialog.Title = "Select ICC Profile";
            // 
            // CMY_C
            // 
            this.CMY_C.Location = new System.Drawing.Point(74, 431);
            this.CMY_C.Name = "CMY_C";
            this.CMY_C.Size = new System.Drawing.Size(86, 20);
            this.CMY_C.TabIndex = 32;
            this.MainToolTip.SetToolTip(this.CMY_C, "C-Channel");
            // 
            // CMY_M
            // 
            this.CMY_M.Location = new System.Drawing.Point(166, 431);
            this.CMY_M.Name = "CMY_M";
            this.CMY_M.Size = new System.Drawing.Size(86, 20);
            this.CMY_M.TabIndex = 33;
            this.MainToolTip.SetToolTip(this.CMY_M, "M-Channel");
            // 
            // CMY_Y
            // 
            this.CMY_Y.Location = new System.Drawing.Point(258, 431);
            this.CMY_Y.Name = "CMY_Y";
            this.CMY_Y.Size = new System.Drawing.Size(86, 20);
            this.CMY_Y.TabIndex = 34;
            this.MainToolTip.SetToolTip(this.CMY_Y, "Y-Channel");
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(31, 432);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 16);
            this.label10.TabIndex = 1;
            this.label10.Text = "CMY";
            // 
            // CMY_Button
            // 
            this.CMY_Button.Location = new System.Drawing.Point(355, 431);
            this.CMY_Button.Name = "CMY_Button";
            this.CMY_Button.Size = new System.Drawing.Size(56, 20);
            this.CMY_Button.TabIndex = 35;
            this.CMY_Button.Text = "Convert";
            this.MainToolTip.SetToolTip(this.CMY_Button, "Convert this color to all other colors");
            this.CMY_Button.UseVisualStyleBackColor = true;
            this.CMY_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // CMYK_C
            // 
            this.CMYK_C.Location = new System.Drawing.Point(74, 457);
            this.CMYK_C.Name = "CMYK_C";
            this.CMYK_C.Size = new System.Drawing.Size(62, 20);
            this.CMYK_C.TabIndex = 32;
            this.MainToolTip.SetToolTip(this.CMYK_C, "C-Channel");
            // 
            // CMYK_M
            // 
            this.CMYK_M.Location = new System.Drawing.Point(142, 457);
            this.CMYK_M.Name = "CMYK_M";
            this.CMYK_M.Size = new System.Drawing.Size(62, 20);
            this.CMYK_M.TabIndex = 33;
            this.MainToolTip.SetToolTip(this.CMYK_M, "M-Channel");
            // 
            // CMYK_Y
            // 
            this.CMYK_Y.Location = new System.Drawing.Point(210, 458);
            this.CMYK_Y.Name = "CMYK_Y";
            this.CMYK_Y.Size = new System.Drawing.Size(62, 20);
            this.CMYK_Y.TabIndex = 34;
            this.MainToolTip.SetToolTip(this.CMYK_Y, "Y-Channel");
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(23, 458);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(45, 16);
            this.label11.TabIndex = 1;
            this.label11.Text = "CMYK";
            // 
            // CMYK_Button
            // 
            this.CMYK_Button.Location = new System.Drawing.Point(355, 457);
            this.CMYK_Button.Name = "CMYK_Button";
            this.CMYK_Button.Size = new System.Drawing.Size(56, 20);
            this.CMYK_Button.TabIndex = 35;
            this.CMYK_Button.Text = "Convert";
            this.MainToolTip.SetToolTip(this.CMYK_Button, "Convert this color to all other colors");
            this.CMYK_Button.UseVisualStyleBackColor = true;
            this.CMYK_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // CMYK_K
            // 
            this.CMYK_K.Location = new System.Drawing.Point(278, 458);
            this.CMYK_K.Name = "CMYK_K";
            this.CMYK_K.Size = new System.Drawing.Size(66, 20);
            this.CMYK_K.TabIndex = 34;
            this.MainToolTip.SetToolTip(this.CMYK_K, "K-Channel");
            // 
            // CMY_ICCbox
            // 
            this.CMY_ICCbox.Location = new System.Drawing.Point(556, 509);
            this.CMY_ICCbox.Name = "CMY_ICCbox";
            this.CMY_ICCbox.ReadOnly = true;
            this.CMY_ICCbox.Size = new System.Drawing.Size(138, 20);
            this.CMY_ICCbox.TabIndex = 42;
            // 
            // CMYK_ICCbox
            // 
            this.CMYK_ICCbox.Location = new System.Drawing.Point(556, 535);
            this.CMYK_ICCbox.Name = "CMYK_ICCbox";
            this.CMYK_ICCbox.ReadOnly = true;
            this.CMYK_ICCbox.Size = new System.Drawing.Size(138, 20);
            this.CMYK_ICCbox.TabIndex = 42;
            // 
            // YCbCr_Y
            // 
            this.YCbCr_Y.Location = new System.Drawing.Point(74, 351);
            this.YCbCr_Y.Name = "YCbCr_Y";
            this.YCbCr_Y.Size = new System.Drawing.Size(86, 20);
            this.YCbCr_Y.TabIndex = 32;
            this.MainToolTip.SetToolTip(this.YCbCr_Y, "Y-Channel");
            // 
            // YCbCr_Cb
            // 
            this.YCbCr_Cb.Location = new System.Drawing.Point(166, 351);
            this.YCbCr_Cb.Name = "YCbCr_Cb";
            this.YCbCr_Cb.Size = new System.Drawing.Size(86, 20);
            this.YCbCr_Cb.TabIndex = 33;
            this.MainToolTip.SetToolTip(this.YCbCr_Cb, "Cb-Channel");
            // 
            // YCbCr_Cr
            // 
            this.YCbCr_Cr.Location = new System.Drawing.Point(258, 351);
            this.YCbCr_Cr.Name = "YCbCr_Cr";
            this.YCbCr_Cr.Size = new System.Drawing.Size(86, 20);
            this.YCbCr_Cr.TabIndex = 34;
            this.MainToolTip.SetToolTip(this.YCbCr_Cr, "Cr-Channel");
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(21, 352);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 16);
            this.label12.TabIndex = 1;
            this.label12.Text = "YCbCr";
            // 
            // YCbCr_Button
            // 
            this.YCbCr_Button.Location = new System.Drawing.Point(355, 351);
            this.YCbCr_Button.Name = "YCbCr_Button";
            this.YCbCr_Button.Size = new System.Drawing.Size(56, 20);
            this.YCbCr_Button.TabIndex = 35;
            this.YCbCr_Button.Text = "Convert";
            this.MainToolTip.SetToolTip(this.YCbCr_Button, "Convert this color to all other colors");
            this.YCbCr_Button.UseVisualStyleBackColor = true;
            this.YCbCr_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // YCbCr_ICCbox
            // 
            this.YCbCr_ICCbox.Location = new System.Drawing.Point(556, 427);
            this.YCbCr_ICCbox.Name = "YCbCr_ICCbox";
            this.YCbCr_ICCbox.ReadOnly = true;
            this.YCbCr_ICCbox.Size = new System.Drawing.Size(138, 20);
            this.YCbCr_ICCbox.TabIndex = 42;
            // 
            // Gray_G
            // 
            this.Gray_G.Location = new System.Drawing.Point(74, 377);
            this.Gray_G.Name = "Gray_G";
            this.Gray_G.Size = new System.Drawing.Size(86, 20);
            this.Gray_G.TabIndex = 32;
            this.MainToolTip.SetToolTip(this.Gray_G, "Gray-Channel");
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(21, 378);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(37, 16);
            this.label13.TabIndex = 1;
            this.label13.Text = "Gray";
            // 
            // Gray_Button
            // 
            this.Gray_Button.Location = new System.Drawing.Point(355, 377);
            this.Gray_Button.Name = "Gray_Button";
            this.Gray_Button.Size = new System.Drawing.Size(56, 20);
            this.Gray_Button.TabIndex = 35;
            this.Gray_Button.Text = "Convert";
            this.MainToolTip.SetToolTip(this.Gray_Button, "Convert this color to all other colors");
            this.Gray_Button.UseVisualStyleBackColor = true;
            this.Gray_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // Gray_ICCbox
            // 
            this.Gray_ICCbox.Location = new System.Drawing.Point(556, 453);
            this.Gray_ICCbox.Name = "Gray_ICCbox";
            this.Gray_ICCbox.ReadOnly = true;
            this.Gray_ICCbox.Size = new System.Drawing.Size(138, 20);
            this.Gray_ICCbox.TabIndex = 42;
            // 
            // XColor_X
            // 
            this.XColor_X.Location = new System.Drawing.Point(74, 405);
            this.XColor_X.Name = "XColor_X";
            this.XColor_X.Size = new System.Drawing.Size(86, 20);
            this.XColor_X.TabIndex = 32;
            this.MainToolTip.SetToolTip(this.XColor_X, "X-Channel");
            this.XColor_X.TextChanged += new System.EventHandler(this.XColor_X_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(16, 404);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 16);
            this.label14.TabIndex = 1;
            this.label14.Text = "X-Color";
            // 
            // XColor_Button
            // 
            this.XColor_Button.Location = new System.Drawing.Point(355, 405);
            this.XColor_Button.Name = "XColor_Button";
            this.XColor_Button.Size = new System.Drawing.Size(56, 20);
            this.XColor_Button.TabIndex = 35;
            this.XColor_Button.Text = "Convert";
            this.MainToolTip.SetToolTip(this.XColor_Button, "Convert this color to all other colors");
            this.XColor_Button.UseVisualStyleBackColor = true;
            this.XColor_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // XColor_ICCbox
            // 
            this.XColor_ICCbox.Location = new System.Drawing.Point(556, 479);
            this.XColor_ICCbox.Name = "XColor_ICCbox";
            this.XColor_ICCbox.ReadOnly = true;
            this.XColor_ICCbox.Size = new System.Drawing.Size(138, 20);
            this.XColor_ICCbox.TabIndex = 42;
            // 
            // XColor_ChannelUpDo
            // 
            this.XColor_ChannelUpDo.Location = new System.Drawing.Point(166, 405);
            this.XColor_ChannelUpDo.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.XColor_ChannelUpDo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.XColor_ChannelUpDo.Name = "XColor_ChannelUpDo";
            this.XColor_ChannelUpDo.Size = new System.Drawing.Size(86, 20);
            this.XColor_ChannelUpDo.TabIndex = 43;
            this.MainToolTip.SetToolTip(this.XColor_ChannelUpDo, "Select channel from X-Color");
            this.XColor_ChannelUpDo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.XColor_ChannelUpDo.ValueChanged += new System.EventHandler(this.XColor_ChannelUpDo_ValueChanged);
            // 
            // HSV_CoBox
            // 
            this.HSV_CoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.HSV_CoBox.FormattingEnabled = true;
            this.HSV_CoBox.Location = new System.Drawing.Point(417, 299);
            this.HSV_CoBox.Name = "HSV_CoBox";
            this.HSV_CoBox.Size = new System.Drawing.Size(133, 21);
            this.HSV_CoBox.TabIndex = 38;
            this.MainToolTip.SetToolTip(this.HSV_CoBox, "Select colorspace for this color");
            this.HSV_CoBox.SelectedIndexChanged += new System.EventHandler(this.Space_CoBox_SelectedIndexChanged);
            // 
            // HSV_ICCbox
            // 
            this.HSV_ICCbox.Location = new System.Drawing.Point(556, 377);
            this.HSV_ICCbox.Name = "HSV_ICCbox";
            this.HSV_ICCbox.ReadOnly = true;
            this.HSV_ICCbox.Size = new System.Drawing.Size(138, 20);
            this.HSV_ICCbox.TabIndex = 42;
            // 
            // HSL_CoBox
            // 
            this.HSL_CoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.HSL_CoBox.FormattingEnabled = true;
            this.HSL_CoBox.Location = new System.Drawing.Point(417, 325);
            this.HSL_CoBox.Name = "HSL_CoBox";
            this.HSL_CoBox.Size = new System.Drawing.Size(133, 21);
            this.HSL_CoBox.TabIndex = 38;
            this.MainToolTip.SetToolTip(this.HSL_CoBox, "Select colorspace for this color");
            this.HSL_CoBox.SelectedIndexChanged += new System.EventHandler(this.Space_CoBox_SelectedIndexChanged);
            // 
            // HSL_ICCbox
            // 
            this.HSL_ICCbox.Location = new System.Drawing.Point(556, 403);
            this.HSL_ICCbox.Name = "HSL_ICCbox";
            this.HSL_ICCbox.ReadOnly = true;
            this.HSL_ICCbox.Size = new System.Drawing.Size(138, 20);
            this.HSL_ICCbox.TabIndex = 42;
            // 
            // CMY_ChICC
            // 
            this.CMY_ChICC.Location = new System.Drawing.Point(417, 432);
            this.CMY_ChICC.Name = "CMY_ChICC";
            this.CMY_ChICC.Size = new System.Drawing.Size(133, 20);
            this.CMY_ChICC.TabIndex = 44;
            this.CMY_ChICC.Text = "Choose ICC...";
            this.MainToolTip.SetToolTip(this.CMY_ChICC, "Choose ICC file from disk");
            this.CMY_ChICC.UseVisualStyleBackColor = true;
            this.CMY_ChICC.Click += new System.EventHandler(this.ChooseICC_Click);
            // 
            // CMYK_ChICC
            // 
            this.CMYK_ChICC.Location = new System.Drawing.Point(417, 459);
            this.CMYK_ChICC.Name = "CMYK_ChICC";
            this.CMYK_ChICC.Size = new System.Drawing.Size(133, 20);
            this.CMYK_ChICC.TabIndex = 44;
            this.CMYK_ChICC.Text = "Choose ICC...";
            this.MainToolTip.SetToolTip(this.CMYK_ChICC, "Choose ICC file from disk");
            this.CMYK_ChICC.UseVisualStyleBackColor = true;
            this.CMYK_ChICC.Click += new System.EventHandler(this.ChooseICC_Click);
            // 
            // Gray_ChICC
            // 
            this.Gray_ChICC.Location = new System.Drawing.Point(417, 378);
            this.Gray_ChICC.Name = "Gray_ChICC";
            this.Gray_ChICC.Size = new System.Drawing.Size(133, 20);
            this.Gray_ChICC.TabIndex = 44;
            this.Gray_ChICC.Text = "Choose ICC...";
            this.MainToolTip.SetToolTip(this.Gray_ChICC, "Choose ICC file from disk");
            this.Gray_ChICC.UseVisualStyleBackColor = true;
            this.Gray_ChICC.Click += new System.EventHandler(this.ChooseICC_Click);
            // 
            // ColorX_ChICC
            // 
            this.ColorX_ChICC.Location = new System.Drawing.Point(417, 405);
            this.ColorX_ChICC.Name = "ColorX_ChICC";
            this.ColorX_ChICC.Size = new System.Drawing.Size(133, 20);
            this.ColorX_ChICC.TabIndex = 44;
            this.ColorX_ChICC.Text = "Choose ICC...";
            this.MainToolTip.SetToolTip(this.ColorX_ChICC, "Choose ICC file from disk");
            this.ColorX_ChICC.UseVisualStyleBackColor = true;
            this.ColorX_ChICC.Click += new System.EventHandler(this.ChooseICC_Click);
            // 
            // Lab_CoBox
            // 
            this.Lab_CoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Lab_CoBox.FormattingEnabled = true;
            this.Lab_CoBox.Location = new System.Drawing.Point(417, 64);
            this.Lab_CoBox.Name = "Lab_CoBox";
            this.Lab_CoBox.Size = new System.Drawing.Size(133, 21);
            this.Lab_CoBox.TabIndex = 41;
            this.MainToolTip.SetToolTip(this.Lab_CoBox, "Select reference white for this color");
            // 
            // LCHab_CoBox
            // 
            this.LCHab_CoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LCHab_CoBox.FormattingEnabled = true;
            this.LCHab_CoBox.Location = new System.Drawing.Point(417, 117);
            this.LCHab_CoBox.Name = "LCHab_CoBox";
            this.LCHab_CoBox.Size = new System.Drawing.Size(133, 21);
            this.LCHab_CoBox.TabIndex = 41;
            this.MainToolTip.SetToolTip(this.LCHab_CoBox, "Select reference white for this color");
            // 
            // LCHuv_CoBox
            // 
            this.LCHuv_CoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LCHuv_CoBox.FormattingEnabled = true;
            this.LCHuv_CoBox.Location = new System.Drawing.Point(417, 143);
            this.LCHuv_CoBox.Name = "LCHuv_CoBox";
            this.LCHuv_CoBox.Size = new System.Drawing.Size(133, 21);
            this.LCHuv_CoBox.TabIndex = 41;
            this.MainToolTip.SetToolTip(this.LCHuv_CoBox, "Select reference white for this color");
            // 
            // Luv_CoBox
            // 
            this.Luv_CoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Luv_CoBox.FormattingEnabled = true;
            this.Luv_CoBox.Location = new System.Drawing.Point(417, 92);
            this.Luv_CoBox.Name = "Luv_CoBox";
            this.Luv_CoBox.Size = new System.Drawing.Size(133, 21);
            this.Luv_CoBox.TabIndex = 41;
            this.MainToolTip.SetToolTip(this.Luv_CoBox, "Select reference white for this color");
            // 
            // Yxy_CoBox
            // 
            this.Yxy_CoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Yxy_CoBox.FormattingEnabled = true;
            this.Yxy_CoBox.Location = new System.Drawing.Point(417, 38);
            this.Yxy_CoBox.Name = "Yxy_CoBox";
            this.Yxy_CoBox.Size = new System.Drawing.Size(133, 21);
            this.Yxy_CoBox.TabIndex = 41;
            this.MainToolTip.SetToolTip(this.Yxy_CoBox, "Select reference white for this color");
            // 
            // XYZ_CoBox
            // 
            this.XYZ_CoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.XYZ_CoBox.FormattingEnabled = true;
            this.XYZ_CoBox.Location = new System.Drawing.Point(417, 12);
            this.XYZ_CoBox.Name = "XYZ_CoBox";
            this.XYZ_CoBox.Size = new System.Drawing.Size(133, 21);
            this.XYZ_CoBox.TabIndex = 41;
            this.MainToolTip.SetToolTip(this.XYZ_CoBox, "Select reference white for this color");
            // 
            // RGB_CoBox
            // 
            this.RGB_CoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RGB_CoBox.FormattingEnabled = true;
            this.RGB_CoBox.Location = new System.Drawing.Point(417, 273);
            this.RGB_CoBox.Name = "RGB_CoBox";
            this.RGB_CoBox.Size = new System.Drawing.Size(133, 21);
            this.RGB_CoBox.TabIndex = 38;
            this.MainToolTip.SetToolTip(this.RGB_CoBox, "Select colorspace for this color");
            this.RGB_CoBox.SelectedIndexChanged += new System.EventHandler(this.Space_CoBox_SelectedIndexChanged);
            // 
            // RGB_ICCbox
            // 
            this.RGB_ICCbox.Location = new System.Drawing.Point(556, 351);
            this.RGB_ICCbox.Name = "RGB_ICCbox";
            this.RGB_ICCbox.ReadOnly = true;
            this.RGB_ICCbox.Size = new System.Drawing.Size(138, 20);
            this.RGB_ICCbox.TabIndex = 42;
            // 
            // XColor_Channel
            // 
            this.XColor_Channel.Location = new System.Drawing.Point(258, 405);
            this.XColor_Channel.Name = "XColor_Channel";
            this.XColor_Channel.Size = new System.Drawing.Size(86, 20);
            this.XColor_Channel.TabIndex = 32;
            this.XColor_Channel.Text = "15";
            this.MainToolTip.SetToolTip(this.XColor_Channel, "Number of channels for X-Color");
            this.XColor_Channel.TextChanged += new System.EventHandler(this.XColor_Channel_TextChanged);
            // 
            // YCbCr_CoBox
            // 
            this.YCbCr_CoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.YCbCr_CoBox.FormattingEnabled = true;
            this.YCbCr_CoBox.Location = new System.Drawing.Point(417, 350);
            this.YCbCr_CoBox.Name = "YCbCr_CoBox";
            this.YCbCr_CoBox.Size = new System.Drawing.Size(133, 21);
            this.YCbCr_CoBox.TabIndex = 38;
            this.MainToolTip.SetToolTip(this.YCbCr_CoBox, "Select colorspace for this color");
            this.YCbCr_CoBox.SelectedIndexChanged += new System.EventHandler(this.YCbCr_CoBox_SelectedIndexChanged);
            // 
            // RenderIntentCoBox
            // 
            this.RenderIntentCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RenderIntentCoBox.FormattingEnabled = true;
            this.RenderIntentCoBox.Location = new System.Drawing.Point(706, 427);
            this.RenderIntentCoBox.Name = "RenderIntentCoBox";
            this.RenderIntentCoBox.Size = new System.Drawing.Size(141, 21);
            this.RenderIntentCoBox.TabIndex = 41;
            this.MainToolTip.SetToolTip(this.RenderIntentCoBox, "The preferred rendering intent for the ICCs");
            this.RenderIntentCoBox.SelectedIndexChanged += new System.EventHandler(this.RenderIntentCoBox_SelectedIndexChanged);
            // 
            // ChroAdaptCoBox
            // 
            this.ChroAdaptCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChroAdaptCoBox.FormattingEnabled = true;
            this.ChroAdaptCoBox.Location = new System.Drawing.Point(706, 401);
            this.ChroAdaptCoBox.Name = "ChroAdaptCoBox";
            this.ChroAdaptCoBox.Size = new System.Drawing.Size(141, 21);
            this.ChroAdaptCoBox.TabIndex = 41;
            this.MainToolTip.SetToolTip(this.ChroAdaptCoBox, "The chromatic adaption method (Bradford is considered the best)");
            this.ChroAdaptCoBox.SelectedIndexChanged += new System.EventHandler(this.ChroAdaptCoBox_SelectedIndexChanged);
            // 
            // LCH99_L
            // 
            this.LCH99_L.Location = new System.Drawing.Point(74, 169);
            this.LCH99_L.Name = "LCH99_L";
            this.LCH99_L.Size = new System.Drawing.Size(86, 20);
            this.LCH99_L.TabIndex = 16;
            this.MainToolTip.SetToolTip(this.LCH99_L, "L-Channel");
            // 
            // LCH99_C
            // 
            this.LCH99_C.Location = new System.Drawing.Point(166, 169);
            this.LCH99_C.Name = "LCH99_C";
            this.LCH99_C.Size = new System.Drawing.Size(86, 20);
            this.LCH99_C.TabIndex = 17;
            this.MainToolTip.SetToolTip(this.LCH99_C, "C-Channel");
            // 
            // LCH99_H
            // 
            this.LCH99_H.Location = new System.Drawing.Point(258, 169);
            this.LCH99_H.Name = "LCH99_H";
            this.LCH99_H.Size = new System.Drawing.Size(86, 20);
            this.LCH99_H.TabIndex = 18;
            this.MainToolTip.SetToolTip(this.LCH99_H, "H-Channel");
            // 
            // LCH99_Button
            // 
            this.LCH99_Button.Location = new System.Drawing.Point(355, 169);
            this.LCH99_Button.Name = "LCH99_Button";
            this.LCH99_Button.Size = new System.Drawing.Size(56, 20);
            this.LCH99_Button.TabIndex = 19;
            this.LCH99_Button.Text = "Convert";
            this.MainToolTip.SetToolTip(this.LCH99_Button, "Convert this color to all other colors");
            this.LCH99_Button.UseVisualStyleBackColor = true;
            this.LCH99_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // LCH99b_L
            // 
            this.LCH99b_L.Location = new System.Drawing.Point(74, 195);
            this.LCH99b_L.Name = "LCH99b_L";
            this.LCH99b_L.Size = new System.Drawing.Size(86, 20);
            this.LCH99b_L.TabIndex = 16;
            this.MainToolTip.SetToolTip(this.LCH99b_L, "L-Channel");
            // 
            // LCH99b_C
            // 
            this.LCH99b_C.Location = new System.Drawing.Point(166, 195);
            this.LCH99b_C.Name = "LCH99b_C";
            this.LCH99b_C.Size = new System.Drawing.Size(86, 20);
            this.LCH99b_C.TabIndex = 17;
            this.MainToolTip.SetToolTip(this.LCH99b_C, "C-Channel");
            // 
            // LCH99b_H
            // 
            this.LCH99b_H.Location = new System.Drawing.Point(258, 195);
            this.LCH99b_H.Name = "LCH99b_H";
            this.LCH99b_H.Size = new System.Drawing.Size(86, 20);
            this.LCH99b_H.TabIndex = 18;
            this.MainToolTip.SetToolTip(this.LCH99b_H, "H-Channel");
            // 
            // LCH99b_Button
            // 
            this.LCH99b_Button.Location = new System.Drawing.Point(355, 195);
            this.LCH99b_Button.Name = "LCH99b_Button";
            this.LCH99b_Button.Size = new System.Drawing.Size(56, 20);
            this.LCH99b_Button.TabIndex = 19;
            this.LCH99b_Button.Text = "Convert";
            this.MainToolTip.SetToolTip(this.LCH99b_Button, "Convert this color to all other colors");
            this.LCH99b_Button.UseVisualStyleBackColor = true;
            this.LCH99b_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // LCH99c_L
            // 
            this.LCH99c_L.Location = new System.Drawing.Point(74, 221);
            this.LCH99c_L.Name = "LCH99c_L";
            this.LCH99c_L.Size = new System.Drawing.Size(86, 20);
            this.LCH99c_L.TabIndex = 16;
            this.MainToolTip.SetToolTip(this.LCH99c_L, "L-Channel");
            // 
            // LCH99c_C
            // 
            this.LCH99c_C.Location = new System.Drawing.Point(166, 221);
            this.LCH99c_C.Name = "LCH99c_C";
            this.LCH99c_C.Size = new System.Drawing.Size(86, 20);
            this.LCH99c_C.TabIndex = 17;
            this.MainToolTip.SetToolTip(this.LCH99c_C, "C-Channel");
            // 
            // LCH99c_H
            // 
            this.LCH99c_H.Location = new System.Drawing.Point(258, 221);
            this.LCH99c_H.Name = "LCH99c_H";
            this.LCH99c_H.Size = new System.Drawing.Size(86, 20);
            this.LCH99c_H.TabIndex = 18;
            this.MainToolTip.SetToolTip(this.LCH99c_H, "H-Channel");
            // 
            // LCH99c_Button
            // 
            this.LCH99c_Button.Location = new System.Drawing.Point(355, 221);
            this.LCH99c_Button.Name = "LCH99c_Button";
            this.LCH99c_Button.Size = new System.Drawing.Size(56, 20);
            this.LCH99c_Button.TabIndex = 19;
            this.LCH99c_Button.Text = "Convert";
            this.MainToolTip.SetToolTip(this.LCH99c_Button, "Convert this color to all other colors");
            this.LCH99c_Button.UseVisualStyleBackColor = true;
            this.LCH99c_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // LCH99d_L
            // 
            this.LCH99d_L.Location = new System.Drawing.Point(74, 247);
            this.LCH99d_L.Name = "LCH99d_L";
            this.LCH99d_L.Size = new System.Drawing.Size(86, 20);
            this.LCH99d_L.TabIndex = 16;
            this.MainToolTip.SetToolTip(this.LCH99d_L, "L-Channel");
            // 
            // LCH99d_C
            // 
            this.LCH99d_C.Location = new System.Drawing.Point(166, 247);
            this.LCH99d_C.Name = "LCH99d_C";
            this.LCH99d_C.Size = new System.Drawing.Size(86, 20);
            this.LCH99d_C.TabIndex = 17;
            this.MainToolTip.SetToolTip(this.LCH99d_C, "C-Channel");
            // 
            // LCH99d_H
            // 
            this.LCH99d_H.Location = new System.Drawing.Point(258, 247);
            this.LCH99d_H.Name = "LCH99d_H";
            this.LCH99d_H.Size = new System.Drawing.Size(86, 20);
            this.LCH99d_H.TabIndex = 18;
            this.MainToolTip.SetToolTip(this.LCH99d_H, "H-Channel");
            // 
            // LCH99d_Button
            // 
            this.LCH99d_Button.Location = new System.Drawing.Point(355, 247);
            this.LCH99d_Button.Name = "LCH99d_Button";
            this.LCH99d_Button.Size = new System.Drawing.Size(56, 20);
            this.LCH99d_Button.TabIndex = 19;
            this.LCH99d_Button.Text = "Convert";
            this.MainToolTip.SetToolTip(this.LCH99d_Button, "Convert this color to all other colors");
            this.LCH99d_Button.UseVisualStyleBackColor = true;
            this.LCH99d_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // DescriptionBox
            // 
            this.DescriptionBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DescriptionBox.Location = new System.Drawing.Point(556, 15);
            this.DescriptionBox.Multiline = true;
            this.DescriptionBox.Name = "DescriptionBox";
            this.DescriptionBox.ReadOnly = true;
            this.DescriptionBox.Size = new System.Drawing.Size(291, 327);
            this.DescriptionBox.TabIndex = 45;
            this.DescriptionBox.TabStop = false;
            this.DescriptionBox.Text = resources.GetString("DescriptionBox.Text");
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(20, 171);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(48, 16);
            this.label15.TabIndex = 1;
            this.label15.Text = "LCH99";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(12, 197);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(56, 16);
            this.label16.TabIndex = 1;
            this.label16.Text = "LCH99b";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(13, 223);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(55, 16);
            this.label17.TabIndex = 1;
            this.label17.Text = "LCH99c";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(12, 249);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(56, 16);
            this.label18.TabIndex = 1;
            this.label18.Text = "LCH99d";
            // 
            // DEF_D
            // 
            this.DEF_D.Location = new System.Drawing.Point(74, 483);
            this.DEF_D.Name = "DEF_D";
            this.DEF_D.Size = new System.Drawing.Size(86, 20);
            this.DEF_D.TabIndex = 32;
            this.MainToolTip.SetToolTip(this.DEF_D, "D-Channel");
            // 
            // DEF_E
            // 
            this.DEF_E.Location = new System.Drawing.Point(166, 483);
            this.DEF_E.Name = "DEF_E";
            this.DEF_E.Size = new System.Drawing.Size(86, 20);
            this.DEF_E.TabIndex = 33;
            this.MainToolTip.SetToolTip(this.DEF_E, "E-Channel");
            // 
            // DEF_F
            // 
            this.DEF_F.Location = new System.Drawing.Point(258, 483);
            this.DEF_F.Name = "DEF_F";
            this.DEF_F.Size = new System.Drawing.Size(86, 20);
            this.DEF_F.TabIndex = 34;
            this.MainToolTip.SetToolTip(this.DEF_F, "F-Channel");
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(33, 484);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(35, 16);
            this.label19.TabIndex = 1;
            this.label19.Text = "DEF";
            // 
            // DEF_Button
            // 
            this.DEF_Button.Location = new System.Drawing.Point(355, 483);
            this.DEF_Button.Name = "DEF_Button";
            this.DEF_Button.Size = new System.Drawing.Size(56, 20);
            this.DEF_Button.TabIndex = 35;
            this.DEF_Button.Text = "Convert";
            this.DEF_Button.UseVisualStyleBackColor = true;
            this.DEF_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // Bef_B
            // 
            this.Bef_B.Location = new System.Drawing.Point(74, 509);
            this.Bef_B.Name = "Bef_B";
            this.Bef_B.Size = new System.Drawing.Size(86, 20);
            this.Bef_B.TabIndex = 32;
            this.MainToolTip.SetToolTip(this.Bef_B, "B-Channel");
            // 
            // Bef_e
            // 
            this.Bef_e.Location = new System.Drawing.Point(166, 509);
            this.Bef_e.Name = "Bef_e";
            this.Bef_e.Size = new System.Drawing.Size(86, 20);
            this.Bef_e.TabIndex = 33;
            this.MainToolTip.SetToolTip(this.Bef_e, "e-Channel");
            // 
            // Bef_f
            // 
            this.Bef_f.Location = new System.Drawing.Point(258, 509);
            this.Bef_f.Name = "Bef_f";
            this.Bef_f.Size = new System.Drawing.Size(86, 20);
            this.Bef_f.TabIndex = 34;
            this.MainToolTip.SetToolTip(this.Bef_f, "f-Channel");
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(40, 510);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(28, 16);
            this.label20.TabIndex = 1;
            this.label20.Text = "Bef";
            // 
            // Bef_Button
            // 
            this.Bef_Button.Location = new System.Drawing.Point(355, 509);
            this.Bef_Button.Name = "Bef_Button";
            this.Bef_Button.Size = new System.Drawing.Size(56, 20);
            this.Bef_Button.TabIndex = 35;
            this.Bef_Button.Text = "Convert";
            this.Bef_Button.UseVisualStyleBackColor = true;
            this.Bef_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // BCH_B
            // 
            this.BCH_B.Location = new System.Drawing.Point(74, 535);
            this.BCH_B.Name = "BCH_B";
            this.BCH_B.Size = new System.Drawing.Size(86, 20);
            this.BCH_B.TabIndex = 32;
            this.MainToolTip.SetToolTip(this.BCH_B, "B-Channel");
            // 
            // BCH_C
            // 
            this.BCH_C.Location = new System.Drawing.Point(166, 535);
            this.BCH_C.Name = "BCH_C";
            this.BCH_C.Size = new System.Drawing.Size(86, 20);
            this.BCH_C.TabIndex = 33;
            this.MainToolTip.SetToolTip(this.BCH_C, "C-Channel");
            // 
            // BCH_H
            // 
            this.BCH_H.Location = new System.Drawing.Point(258, 535);
            this.BCH_H.Name = "BCH_H";
            this.BCH_H.Size = new System.Drawing.Size(86, 20);
            this.BCH_H.TabIndex = 34;
            this.MainToolTip.SetToolTip(this.BCH_H, "H-Channel");
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(32, 536);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(36, 16);
            this.label21.TabIndex = 1;
            this.label21.Text = "BCH";
            // 
            // BCH_Button
            // 
            this.BCH_Button.Location = new System.Drawing.Point(355, 535);
            this.BCH_Button.Name = "BCH_Button";
            this.BCH_Button.Size = new System.Drawing.Size(56, 20);
            this.BCH_Button.TabIndex = 35;
            this.BCH_Button.Text = "Convert";
            this.BCH_Button.UseVisualStyleBackColor = true;
            this.BCH_Button.Click += new System.EventHandler(this.Convert_Button_Click);
            // 
            // DEF_CoBox
            // 
            this.DEF_CoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DEF_CoBox.FormattingEnabled = true;
            this.DEF_CoBox.Location = new System.Drawing.Point(417, 484);
            this.DEF_CoBox.Name = "DEF_CoBox";
            this.DEF_CoBox.Size = new System.Drawing.Size(133, 21);
            this.DEF_CoBox.TabIndex = 41;
            this.MainToolTip.SetToolTip(this.DEF_CoBox, "Select reference white for this color");
            // 
            // Bef_CoBox
            // 
            this.Bef_CoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Bef_CoBox.FormattingEnabled = true;
            this.Bef_CoBox.Location = new System.Drawing.Point(417, 509);
            this.Bef_CoBox.Name = "Bef_CoBox";
            this.Bef_CoBox.Size = new System.Drawing.Size(133, 21);
            this.Bef_CoBox.TabIndex = 41;
            this.MainToolTip.SetToolTip(this.Bef_CoBox, "Select reference white for this color");
            // 
            // BCH_CoBox
            // 
            this.BCH_CoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BCH_CoBox.FormattingEnabled = true;
            this.BCH_CoBox.Location = new System.Drawing.Point(417, 535);
            this.BCH_CoBox.Name = "BCH_CoBox";
            this.BCH_CoBox.Size = new System.Drawing.Size(133, 21);
            this.BCH_CoBox.TabIndex = 41;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 560);
            this.Controls.Add(this.DescriptionBox);
            this.Controls.Add(this.ColorX_ChICC);
            this.Controls.Add(this.Gray_ChICC);
            this.Controls.Add(this.CMYK_ChICC);
            this.Controls.Add(this.CMY_ChICC);
            this.Controls.Add(this.XColor_ChannelUpDo);
            this.Controls.Add(this.CMYK_ICCbox);
            this.Controls.Add(this.XColor_ICCbox);
            this.Controls.Add(this.Gray_ICCbox);
            this.Controls.Add(this.YCbCr_ICCbox);
            this.Controls.Add(this.CMY_ICCbox);
            this.Controls.Add(this.HSL_ICCbox);
            this.Controls.Add(this.HSV_ICCbox);
            this.Controls.Add(this.RGB_ICCbox);
            this.Controls.Add(this.Yxy_CoBox);
            this.Controls.Add(this.Luv_CoBox);
            this.Controls.Add(this.LCHab_CoBox);
            this.Controls.Add(this.BCH_CoBox);
            this.Controls.Add(this.Bef_CoBox);
            this.Controls.Add(this.DEF_CoBox);
            this.Controls.Add(this.LCHuv_CoBox);
            this.Controls.Add(this.Lab_CoBox);
            this.Controls.Add(this.XYZ_CoBox);
            this.Controls.Add(this.ChroAdaptCoBox);
            this.Controls.Add(this.RenderIntentCoBox);
            this.Controls.Add(this.RefWhiteDrDo);
            this.Controls.Add(this.Hex_Label);
            this.Controls.Add(this.RGB_Label);
            this.Controls.Add(this.ColorPanel);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.YCbCr_CoBox);
            this.Controls.Add(this.HSL_CoBox);
            this.Controls.Add(this.HSV_CoBox);
            this.Controls.Add(this.RGB_CoBox);
            this.Controls.Add(this.General_SpaceDrDo);
            this.Controls.Add(this.XColor_Button);
            this.Controls.Add(this.Gray_Button);
            this.Controls.Add(this.BCH_Button);
            this.Controls.Add(this.Bef_Button);
            this.Controls.Add(this.DEF_Button);
            this.Controls.Add(this.YCbCr_Button);
            this.Controls.Add(this.CMYK_Button);
            this.Controls.Add(this.CMY_Button);
            this.Controls.Add(this.HSL_Button);
            this.Controls.Add(this.HSV_Button);
            this.Controls.Add(this.Yxy_Button);
            this.Controls.Add(this.Luv_Button);
            this.Controls.Add(this.LCH99d_Button);
            this.Controls.Add(this.LCH99c_Button);
            this.Controls.Add(this.LCH99b_Button);
            this.Controls.Add(this.LCH99_Button);
            this.Controls.Add(this.LCHuv_Button);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.LCHab_Button);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.XYZ_Button);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.Lab_Button);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.RGB_Button);
            this.Controls.Add(this.BCH_H);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Bef_f);
            this.Controls.Add(this.CMYK_K);
            this.Controls.Add(this.DEF_F);
            this.Controls.Add(this.CMYK_Y);
            this.Controls.Add(this.YCbCr_Cr);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.CMY_Y);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.HSL_L);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.HSV_V);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Yxy_sy);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BCH_C);
            this.Controls.Add(this.Luv_v);
            this.Controls.Add(this.Bef_e);
            this.Controls.Add(this.CMYK_M);
            this.Controls.Add(this.DEF_E);
            this.Controls.Add(this.LCH99d_H);
            this.Controls.Add(this.YCbCr_Cb);
            this.Controls.Add(this.LCH99c_H);
            this.Controls.Add(this.LCH99b_H);
            this.Controls.Add(this.LCH99_H);
            this.Controls.Add(this.LCHuv_H);
            this.Controls.Add(this.CMY_M);
            this.Controls.Add(this.LCHab_H);
            this.Controls.Add(this.HSL_S);
            this.Controls.Add(this.XYZ_Z);
            this.Controls.Add(this.HSV_S);
            this.Controls.Add(this.Lab_b);
            this.Controls.Add(this.Yxy_x);
            this.Controls.Add(this.RGB_B);
            this.Controls.Add(this.XColor_Channel);
            this.Controls.Add(this.XColor_X);
            this.Controls.Add(this.Luv_u);
            this.Controls.Add(this.BCH_B);
            this.Controls.Add(this.Gray_G);
            this.Controls.Add(this.Bef_B);
            this.Controls.Add(this.CMYK_C);
            this.Controls.Add(this.DEF_D);
            this.Controls.Add(this.LCH99d_C);
            this.Controls.Add(this.YCbCr_Y);
            this.Controls.Add(this.LCH99c_C);
            this.Controls.Add(this.LCH99b_C);
            this.Controls.Add(this.LCH99_C);
            this.Controls.Add(this.LCHuv_C);
            this.Controls.Add(this.CMY_C);
            this.Controls.Add(this.LCHab_C);
            this.Controls.Add(this.HSL_H);
            this.Controls.Add(this.XYZ_Y);
            this.Controls.Add(this.HSV_H);
            this.Controls.Add(this.Lab_a);
            this.Controls.Add(this.Yxy_Y);
            this.Controls.Add(this.RGB_G);
            this.Controls.Add(this.LCH99d_L);
            this.Controls.Add(this.Luv_L);
            this.Controls.Add(this.LCH99c_L);
            this.Controls.Add(this.LCH99b_L);
            this.Controls.Add(this.LCH99_L);
            this.Controls.Add(this.LCHuv_L);
            this.Controls.Add(this.LCHab_L);
            this.Controls.Add(this.XYZ_X);
            this.Controls.Add(this.Lab_L);
            this.Controls.Add(this.RGB_R);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Color Converter";
            this.MainToolTip.SetToolTip(this, "Select reference white for this color");
            ((System.ComponentModel.ISupportInitialize)(this.XColor_ChannelUpDo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox RGB_R;
        private System.Windows.Forms.TextBox RGB_G;
        private System.Windows.Forms.TextBox RGB_B;
        private System.Windows.Forms.TextBox Lab_L;
        private System.Windows.Forms.TextBox Lab_a;
        private System.Windows.Forms.TextBox Lab_b;
        private System.Windows.Forms.TextBox XYZ_X;
        private System.Windows.Forms.TextBox XYZ_Y;
        private System.Windows.Forms.TextBox XYZ_Z;
        private System.Windows.Forms.TextBox LCHab_L;
        private System.Windows.Forms.TextBox LCHab_C;
        private System.Windows.Forms.TextBox LCHab_H;
        private System.Windows.Forms.TextBox LCHuv_L;
        private System.Windows.Forms.TextBox LCHuv_C;
        private System.Windows.Forms.TextBox LCHuv_H;
        private System.Windows.Forms.TextBox Luv_L;
        private System.Windows.Forms.TextBox Luv_u;
        private System.Windows.Forms.TextBox Luv_v;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button RGB_Button;
        private System.Windows.Forms.Button Lab_Button;
        private System.Windows.Forms.Button XYZ_Button;
        private System.Windows.Forms.Button LCHab_Button;
        private System.Windows.Forms.Button LCHuv_Button;
        private System.Windows.Forms.Button Luv_Button;
        private System.Windows.Forms.ComboBox General_SpaceDrDo;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.TextBox Yxy_Y;
        private System.Windows.Forms.TextBox Yxy_x;
        private System.Windows.Forms.TextBox Yxy_sy;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button Yxy_Button;
        private System.Windows.Forms.TextBox HSV_H;
        private System.Windows.Forms.TextBox HSV_S;
        private System.Windows.Forms.TextBox HSV_V;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button HSV_Button;
        private System.Windows.Forms.TextBox HSL_H;
        private System.Windows.Forms.TextBox HSL_S;
        private System.Windows.Forms.TextBox HSL_L;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button HSL_Button;
        private System.Windows.Forms.Panel ColorPanel;
        private System.Windows.Forms.Label RGB_Label;
        private System.Windows.Forms.Label Hex_Label;
        private System.Windows.Forms.ComboBox RefWhiteDrDo;
        private System.Windows.Forms.ColorDialog ColorSelectDialog;
        private System.Windows.Forms.OpenFileDialog iccOpenDialog;
        private System.Windows.Forms.TextBox CMY_C;
        private System.Windows.Forms.TextBox CMY_M;
        private System.Windows.Forms.TextBox CMY_Y;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button CMY_Button;
        private System.Windows.Forms.TextBox CMYK_C;
        private System.Windows.Forms.TextBox CMYK_M;
        private System.Windows.Forms.TextBox CMYK_Y;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button CMYK_Button;
        private System.Windows.Forms.TextBox CMYK_K;
        private System.Windows.Forms.TextBox CMY_ICCbox;
        private System.Windows.Forms.TextBox CMYK_ICCbox;
        private System.Windows.Forms.TextBox YCbCr_Y;
        private System.Windows.Forms.TextBox YCbCr_Cb;
        private System.Windows.Forms.TextBox YCbCr_Cr;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button YCbCr_Button;
        private System.Windows.Forms.TextBox YCbCr_ICCbox;
        private System.Windows.Forms.TextBox Gray_G;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button Gray_Button;
        private System.Windows.Forms.TextBox Gray_ICCbox;
        private System.Windows.Forms.TextBox XColor_X;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button XColor_Button;
        private System.Windows.Forms.TextBox XColor_ICCbox;
        private System.Windows.Forms.NumericUpDown XColor_ChannelUpDo;
        private System.Windows.Forms.ComboBox HSV_CoBox;
        private System.Windows.Forms.TextBox HSV_ICCbox;
        private System.Windows.Forms.ComboBox HSL_CoBox;
        private System.Windows.Forms.TextBox HSL_ICCbox;
        private System.Windows.Forms.Button CMY_ChICC;
        private System.Windows.Forms.Button CMYK_ChICC;
        private System.Windows.Forms.Button Gray_ChICC;
        private System.Windows.Forms.Button ColorX_ChICC;
        private System.Windows.Forms.ComboBox Lab_CoBox;
        private System.Windows.Forms.ComboBox LCHab_CoBox;
        private System.Windows.Forms.ComboBox LCHuv_CoBox;
        private System.Windows.Forms.ComboBox Luv_CoBox;
        private System.Windows.Forms.ComboBox Yxy_CoBox;
        private System.Windows.Forms.ComboBox XYZ_CoBox;
        private System.Windows.Forms.ComboBox RGB_CoBox;
        private System.Windows.Forms.TextBox RGB_ICCbox;
        private System.Windows.Forms.TextBox XColor_Channel;
        private System.Windows.Forms.ComboBox YCbCr_CoBox;
        private System.Windows.Forms.ComboBox RenderIntentCoBox;
        private System.Windows.Forms.ComboBox ChroAdaptCoBox;
        private System.Windows.Forms.ToolTip MainToolTip;
        private System.Windows.Forms.TextBox DescriptionBox;
        private System.Windows.Forms.TextBox LCH99_L;
        private System.Windows.Forms.TextBox LCH99_C;
        private System.Windows.Forms.TextBox LCH99_H;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button LCH99_Button;
        private System.Windows.Forms.TextBox LCH99b_L;
        private System.Windows.Forms.TextBox LCH99b_C;
        private System.Windows.Forms.TextBox LCH99b_H;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button LCH99b_Button;
        private System.Windows.Forms.TextBox LCH99c_L;
        private System.Windows.Forms.TextBox LCH99c_C;
        private System.Windows.Forms.TextBox LCH99c_H;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button LCH99c_Button;
        private System.Windows.Forms.TextBox LCH99d_L;
        private System.Windows.Forms.TextBox LCH99d_C;
        private System.Windows.Forms.TextBox LCH99d_H;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button LCH99d_Button;
        private System.Windows.Forms.TextBox DEF_D;
        private System.Windows.Forms.TextBox DEF_E;
        private System.Windows.Forms.TextBox DEF_F;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button DEF_Button;
        private System.Windows.Forms.TextBox Bef_B;
        private System.Windows.Forms.TextBox Bef_e;
        private System.Windows.Forms.TextBox Bef_f;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button Bef_Button;
        private System.Windows.Forms.TextBox BCH_B;
        private System.Windows.Forms.TextBox BCH_C;
        private System.Windows.Forms.TextBox BCH_H;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button BCH_Button;
        private System.Windows.Forms.ComboBox DEF_CoBox;
        private System.Windows.Forms.ComboBox Bef_CoBox;
        private System.Windows.Forms.ComboBox BCH_CoBox;
    }
}

