using System;
using System.Windows.Forms;

namespace ExcelAddIn_Graphics
{
    partial class Form_GetData
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
            this.pictureBox_Data = new System.Windows.Forms.PictureBox();
            this.button_ReadImage = new System.Windows.Forms.Button();
            this.button_GetData = new System.Windows.Forms.Button();
            this.button_LableData = new System.Windows.Forms.Button();
            this.checkBox_Marker = new System.Windows.Forms.CheckBox();
            this.checkBox_Line = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox__Yaxismax = new System.Windows.Forms.TextBox();
            this.textBox__Yaxismin = new System.Windows.Forms.TextBox();
            this.textBox__Xaxismax = new System.Windows.Forms.TextBox();
            this.textBox__Xaxismin = new System.Windows.Forms.TextBox();
            this.button__Yaxismax = new System.Windows.Forms.Button();
            this.button_Yaxismin = new System.Windows.Forms.Button();
            this.button_Xaxismax = new System.Windows.Forms.Button();
            this.button_Xaxismin = new System.Windows.Forms.Button();
            this.pictureBox_LocalRegion = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBox_Threshold = new System.Windows.Forms.TextBox();
            this.textBox_Step = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_End = new System.Windows.Forms.Button();
            this.button_Start = new System.Windows.Forms.Button();
            this.button_Automatic = new System.Windows.Forms.Button();
            this.button_GetData2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Data)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LocalRegion)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox_Data
            // 
            this.pictureBox_Data.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox_Data.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_Data.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pictureBox_Data.Location = new System.Drawing.Point(16, 12);
            this.pictureBox_Data.Name = "pictureBox_Data";
            this.pictureBox_Data.Size = new System.Drawing.Size(720, 547);
            this.pictureBox_Data.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Data.TabIndex = 0;
            this.pictureBox_Data.TabStop = false;
            this.pictureBox_Data.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox_Data_MouseDown);
            this.pictureBox_Data.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox_Data_MouseMove);
            // 
            // button_ReadImage
            // 
            this.button_ReadImage.Location = new System.Drawing.Point(751, 12);
            this.button_ReadImage.Name = "button_ReadImage";
            this.button_ReadImage.Size = new System.Drawing.Size(160, 38);
            this.button_ReadImage.TabIndex = 1;
            this.button_ReadImage.Text = "导入图像";
            this.button_ReadImage.UseVisualStyleBackColor = true;
            this.button_ReadImage.Click += new System.EventHandler(this.button_ReadImage_Click);
            // 
            // button_GetData
            // 
            this.button_GetData.Location = new System.Drawing.Point(23, 97);
            this.button_GetData.Name = "button_GetData";
            this.button_GetData.Size = new System.Drawing.Size(102, 37);
            this.button_GetData.TabIndex = 2;
            this.button_GetData.Text = "数据输出";
            this.button_GetData.UseVisualStyleBackColor = true;
            this.button_GetData.Click += new System.EventHandler(this.button_GetData_Click);
            // 
            // button_LableData
            // 
            this.button_LableData.Location = new System.Drawing.Point(23, 56);
            this.button_LableData.Name = "button_LableData";
            this.button_LableData.Size = new System.Drawing.Size(102, 37);
            this.button_LableData.TabIndex = 3;
            this.button_LableData.Text = "数据标记";
            this.button_LableData.UseVisualStyleBackColor = true;
            this.button_LableData.Click += new System.EventHandler(this.button_LableData_Click);
            // 
            // checkBox_Marker
            // 
            this.checkBox_Marker.AutoSize = true;
            this.checkBox_Marker.Location = new System.Drawing.Point(20, 34);
            this.checkBox_Marker.Name = "checkBox_Marker";
            this.checkBox_Marker.Size = new System.Drawing.Size(108, 16);
            this.checkBox_Marker.TabIndex = 1;
            this.checkBox_Marker.Text = "无直线的标记点";
            this.checkBox_Marker.UseVisualStyleBackColor = true;
            this.checkBox_Marker.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox_Line
            // 
            this.checkBox_Line.AutoSize = true;
            this.checkBox_Line.Checked = true;
            this.checkBox_Line.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Line.Location = new System.Drawing.Point(20, 12);
            this.checkBox_Line.Name = "checkBox_Line";
            this.checkBox_Line.Size = new System.Drawing.Size(108, 16);
            this.checkBox_Line.TabIndex = 0;
            this.checkBox_Line.Text = "带直线的标记点";
            this.checkBox_Line.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox__Yaxismax);
            this.groupBox2.Controls.Add(this.textBox__Yaxismin);
            this.groupBox2.Controls.Add(this.textBox__Xaxismax);
            this.groupBox2.Controls.Add(this.textBox__Xaxismin);
            this.groupBox2.Controls.Add(this.button__Yaxismax);
            this.groupBox2.Controls.Add(this.button_Yaxismin);
            this.groupBox2.Controls.Add(this.button_Xaxismax);
            this.groupBox2.Controls.Add(this.button_Xaxismin);
            this.groupBox2.Location = new System.Drawing.Point(751, 56);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(160, 171);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "坐标选定";
            // 
            // textBox__Yaxismax
            // 
            this.textBox__Yaxismax.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox__Yaxismax.Location = new System.Drawing.Point(12, 137);
            this.textBox__Yaxismax.Name = "textBox__Yaxismax";
            this.textBox__Yaxismax.Size = new System.Drawing.Size(48, 26);
            this.textBox__Yaxismax.TabIndex = 11;
            this.textBox__Yaxismax.Text = "1.0";
            this.textBox__Yaxismax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox__Yaxismin
            // 
            this.textBox__Yaxismin.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox__Yaxismin.Location = new System.Drawing.Point(12, 98);
            this.textBox__Yaxismin.Name = "textBox__Yaxismin";
            this.textBox__Yaxismin.Size = new System.Drawing.Size(48, 26);
            this.textBox__Yaxismin.TabIndex = 10;
            this.textBox__Yaxismin.Text = "0.0";
            this.textBox__Yaxismin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox__Xaxismax
            // 
            this.textBox__Xaxismax.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox__Xaxismax.Location = new System.Drawing.Point(12, 62);
            this.textBox__Xaxismax.Name = "textBox__Xaxismax";
            this.textBox__Xaxismax.Size = new System.Drawing.Size(48, 26);
            this.textBox__Xaxismax.TabIndex = 9;
            this.textBox__Xaxismax.Text = "1.0";
            this.textBox__Xaxismax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox__Xaxismin
            // 
            this.textBox__Xaxismin.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox__Xaxismin.Location = new System.Drawing.Point(12, 23);
            this.textBox__Xaxismin.Name = "textBox__Xaxismin";
            this.textBox__Xaxismin.Size = new System.Drawing.Size(48, 26);
            this.textBox__Xaxismin.TabIndex = 8;
            this.textBox__Xaxismin.Text = "0.0";
            this.textBox__Xaxismin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button__Yaxismax
            // 
            this.button__Yaxismax.Location = new System.Drawing.Point(66, 135);
            this.button__Yaxismax.Name = "button__Yaxismax";
            this.button__Yaxismax.Size = new System.Drawing.Size(82, 29);
            this.button__Yaxismax.TabIndex = 7;
            this.button__Yaxismax.Text = "Y轴 Max";
            this.button__Yaxismax.UseVisualStyleBackColor = true;
            this.button__Yaxismax.Click += new System.EventHandler(this.button__Yaxismax_Click);
            // 
            // button_Yaxismin
            // 
            this.button_Yaxismin.Location = new System.Drawing.Point(66, 95);
            this.button_Yaxismin.Name = "button_Yaxismin";
            this.button_Yaxismin.Size = new System.Drawing.Size(82, 29);
            this.button_Yaxismin.TabIndex = 6;
            this.button_Yaxismin.Text = "Y轴 Min";
            this.button_Yaxismin.UseVisualStyleBackColor = true;
            this.button_Yaxismin.Click += new System.EventHandler(this.button_Yaxismin_Click);
            // 
            // button_Xaxismax
            // 
            this.button_Xaxismax.Location = new System.Drawing.Point(66, 58);
            this.button_Xaxismax.Name = "button_Xaxismax";
            this.button_Xaxismax.Size = new System.Drawing.Size(82, 29);
            this.button_Xaxismax.TabIndex = 5;
            this.button_Xaxismax.Text = "X轴 Max";
            this.button_Xaxismax.UseVisualStyleBackColor = true;
            this.button_Xaxismax.Click += new System.EventHandler(this.button_Xaxismax_Click);
            // 
            // button_Xaxismin
            // 
            this.button_Xaxismin.Location = new System.Drawing.Point(66, 22);
            this.button_Xaxismin.Name = "button_Xaxismin";
            this.button_Xaxismin.Size = new System.Drawing.Size(82, 29);
            this.button_Xaxismin.TabIndex = 4;
            this.button_Xaxismin.Text = "X轴 Min";
            this.button_Xaxismin.UseVisualStyleBackColor = true;
            this.button_Xaxismin.Click += new System.EventHandler(this.button_Xaxismin_Click);
            // 
            // pictureBox_LocalRegion
            // 
            this.pictureBox_LocalRegion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pictureBox_LocalRegion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_LocalRegion.Location = new System.Drawing.Point(751, 409);
            this.pictureBox_LocalRegion.Name = "pictureBox_LocalRegion";
            this.pictureBox_LocalRegion.Size = new System.Drawing.Size(160, 150);
            this.pictureBox_LocalRegion.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_LocalRegion.TabIndex = 5;
            this.pictureBox_LocalRegion.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(751, 233);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(160, 170);
            this.tabControl1.TabIndex = 6;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage1.Controls.Add(this.button_LableData);
            this.tabPage1.Controls.Add(this.checkBox_Marker);
            this.tabPage1.Controls.Add(this.checkBox_Line);
            this.tabPage1.Controls.Add(this.button_GetData);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(152, 144);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Tag = "1";
            this.tabPage1.Text = "人工拾取";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage2.Controls.Add(this.textBox_Threshold);
            this.tabPage2.Controls.Add(this.textBox_Step);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.button_End);
            this.tabPage2.Controls.Add(this.button_Start);
            this.tabPage2.Controls.Add(this.button_Automatic);
            this.tabPage2.Controls.Add(this.button_GetData2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(152, 144);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Tag = "2";
            this.tabPage2.Text = "自动拾取";
            // 
            // textBox_Threshold
            // 
            this.textBox_Threshold.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_Threshold.Location = new System.Drawing.Point(77, 76);
            this.textBox_Threshold.Name = "textBox_Threshold";
            this.textBox_Threshold.Size = new System.Drawing.Size(69, 26);
            this.textBox_Threshold.TabIndex = 13;
            this.textBox_Threshold.Text = "50";
            this.textBox_Threshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_Threshold.TextChanged += new System.EventHandler(this.textBox_Threshold_TextChanged);
            // 
            // textBox_Step
            // 
            this.textBox_Step.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_Step.Location = new System.Drawing.Point(77, 44);
            this.textBox_Step.Name = "textBox_Step";
            this.textBox_Step.Size = new System.Drawing.Size(69, 26);
            this.textBox_Step.TabIndex = 12;
            this.textBox_Step.Text = "3";
            this.textBox_Step.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_Step.TextChanged += new System.EventHandler(this.textBox_Step_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(10, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "取点阈值";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(10, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "取点步长";
            // 
            // button_End
            // 
            this.button_End.Location = new System.Drawing.Point(77, 6);
            this.button_End.Name = "button_End";
            this.button_End.Size = new System.Drawing.Size(69, 32);
            this.button_End.TabIndex = 9;
            this.button_End.Text = "终点标记";
            this.button_End.UseVisualStyleBackColor = true;
            this.button_End.Click += new System.EventHandler(this.button_End_Click);
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(6, 6);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(65, 32);
            this.button_Start.TabIndex = 8;
            this.button_Start.Text = "始点标记";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // button_Automatic
            // 
            this.button_Automatic.Location = new System.Drawing.Point(6, 109);
            this.button_Automatic.Name = "button_Automatic";
            this.button_Automatic.Size = new System.Drawing.Size(69, 32);
            this.button_Automatic.TabIndex = 5;
            this.button_Automatic.Text = "自动标记";
            this.button_Automatic.UseVisualStyleBackColor = true;
            this.button_Automatic.Click += new System.EventHandler(this.button_Automatic_Click);
            // 
            // button_GetData2
            // 
            this.button_GetData2.Location = new System.Drawing.Point(81, 109);
            this.button_GetData2.Name = "button_GetData2";
            this.button_GetData2.Size = new System.Drawing.Size(65, 32);
            this.button_GetData2.TabIndex = 4;
            this.button_GetData2.Text = "数据输出";
            this.button_GetData2.UseVisualStyleBackColor = true;
            this.button_GetData2.Click += new System.EventHandler(this.button_GetData2_Click);
            // 
            // Form_GetData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 574);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pictureBox_LocalRegion);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button_ReadImage);
            this.Controls.Add(this.pictureBox_Data);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form_GetData";
            this.Text = "图表原始数据拾取";
            this.Load += new System.EventHandler(this.Form_GetData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Data)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LocalRegion)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        //private void PictureBox_Data_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        //{
        //    //throw new System.NotImplementedException();
        //}

        //private void PictureBox_Data_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        //{
        //    throw new System.NotImplementedException();
        //}

        //private void PictureBox_Data_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        //{
        //    throw new System.NotImplementedException();
        //}

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_Data;
        private System.Windows.Forms.Button button_ReadImage;
        private System.Windows.Forms.Button button_GetData;
        private System.Windows.Forms.CheckBox checkBox_Marker;
        private System.Windows.Forms.CheckBox checkBox_Line;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox__Yaxismax;
        private System.Windows.Forms.TextBox textBox__Yaxismin;
        private System.Windows.Forms.TextBox textBox__Xaxismax;
        private System.Windows.Forms.TextBox textBox__Xaxismin;
        private System.Windows.Forms.Button button__Yaxismax;
        private System.Windows.Forms.Button button_Yaxismin;
        private System.Windows.Forms.Button button_Xaxismax;
        private System.Windows.Forms.Button button_Xaxismin;
        private System.Windows.Forms.Button button_LableData;
        private System.Windows.Forms.PictureBox pictureBox_LocalRegion;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button button_End;
        private Button button_Start;
        private Button button_Automatic;
        private Button button_GetData2;
        private TextBox textBox_Threshold;
        private TextBox textBox_Step;
        private Label label2;
        private Label label1;
    }
}