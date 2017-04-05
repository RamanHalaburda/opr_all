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
        public Form1()
        { 
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            comboBox1.SelectedIndex = 0;
        }

        double funDfDx1(double _x1, double _x2)
        {
            return (-6 - 2 * _x2);
        }

        double funDfDx2(double _x1, double _x2)
        {
            return (4 * _x2 - 2 * _x1 + 4 * _x2);
        }

        // Hyperbolic cylinder
        double fun(double _x1, double _x2)
        {
            return (-6 * _x1 + 2 * Math.Pow(_x2, 2) - 2 * _x1 * _x2 + 2 * Math.Pow(_x2, 2));
        }

        Boolean condition(double _x1, double _x2)
        {
            if (_x1 + _x2 <= 2 && _x1 >= 0 && _x2 >= 0)
            {
                return true;
            }
            return false;
        }

        private void monte_carlo()
        {
            LevelLine ll = new LevelLine(chart3);
            double extremum;

            Monte_Carlo mc = new Monte_Carlo();

            extremum = mc.monteCarlo();
            List<ExtremumCoordinates> extremumCoordinatesList = mc.getExtremumCoordinatesList();

            dataGridView1.RowCount = extremumCoordinatesList.Count;
            dataGridView1.ColumnCount = 3;
            dataGridView1.ColumnHeadersVisible = true;            
            dataGridView1.Columns[0].HeaderText = "X1";
            dataGridView1.Columns[1].HeaderText = "X2";
            dataGridView1.Columns[2].HeaderText = "MIN";
            //dataGridView1.colum

            for (int i = 0; i < extremumCoordinatesList.Count; i++)
            {
                ll.DrawLine(extremumCoordinatesList[i].getExtremum());

                dataGridView1.Rows[i].Cells[0].Value = extremumCoordinatesList[i].getX1();
                dataGridView1.Rows[i].Cells[1].Value = extremumCoordinatesList[i].getX2();
                dataGridView1.Rows[i].Cells[2].Value = extremumCoordinatesList[i].getExtremum();
                if (extremumCoordinatesList[i].getExtremum() == extremum)
                {
                    this.label1.Text = extremumCoordinatesList[i].getExtremum().ToString();
                    this.label2.Text = extremumCoordinatesList[i].getX1().ToString();
                    this.label3.Text = extremumCoordinatesList[i].getX2().ToString();
                    ll.pointExtremum(extremumCoordinatesList[i].getX1(), extremumCoordinatesList[i].getX2());
                }
            }
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        clearInterface();
                        textBox1.Visible = true;
                        methodDirectThroughGrid();
                        dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                        label1.Visible = false;
                        label2.Visible = false;
                        label3.Visible = false;
                        chart1.Visible = true;
                        chart3.Visible = false;
                        break;
                    case 1:
                        clearInterface();
                        label1.Visible = true;
                        label2.Visible = true;
                        label3.Visible = true;
                        textBox1.Visible = false;
                        chart1.Visible = true;
                        chart3.Visible = false;
                        //methodMonteCarlo();
                        monte_carlo();                        
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

        Graphics g;

        private void methodMonteCarlo()
        {
            clearInterface();
                      
            double x1min = -1;
            double x1max = 4;
            double x2min = -1;
            double x2max = 4;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns.Add("Column" + System.Convert.ToString(1), "x1");

            dataGridView1.Columns.Add("Column" + System.Convert.ToString(2), "x2");

            dataGridView1.Columns.Add("Column" + System.Convert.ToString(3), "z");


            int n = 20 /*System.Convert.ToInt32(textBox1.Text)*/;

            double x1, x2, f;
            double min = 10000;
            double minx1 = 0.0, minx2 = 0.0;
            Random r = new Random();

            dataGridView1.ColumnHeadersVisible = true;
            dataGridView1.Columns[0].HeaderCell.Value = "x1";
            dataGridView1.Columns[1].HeaderCell.Value = "x2";
            dataGridView1.Columns[2].HeaderCell.Value = "z";

            for (int i = 0; i < n; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].HeaderCell.Value = System.Convert.ToString(i);
                //NextDouble Возвращает случайное число с плавающей запятой, которое больше или равно 0,0 и меньше 1,0
                x1 = r.NextDouble() * (Math.Abs(x1max) + Math.Abs(x1min)) + x1min;
                x2 = r.NextDouble() * (Math.Abs(x2max) + Math.Abs(x2min)) + x2min;
                f = fun(x1, x2);

                if ((condition(x1, x2) == true) && (f < min))
                {
                    min = f;
                    minx1 = x1;
                    minx2 = x2;
                }

                dataGridView1[0, i].Value = System.Convert.ToString(x1.ToString("F2"));
                dataGridView1[1, i].Value = System.Convert.ToString(x2.ToString("F2"));
                dataGridView1[2, i].Value = System.Convert.ToString(f.ToString("F2"));
            }

            label1.Text = minx1.ToString("F5");
            label2.Text = minx2.ToString("F5");
            label3.Text = min.ToString("F5");


            // start build level line
            /*
            int size = pictureBox1.Width;
            x1 = -1; // -1
            x2 = 4; // 6
            int razm = Convert.ToInt32(x2 - x1);
            for (double i = 0; i < razm; i += 0.5)
            {
                double X0 = (razm - x2) * (size / razm);
                double Y0 = (razm - x2) * (size / razm);
                int z = Convert.ToInt32(fun(i, i) * (pictureBox1.Width / razm));
                
                g.DrawEllipse(new Pen(Color.Red,2),Convert.ToInt32(X0 - z/2 ), Convert.ToInt32(Y0 - z/2), z, z);                
            }
             */ 
            // finish bild level line
        }             
    }
}
