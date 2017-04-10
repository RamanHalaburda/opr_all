using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

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
        }

        private void hooke_jeeves(Double _startX1, Double _startX2, Double _step, Double _accuracy)
        {
            chart3.Series.Clear();
            LevelLine ll = new LevelLine(chart1);
            HookeJeeves hookeJeeves = new HookeJeeves();
            ExtremumCoordinates ec = hookeJeeves.extremumFunction(_startX1, _startX2, _step, _accuracy);
            MessageBox.Show("Экстремум " + ec.getExtremum().ToString() + " x1= " + ec.getX1() + " x2 = " + ec.getX2());

            List<ExtremumCoordinates> extremumCoordinatesList = hookeJeeves.getExtremumCoordinatesList();

            Series series = new Series("way");
            Series seriesPoint = new Series("point");

            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series.Color = System.Drawing.Color.Black;
            series.BorderWidth = 3;

            dataGridView1.RowCount = extremumCoordinatesList.Count;
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].HeaderText = "X1";
            dataGridView1.Columns[1].HeaderText = "X2";
            dataGridView1.Columns[2].HeaderText = "MIN";

            for (double extremum = 0; extremum < 4; extremum += 0.25)
            {
                ll.DrawLine(extremum);
            }

            for (int i = 0; i < extremumCoordinatesList.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = extremumCoordinatesList[i].getX1();
                dataGridView1.Rows[i].Cells[1].Value = extremumCoordinatesList[i].getX2();
                dataGridView1.Rows[i].Cells[2].Value = extremumCoordinatesList[i].getExtremum();
                double x1 = extremumCoordinatesList[i].getX1();
                double x2 = extremumCoordinatesList[i].getX2();
                series.Points.AddXY(x1, x2);

                if (extremumCoordinatesList[i].getExtremum() == ec.getExtremum())
                {
                    this.label1.Text = Math.Round(extremumCoordinatesList[i].getExtremum(), 2).ToString();
                    this.label2.Text = Math.Round(extremumCoordinatesList[i].getX1(), 2).ToString();
                    this.label3.Text = Math.Round(extremumCoordinatesList[i].getX2(), 2).ToString();
                    ll.pointExtremum(extremumCoordinatesList[i].getX1(), extremumCoordinatesList[i].getX2());
                }
            }
            chart3.Series.Add(series);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        clearInterface();
                        label1.Visible = false;
                        label2.Visible = false;
                        label3.Visible = false;
                        groupBox1.Visible = true;
                        groupBox2.Visible = false;
                        textBox1.Visible = true;
                        methodDirectThroughGrid();
                        dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);                        
                        break;
                    case 1:
                        clearInterface();
                        label1.Visible = true;
                        label2.Visible = true;
                        label3.Visible = true;
                        textBox1.Visible = false;
                        groupBox2.Visible = true;
                        groupBox1.Visible = false;
                        groupBox2.Size = new Size(746,542);
                        chart3.Size = new Size(720, 520);
                        groupBox2.Location = new Point(259,12);
                        //methodMonteCarlo();
                        monte_carlo();
                        //dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);
                        
                        break;
                    case 2:
                        clearInterface();
                        Double startX1 = 0, startX2 = 0, step = 0, accuracy = 0;
                        if (Double.TryParse(textBox2.Text, out startX1) &&
                            Double.TryParse(textBox3.Text, out startX2) &&
                            Double.TryParse(textBox4.Text, out step) &&
                            Double.TryParse(textBox5.Text, out accuracy))
                        {
                            hooke_jeeves(startX1, startX2, step, accuracy);
                            //MessageBox.Show("Этот метод еще не реализован!");
                        }
                        else throw new Exception("Заполните начальную точку, шаг и точность!");
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

        //private double h = 0.5;

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
            dataGridView1.RowCount = 20; dataGridView1.ColumnCount = 10;
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

        //Graphics g;

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    groupBox3.Visible = false;
                    textBox1.Visible = true;
                    groupBox1.Visible = true;
                    groupBox2.Visible = false;
                    label1.Visible = label2.Visible = label3.Visible = false;
                    break;
                case 1:
                    label1.Visible = label2.Visible = label3.Visible = true;
                    groupBox2.Visible = true;
                    groupBox1.Visible = false;
                    break;
                case 2:   
                    textBox1.Visible = false;
                    groupBox3.Visible = true;
                    groupBox1.Visible = false;
                    groupBox2.Visible = true;
                    break;
            }
        }             
    }
}
