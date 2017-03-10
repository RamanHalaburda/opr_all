using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OPR1
{
    public partial class Form1 : Form
    {
        public Form1(){ InitializeComponent(); }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            comboBox1.SelectedIndex = 0;
        }

        double fun(double _x1, double _x2)
        {
            return (-6 * _x1 + Math.Pow(_x2, 2) - 2 * _x1 * _x2 + Math.Pow(_x2, 2));
        }

        Boolean condition(double _x1, double _x2)
        {
            if (_x1 + _x2 <= 2 && _x1 >= 0 && _x2 >= 0)
            {
                return true;
            }
            return false;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        clearInterface();
                        methodDirectThroughGrid();
                        break;
                    case 1:
                        clearInterface();
                        methodMonteCarlo();
                        break;
                    case 2:
                        clearInterface();
                        MessageBox.Show("Этот метод еще не реализован!");
                        break;
                    case 3:
                        clearInterface();
                        MessageBox.Show("Этот метод еще не реализован!");
                        break;
                    default:
                        MessageBox.Show("Error!");
                        break;
                }
            }
            catch (Exception exx) { MessageBox.Show(""+exx); }
        }

/*
    +-----------------------------+
    | direct-through-grid method  |
    +-----------------------------+
*/

        private double h = 0.5;

        private void clearInterface()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 0;
            chart1.Series.Clear();
            chart1.Series.Add("Значения");
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            chart1.Series.Add("Экстремум");
            chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            chart1.Series.Add("Усл. Экстремум");
            chart1.Series[2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            chart3.Series.Clear();
            chart3.Series.Add("11");
            chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart3.Series.Add("22");
            chart3.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart3.Series.Add("33");
            chart3.Series[2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
        }

        private void methodDirectThroughGrid()
        {
            dataGridView1.RowCount = 20; dataGridView1.ColumnCount = 20;
            double h = 0.5,
                x = 0,
                y = 0,
                min = fun(0, 0);
            int n = 11, k = 0, l = 0;
            bool[,] flag = new bool[n, n];
            for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) flag[i, j] = false;
            for (double x1 = -1; x1 <= 4; x1 += h)
            {
                l = 0;
                for (double x2 = -1; x2 <= 4; x2 += h)
                {
                    if (condition(x1,x2))
                    {
                        double res = fun(x1, x2);
                        if (min > res)
                        {
                            min = res;
                            x = x1; y = x2;
                        }
                        flag[k, l] = true;
                        dataGridView1[k, l].Value = res;
                    }
                    l++;
                }
                k++;
            }
            double extr = min;
            chart1.Series[1].Points.AddXY(x, y);
            textBox1.Visible = true;
            textBox1.Text = "Минимум: " + Convert.ToString(Math.Round(min, 2));
            k = 0;
            for (double x1 = -1; x1 <= 4; x1 += h)
            {
                l = 0;
                for (double x2 = -1; x2 <= 4; x2 += h)
                {
                    if (flag[k, l] == true)
                    {
                        chart1.Series[0].Points.AddXY(x1, x2);
                    }
                    l++;
                }
                k++;
            }
            min = fun(0, 0);
            for (double x1 = -1; x1 <= 4; x1 += h)
            {
                l = 0;
                for (double x2 = -1; x2 <= 4; x2 += h)
                {
                    if (x1 + x2 <= 2 && x1 >= 0 && x2 >= 0)
                    {
                        double res = fun(x1, x2);
                        if (res != extr)
                        {
                            if (min > res)
                            {
                                min = res;
                                x = x1; y = x2;
                            }
                        }
                    }
                    l++;
                }
                k++;
            } chart1.Series[2].Points.AddXY(x, y);

            for (double x1 = -1; x1 <= 4; x1 += h)
            {
                l = 0;
                for (double x2 = -1; x2 <= 4; x2 += h)
                {
                    if (condition(x1, x2))
                    {
                        double res = fun(x1, x2);
                        if (res >= -6.1 && res <= -5.9)
                        {
                            chart3.Series[0].Points.AddXY(x1, x2);
                        }
                        if (res >= -0.1 && res <= 0.1)
                        {
                            chart3.Series[1].Points.AddXY(x1, x2);
                        }
                        if (res >= -3.1 && res <= -2.9)
                        {
                            chart3.Series[2].Points.AddXY(x1, x2);
                        }
                    }
                    l++;
                }
                k++;
            }
        }

/*
    +-----------------------------+
    |     Monte Carlo method      |
    +-----------------------------+
*/
        public List<double> value = new List<double>();
        public List<double> x1L = new List<double>();
        public List<double> x2L = new List<double>();

        private void methodMonteCarlo()
        {
            clearInterface();

            Random r = new Random();
            int imin = -1, imax = 4; double x1 = 0, x2 = 0;
           
            for (int i = 0; i < 121; i++)
            {
                x1 = imin + r.NextDouble() * (imax - imin);
                x2 = imin + r.NextDouble() * (imax - imin);
                if (condition(x1,x2))
                {
                    x1L.Add(Math.Round(x1, 2)); 
                    x2L.Add(Math.Round(x2, 2)); 
                    value.Add(Math.Round(fun(x1, x2), 5));
                }
            }

            int k1 = 0;
            for (int i = 0; i < x1L.Count; i++)
            {
                double az = x2L[i];
                for (int j = i + 1; j < x1L.Count; j++)
                {
                    if (az == x2L[j])
                    {
                        if (k1 == 8) break;
                        chart1.Series[k1].Points.AddXY(x1L[i], x2L[i]);
                        chart1.Series[k1].Points.AddXY(x1L[j], x2L[j]);
                        k1++;
                    }
                }
            }
            double xx = 0, yy = 0;
            for (int i = 0; i < value.Count; i++)
            {
                if (value[i] == value.Min())
                {
                    xx = x1L[i]; yy = x2L[i];
                }
            }
            buildGrid();
            chart1.Series[k1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            chart1.Series[k1].Points.AddXY(xx, yy);
        }

        private void buildGrid()
        {
            dataGridView1.RowCount = x1L.Count + 1; dataGridView1.ColumnCount = x2L.Count + 1;
            for (int i = 0; i < x1L.Count; i++)
            {
                dataGridView1.Rows[i + 1].Cells[0].Value = x1L[i];
                dataGridView1.Rows[0].Cells[i + 1].Value = x2L[i];
            }
            for (int i = 1; i <= x1L.Count; i++)
            {
                for (int j = 1; j <= x1L.Count; j++)
                {
                    if (i == j)
                    {
                        dataGridView1.Rows[i].Cells[i].Value = Math.Round(value[i - 1], 2);
                        if (value[i - 1] == value.Min())
                        {
                            dataGridView1.Rows[i].Cells[i].Style.BackColor = System.Drawing.Color.Aqua;
                            dataGridView1.Rows[i].Cells[0].Style.BackColor = System.Drawing.Color.Aqua;
                            dataGridView1.Rows[0].Cells[i].Style.BackColor = System.Drawing.Color.Aqua;
                            textBox1.Visible = true;
                            textBox1.Text = Convert.ToString(dataGridView1.Rows[i].Cells[i].Value);
                            //label2.Text = Convert.ToString(dataGridView1.Rows[i].Cells[0].Value);
                            //label3.Text = Convert.ToString(dataGridView1.Rows[0].Cells[i].Value);
                        }
                    }
                    else dataGridView1.Rows[i].Cells[j].Value = " - ";
                }
            }
        }
    }
}
