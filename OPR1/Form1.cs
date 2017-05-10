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

namespace opr_all
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        clearInterface();
                        label1.Visible = label2.Visible = label3.Visible = false;
                        groupBox1.Visible = true;
                        groupBox2.Visible = false;
                        textBox1.Visible = true;
                        methodDirectThroughGrid();
                        dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);                        
                        break;
                    case 1:
                        clearInterface();
                        label1.Visible = label2.Visible = label3.Visible = true;
                        textBox1.Visible = false;
                        groupBox2.Visible = true;
                        groupBox1.Visible = false;
                        groupBox2.Size = new Size(746,542);
                        chart3.Size = new Size(720, 520);
                        groupBox2.Location = new Point(259,12);
                        methodMonteCarlo();
                        
                        break;
                    case 2:
                        clearInterface();
                        Double startX1 = 0, startX2 = 0, step = 0, accuracy = 0;
                        if (Double.TryParse(textBox2.Text, out startX1) &&
                            Double.TryParse(textBox3.Text, out startX2) &&
                            Double.TryParse(textBox4.Text, out step) &&
                            Double.TryParse(textBox5.Text, out accuracy))
                        {
                            methodHookeJeeves(startX1, startX2, step, accuracy);
                        }
                        else throw new Exception("Заполните начальную точку, шаг и точность!");
                        break;
                    case 3:
                        clearInterface();

                        startX1 = 0; startX2 = 0; step = 0; accuracy = 0;
                        if (Double.TryParse(textBox2.Text, out startX1) &&
                            Double.TryParse(textBox3.Text, out startX2) &&
                            Double.TryParse(textBox4.Text, out step) &&
                            Double.TryParse(textBox5.Text, out accuracy))
                        {
                            methodHookeJeevesWithPenalty(startX1, startX2, step, accuracy);
                        }
                        else throw new Exception("Заполните начальную точку, шаг и точность!");
                        break;
                    default:
                        MessageBox.Show("Error!");
                        break;
                }
            }
            catch (Exception exception) { MessageBox.Show(exception.ToString()); }
        }

/*
    +-----------------------------+
    | Direct Through Grid method  |
    +-----------------------------+
*/

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

        private void methodMonteCarlo()
        {
            LevelLine ll = new LevelLine(chart3);
            double extremum;

            MonteCarlo mc = new MonteCarlo();

            extremum = mc.monteCarlo();
            List<Extremums> extremumCoordinatesList = mc.getExtremumsList();

            dataGridView1.RowCount = extremumCoordinatesList.Count;
            dataGridView1.ColumnCount = 3;
            dataGridView1.ColumnHeadersVisible = true;
            dataGridView1.Columns[0].HeaderText = "X1";
            dataGridView1.Columns[1].HeaderText = "X2";
            dataGridView1.Columns[2].HeaderText = "MIN";

            for (int i = 0; i < extremumCoordinatesList.Count; i++)
            {
                ll.drawLine(extremumCoordinatesList[i].getExtremum());

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

/*
    +-----------------------------+
    |     Hooke Jeeves method      |
    +-----------------------------+
*/

        private void methodHookeJeeves(Double _startX1, Double _startX2, Double _step, Double _accuracy)
        {
            chart3.Series.Clear();
            LevelLine ll = new LevelLine(chart3);
            HookeJeeves hookeJeeves = new HookeJeeves();
            Extremums ec = hookeJeeves.extremumFunction(_startX1, _startX2, _step, _accuracy);
            String answer = "Экстремум " + ec.getExtremum().ToString() + " x1= " + ec.getX1() + " x2 = " + ec.getX2();
            MessageBox.Show(answer);
            textBox1.Visible = true;
            textBox1.Text = answer;

            List<Extremums> extremumCoordinatesList = hookeJeeves.getExtremumsList();

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
                ll.drawLine(extremum);
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

/*
    +------------------------------------------+
    |     Hooke Jeeves method with penalty     |
    +------------------------------------------+
*/

        private void methodHookeJeevesWithPenalty(Double _startX1, Double _startX2, Double _step, Double _accuracy)
        {
            chart3.Series.Clear();
            LevelLine ll = new LevelLine(chart3);
            HookeJeevesPenalty hookeJeevesPenalty = new HookeJeevesPenalty();
            List<Extremums> extremums = hookeJeevesPenalty.extremumFunctionWithPenalty(_startX1, _startX2, _step, _accuracy);
            
            Series series = new Series("way");
            Series seriesPoint = new Series("point");

            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series.Color = System.Drawing.Color.Black;
            series.BorderWidth = 3;

            dataGridView1.RowCount = extremums.Count;
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].HeaderText = "X1";
            dataGridView1.Columns[1].HeaderText = "X2";
            dataGridView1.Columns[2].HeaderText = "MIN";

            for (double extremum = 0; extremum < 4; extremum += 0.25)
            {
                ll.drawLine(extremum);
            }

            Double extr = extremums[0].getExtremum(),
                extr_x1 = extremums[0].getX1(),
                extr_x2 = extremums[0].getX2();
            for (int i = 0; i < extremums.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = extremums[i].getX1();
                dataGridView1.Rows[i].Cells[1].Value = extremums[i].getX2();
                dataGridView1.Rows[i].Cells[2].Value = extremums[i].getExtremum();
                double x1 = extremums[i].getX1();
                double x2 = extremums[i].getX2();
                series.Points.AddXY(x1, x2);

                if (extremums[i].getExtremum() == extremums[0].getExtremum())
                {
                    this.label1.Text = Math.Round(extremums[i].getExtremum(), 2).ToString();
                    this.label2.Text = Math.Round(extremums[i].getX1(), 2).ToString();
                    this.label3.Text = Math.Round(extremums[i].getX2(), 2).ToString();
                    ll.pointExtremum(extremums[i].getX1(), extremums[i].getX2());
                }

                if (extr > extremums[i].getExtremum())
                {
                    extr = extremums[i].getExtremum();
                    extr_x1 = extremums[i].getX1();
                    extr_x2 = extremums[i].getX2();
                }
            }
            chart3.Series.Add(series);

            String answer = "Экстремум " + extr + " x1= " + extr_x1 + " x2 = " + extr_x2;
            MessageBox.Show(answer);
            textBox1.Visible = true;
            textBox1.Text = answer;
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
                    break;
                case 1:
                    label1.Visible = label2.Visible = label3.Visible = true;                    
                    groupBox1.Visible = false;
                    groupBox2.Visible = true;
                    groupBox3.Visible = false;
                    break;
                case 2:
                    label1.Visible = label2.Visible = label3.Visible = false;
                    textBox2.Text = textBox3.Text = "0,0";
                    textBox4.Text = "0,2";
                    textBox5.Text = "0,1";
                    textBox1.Visible = false;
                    groupBox1.Visible = false;
                    groupBox2.Visible = true;
                    groupBox3.Visible = true;
                    groupBox2.Location = new Point(259, 138);
                    groupBox2.Size = new Size(746, 398);
                    groupBox3.Location = new Point(259, 4);
                    chart3.Dock = DockStyle.Fill;
                    break;
                case 3:
                    label1.Visible = label2.Visible = label3.Visible = false;
                    textBox2.Text = textBox3.Text = "0,0";
                    textBox4.Text = "0,2";
                    textBox5.Text = "0,1";
                    textBox1.Visible = false;
                    groupBox1.Visible = false;
                    groupBox2.Visible = true;
                    groupBox3.Visible = true;
                    groupBox2.Location = new Point(259, 138);
                    groupBox2.Size = new Size(746, 398);
                    groupBox3.Location = new Point(259, 4);
                    chart3.Dock = DockStyle.Fill;
                    break;
            }
        }

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
    }
}