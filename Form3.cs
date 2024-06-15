using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastReport.DataVisualization.Charting;

namespace ProjectCA3
{
    public partial class Form3 : Form
    {
        private bool _dragging = false;
        private Point _start_point = new Point(0, 0);


        List<string> departList = new List<string>();
        List<int> totalWageList = new List<int>();
        public Form3(DataGridView dataGridView)
        {
            InitializeComponent();
            GenerateList(dataGridView);
            MakeChart();
        }
        private void MakeChart()
        {
            // Create and configure the pie chart
            Chart pieChart = new Chart();
            pieChart.Dock = DockStyle.Fill;
            this.Controls.Add(pieChart);
            ChartArea chartArea = new ChartArea();
            Series series = new Series();
            series.ChartType = SeriesChartType.Pie;
            pieChart.ChartAreas.Add(chartArea);

            // Add data to the pie chart
            for (int i = 0; i < departList.Count; i++)
            {
                series.Points.AddXY((i+1).ToString(), totalWageList[i]);
            }
            pieChart.Series.Add(series);

            // Create and configure the color marking table (DataGridView)
            DataGridView colorTable = new DataGridView();
            colorTable.Dock = DockStyle.Right; // or any other dock style you prefer
            colorTable.AutoGenerateColumns = false;
            colorTable.RowHeadersVisible = false;

            // Define columns for the color marking table
            DataGridViewTextBoxColumn departmentColumn = new DataGridViewTextBoxColumn();
            departmentColumn.HeaderText = "Department";
            departmentColumn.DataPropertyName = "Department";
            colorTable.Columns.Add(departmentColumn);

            DataGridViewTextBoxColumn colorColumn = new DataGridViewTextBoxColumn();
            colorColumn.HeaderText = "Color";
            colorColumn.DataPropertyName = "Color";
            colorTable.Columns.Add(colorColumn);

            // Add data to the color marking table
            for (int i = 0; i < departList.Count; i++)
            {
                colorTable.Rows.Add(departList[i], (i+1).ToString());
            }
            // Add the color marking table to the form
            this.Controls.Add(colorTable);
        }

        private Color GetRandomColor()
        {
            Random random = new Random();
            return Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        }

        private void GenerateList(DataGridView dataGridView)
        {
            for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
            {
                string department = dataGridView.Rows[i].Cells[6].Value.ToString();
                if (!departList.Contains(department))
                {
                    departList.Add(department);
                    totalWageList.Add(0);
                }
            }

            for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
            {
                string employeeDepartment = dataGridView.Rows[i].Cells[6].Value.ToString();
                for (int j = 0; j < departList.Count; j++)
                {
                    if (departList[j] == employeeDepartment)
                    {
                        totalWageList[j] += Convert.ToInt32(dataGridView.Rows[i].Cells[8].Value) * Convert.ToInt32(dataGridView.Rows[i].Cells[9].Value);
                    }
                }
            }
        }
    }
}
