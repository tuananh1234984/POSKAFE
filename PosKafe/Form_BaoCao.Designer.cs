namespace PosKafe
{
    partial class Form_BaoCao
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtpkFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtpkToDate = new System.Windows.Forms.DateTimePicker();
            this.btnThongKe = new System.Windows.Forms.Button();
            this.dtgvBill = new System.Windows.Forms.DataGridView();
            this.chartRevenue = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.tabPageDoanhThu = new MetroFramework.Controls.MetroTabControl();
            this.metroTabPage1 = new MetroFramework.Controls.MetroTabPage();
            this.metroTabPage2 = new MetroFramework.Controls.MetroTabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.ChartBestSeller = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblMonHotNhat = new System.Windows.Forms.Label();
            this.dtgvBestSeller = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvBill)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenue)).BeginInit();
            this.tabPageDoanhThu.SuspendLayout();
            this.metroTabPage1.SuspendLayout();
            this.metroTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChartBestSeller)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvBestSeller)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.btnXuatExcel);
            this.panel1.Controls.Add(this.btnThongKe);
            this.panel1.Controls.Add(this.dtpkToDate);
            this.panel1.Controls.Add(this.dtpkFromDate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(15, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1398, 100);
            this.panel1.TabIndex = 0;
            // 
            // dtpkFromDate
            // 
            this.dtpkFromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpkFromDate.Location = new System.Drawing.Point(274, 39);
            this.dtpkFromDate.Name = "dtpkFromDate";
            this.dtpkFromDate.Size = new System.Drawing.Size(192, 24);
            this.dtpkFromDate.TabIndex = 0;
            // 
            // dtpkToDate
            // 
            this.dtpkToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpkToDate.Location = new System.Drawing.Point(596, 38);
            this.dtpkToDate.Name = "dtpkToDate";
            this.dtpkToDate.Size = new System.Drawing.Size(200, 24);
            this.dtpkToDate.TabIndex = 1;
            // 
            // btnThongKe
            // 
            this.btnThongKe.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThongKe.Location = new System.Drawing.Point(907, 36);
            this.btnThongKe.Name = "btnThongKe";
            this.btnThongKe.Size = new System.Drawing.Size(103, 27);
            this.btnThongKe.TabIndex = 2;
            this.btnThongKe.Text = "Thống kê";
            this.btnThongKe.UseVisualStyleBackColor = true;
            this.btnThongKe.Click += new System.EventHandler(this.btnThongKe_Click);
            // 
            // dtgvBill
            // 
            this.dtgvBill.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvBill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgvBill.Location = new System.Drawing.Point(0, 0);
            this.dtgvBill.Name = "dtgvBill";
            this.dtgvBill.Size = new System.Drawing.Size(557, 657);
            this.dtgvBill.TabIndex = 0;
            // 
            // chartRevenue
            // 
            chartArea3.Name = "ChartArea1";
            this.chartRevenue.ChartAreas.Add(chartArea3);
            this.chartRevenue.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.chartRevenue.Legends.Add(legend3);
            this.chartRevenue.Location = new System.Drawing.Point(0, 0);
            this.chartRevenue.Name = "chartRevenue";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartRevenue.Series.Add(series3);
            this.chartRevenue.Size = new System.Drawing.Size(829, 657);
            this.chartRevenue.TabIndex = 0;
            this.chartRevenue.Text = "chart1";
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXuatExcel.Location = new System.Drawing.Point(1067, 35);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(102, 28);
            this.btnXuatExcel.TabIndex = 3;
            this.btnXuatExcel.Text = "Xuất Báo Cáo";
            this.btnXuatExcel.UseVisualStyleBackColor = true;
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);
            // 
            // tabPageDoanhThu
            // 
            this.tabPageDoanhThu.Controls.Add(this.metroTabPage1);
            this.tabPageDoanhThu.Controls.Add(this.metroTabPage2);
            this.tabPageDoanhThu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPageDoanhThu.Location = new System.Drawing.Point(15, 124);
            this.tabPageDoanhThu.Name = "tabPageDoanhThu";
            this.tabPageDoanhThu.SelectedIndex = 1;
            this.tabPageDoanhThu.Size = new System.Drawing.Size(1398, 699);
            this.tabPageDoanhThu.TabIndex = 4;
            this.tabPageDoanhThu.UseSelectable = true;
            // 
            // metroTabPage1
            // 
            this.metroTabPage1.Controls.Add(this.splitContainer1);
            this.metroTabPage1.HorizontalScrollbarBarColor = true;
            this.metroTabPage1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.HorizontalScrollbarSize = 10;
            this.metroTabPage1.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage1.Name = "metroTabPage1";
            this.metroTabPage1.Size = new System.Drawing.Size(1390, 657);
            this.metroTabPage1.TabIndex = 0;
            this.metroTabPage1.Text = "Doanh Thu";
            this.metroTabPage1.VerticalScrollbarBarColor = true;
            this.metroTabPage1.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.VerticalScrollbarSize = 10;
            // 
            // metroTabPage2
            // 
            this.metroTabPage2.Controls.Add(this.splitContainer2);
            this.metroTabPage2.HorizontalScrollbarBarColor = true;
            this.metroTabPage2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.HorizontalScrollbarSize = 10;
            this.metroTabPage2.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage2.Name = "metroTabPage2";
            this.metroTabPage2.Size = new System.Drawing.Size(1390, 657);
            this.metroTabPage2.TabIndex = 1;
            this.metroTabPage2.Text = "Món Bán Chạy";
            this.metroTabPage2.VerticalScrollbarBarColor = true;
            this.metroTabPage2.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.VerticalScrollbarSize = 10;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chartRevenue);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dtgvBill);
            this.splitContainer1.Size = new System.Drawing.Size(1390, 657);
            this.splitContainer1.SplitterDistance = 829;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.ChartBestSeller);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lblMonHotNhat);
            this.splitContainer2.Panel2.Controls.Add(this.dtgvBestSeller);
            this.splitContainer2.Size = new System.Drawing.Size(1390, 657);
            this.splitContainer2.SplitterDistance = 500;
            this.splitContainer2.TabIndex = 2;
            // 
            // ChartBestSeller
            // 
            chartArea4.Name = "ChartArea1";
            this.ChartBestSeller.ChartAreas.Add(chartArea4);
            this.ChartBestSeller.Dock = System.Windows.Forms.DockStyle.Fill;
            legend4.Name = "Legend1";
            this.ChartBestSeller.Legends.Add(legend4);
            this.ChartBestSeller.Location = new System.Drawing.Point(0, 0);
            this.ChartBestSeller.Name = "ChartBestSeller";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.ChartBestSeller.Series.Add(series4);
            this.ChartBestSeller.Size = new System.Drawing.Size(500, 657);
            this.ChartBestSeller.TabIndex = 0;
            this.ChartBestSeller.Text = "chart1";
            // 
            // lblMonHotNhat
            // 
            this.lblMonHotNhat.AutoSize = true;
            this.lblMonHotNhat.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMonHotNhat.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonHotNhat.ForeColor = System.Drawing.Color.Red;
            this.lblMonHotNhat.Location = new System.Drawing.Point(0, 0);
            this.lblMonHotNhat.Name = "lblMonHotNhat";
            this.lblMonHotNhat.Size = new System.Drawing.Size(181, 20);
            this.lblMonHotNhat.TabIndex = 0;
            this.lblMonHotNhat.Text = "Món bán chạy nhất:...";
            // 
            // dtgvBestSeller
            // 
            this.dtgvBestSeller.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtgvBestSeller.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvBestSeller.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgvBestSeller.Location = new System.Drawing.Point(0, 0);
            this.dtgvBestSeller.Name = "dtgvBestSeller";
            this.dtgvBestSeller.Size = new System.Drawing.Size(886, 657);
            this.dtgvBestSeller.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 100);
            this.panel2.TabIndex = 4;
            // 
            // Form_BaoCao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1428, 839);
            this.Controls.Add(this.tabPageDoanhThu);
            this.Controls.Add(this.panel1);
            this.DisplayHeader = false;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form_BaoCao";
            this.Padding = new System.Windows.Forms.Padding(15, 24, 15, 16);
            this.Text = "Form_BaoCao";
            this.Load += new System.EventHandler(this.Form_BaoCao_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgvBill)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenue)).EndInit();
            this.tabPageDoanhThu.ResumeLayout(false);
            this.metroTabPage1.ResumeLayout(false);
            this.metroTabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ChartBestSeller)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvBestSeller)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnThongKe;
        private System.Windows.Forms.DateTimePicker dtpkToDate;
        private System.Windows.Forms.DateTimePicker dtpkFromDate;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRevenue;
        private System.Windows.Forms.DataGridView dtgvBill;
        private System.Windows.Forms.Button btnXuatExcel;
        private MetroFramework.Controls.MetroTabControl tabPageDoanhThu;
        private MetroFramework.Controls.MetroTabPage metroTabPage1;
        private MetroFramework.Controls.MetroTabPage metroTabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataVisualization.Charting.Chart ChartBestSeller;
        private System.Windows.Forms.DataGridView dtgvBestSeller;
        private System.Windows.Forms.Label lblMonHotNhat;
        private System.Windows.Forms.Panel panel2;
    }
}