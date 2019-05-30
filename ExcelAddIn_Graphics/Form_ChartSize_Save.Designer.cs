namespace ExcelAddIn_Graphics
{
    partial class Form_ChartSize
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_ChartHeight = new System.Windows.Forms.TextBox();
            this.textBox_ChartWidth = new System.Windows.Forms.TextBox();
            this.textBox_PlotAreaHeight = new System.Windows.Forms.TextBox();
            this.textBox_PlotAreaWidth = new System.Windows.Forms.TextBox();
            this.button_OK = new System.Windows.Forms.Button();
            this.checkBox_AllChart = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(36, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "图表高度:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(36, 62);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "图表宽度:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(34, 145);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "绘图区宽度:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(34, 104);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "绘图区高度:";
            // 
            // textBox_ChartHeight
            // 
            this.textBox_ChartHeight.Location = new System.Drawing.Point(118, 22);
            this.textBox_ChartHeight.Name = "textBox_ChartHeight";
            this.textBox_ChartHeight.Size = new System.Drawing.Size(66, 26);
            this.textBox_ChartHeight.TabIndex = 4;
            this.textBox_ChartHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_ChartWidth
            // 
            this.textBox_ChartWidth.Location = new System.Drawing.Point(118, 61);
            this.textBox_ChartWidth.Name = "textBox_ChartWidth";
            this.textBox_ChartWidth.Size = new System.Drawing.Size(66, 26);
            this.textBox_ChartWidth.TabIndex = 5;
            this.textBox_ChartWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_PlotAreaHeight
            // 
            this.textBox_PlotAreaHeight.Location = new System.Drawing.Point(118, 101);
            this.textBox_PlotAreaHeight.Name = "textBox_PlotAreaHeight";
            this.textBox_PlotAreaHeight.Size = new System.Drawing.Size(66, 26);
            this.textBox_PlotAreaHeight.TabIndex = 6;
            this.textBox_PlotAreaHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_PlotAreaWidth
            // 
            this.textBox_PlotAreaWidth.Location = new System.Drawing.Point(118, 139);
            this.textBox_PlotAreaWidth.Name = "textBox_PlotAreaWidth";
            this.textBox_PlotAreaWidth.Size = new System.Drawing.Size(66, 26);
            this.textBox_PlotAreaWidth.TabIndex = 7;
            this.textBox_PlotAreaWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(118, 183);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(66, 35);
            this.button_OK.TabIndex = 8;
            this.button_OK.Text = "确定";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // checkBox_AllChart
            // 
            this.checkBox_AllChart.AutoSize = true;
            this.checkBox_AllChart.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_AllChart.Location = new System.Drawing.Point(37, 192);
            this.checkBox_AllChart.Name = "checkBox_AllChart";
            this.checkBox_AllChart.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBox_AllChart.Size = new System.Drawing.Size(75, 21);
            this.checkBox_AllChart.TabIndex = 9;
            this.checkBox_AllChart.Text = "所有图表";
            this.checkBox_AllChart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_AllChart.UseVisualStyleBackColor = true;
            // 
            // Form_ChartSize
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 237);
            this.Controls.Add(this.checkBox_AllChart);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.textBox_PlotAreaWidth);
            this.Controls.Add(this.textBox_PlotAreaHeight);
            this.Controls.Add(this.textBox_ChartWidth);
            this.Controls.Add(this.textBox_ChartHeight);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form_ChartSize";
            this.Text = "图表尺寸";
            this.Load += new System.EventHandler(this.Form_ChartSize_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_ChartHeight;
        private System.Windows.Forms.TextBox textBox_ChartWidth;
        private System.Windows.Forms.TextBox textBox_PlotAreaHeight;
        private System.Windows.Forms.TextBox textBox_PlotAreaWidth;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.CheckBox checkBox_AllChart;
    }
}