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

        double funDfDx1(double _x1, double _x2)
        {
            return (-6 - 2 * _x2);
        }

        double funDfDx2(double _x1, double _x2)
        {
            return (4 * _x2 - 2 * _x1 + 4 * _x2);
        }

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
                    
                    
                    double zoom = 3.5;                    
                    double w = 1 / zoom;
                    double temp_value = value[value.Count - 1];
                    int rr = ((int)temp_value >> 16) & 0xFF;
                    if (((x1L[x1L.Count - 1] > -w) && (x1L[x1L.Count - 1] < w)) || ((x2L[x2L.Count - 1] > -w) && (x2L[x2L.Count - 1] < w)))
                    { 
                        temp_value = 0xFFFFFF; 
                    }
                    if ((rr > 230) && (rr < 255))
                    { 
                        temp_value = 0; 
                    }
                    else
                    { 
                        temp_value = 0xFFFFFF; 
                    }
                    //chart3.Series[1].Points.Add(temp_value);
                    chart3.Series[2].Points.AddXY(x1L[x1L.Count - 1], x2L[x2L.Count - 1]);
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
                        

                        
                        LevelLine ll = new LevelLine(chart3);
                        ll.DrawLine(value[i - 1]);
                        
                        if (value[i - 1] == value.Min())
                        {
                            dataGridView1.Rows[i].Cells[i].Style.BackColor = System.Drawing.Color.Aqua;
                            dataGridView1.Rows[i].Cells[0].Style.BackColor = System.Drawing.Color.Aqua;
                            dataGridView1.Rows[0].Cells[i].Style.BackColor = System.Drawing.Color.Aqua;
                            textBox1.Visible = true;
                            textBox1.Text = Convert.ToString(dataGridView1.Rows[i].Cells[i].Value);
                            dataGridView1.Rows[i].Cells[i].Value = Math.Round(value[i - 1], 2);
                            //label2.Text = Convert.ToString(dataGridView1.Rows[i].Cells[0].Value);
                            //label3.Text = Convert.ToString(dataGridView1.Rows[0].Cells[i].Value);
                        }
                        
                    }
                    else dataGridView1.Rows[i].Cells[j].Value = " - ";
                }
            }
        }














        /*
        public float f(float x, float y)
        {
            float f1;
            f1 = 2 * x * x * x * x - 3 * x * x + 4 * y * y;
            return f1;
        }

//Вводим функцию для частной производной df/dx:

        public float df_dx(float x, float y)
        {
            float f2;
            f2 = 8 * x * x * x - 6 * x;
            return f2;
        }

//Вводим функцию для частной производной df/dy:

        public float df_dy(float x, float y)
        {
            float f3;
            f3 = 8 * y;
            return f3;
        }
        */
        float f(float _x1, float _x2)
        {
            return (-6 - 2 * _x2);
        }

        float df_dx(float _x1, float _x2)
        {
            return (4 * _x2 - 2 * _x1 + 4 * _x2);
        }

        float df_dy(float _x1, float _x2)
        {
            return (-6 * _x1 + 2 * (float)Math.Pow(_x2, 2) - 2 * _x1 * _x2 + 2 * (float)Math.Pow(_x2, 2));
        }

//Объявляем перо для рисования линий уровня:

        Pen myPen;

//Загружаем функции для рисования линий уровня:

        private void Form1_Load_1(object sender, EventArgs e)
        {
            //Создаем экземпляр пера с цветом и толщиной:
            myPen = new Pen(Color.Black, 0);

            //Связываем графический элемент PictureBox1 с объектом g класса Graphics:
            Bitmap bmp = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
            Graphics g = Graphics.FromImage(bmp);

            //Определяем преобразования для масштабирования и рисования линий на PictureBox в интервале
            //-2 <= x <= 2, -1.5 <= y <= 1.5:
            float x_min = -1f;
            float x_max = 4f;
            float y_min = -1f;
            float y_max = 4f;

            g.ScaleTransform(bmp.Width / (x_max - x_min), bmp.Height / (y_max - y_min));
            g.TranslateTransform(-x_min, -y_min, System.Drawing.Drawing2D.MatrixOrder.Prepend);

            //Вызываем функцию для рисования линий уровня:
            for (int LevelCurves = -3; LevelCurves <= 25; LevelCurves++)
            {
                PlotLevelCurve(g, Convert.ToSingle(LevelCurves / 4), -4f, 4f, -4f, 4f, 0.05f, 1f, 1f, 0.002f);
            }

            //Показываем результат рисования:
            pictureBox1.Image = bmp;
        }

/*
Ниже этого кода записываем следующие вспомогательные методы.
Листинг 40.2. Вспомогательные методы.
*/
//Находим точку на линии:

        float initial_delta = 0.1f;
        private void FindPointOnCurve(ref float x, ref float y, float LevelCurves, float start_x, float start_y, float tolerance)
        {
            float dx = 0, dy = 0, dz, delta, f_xy;
            int direction = 0;

            //Начальная точка:
            x = start_x; y = start_y; delta = initial_delta;

            //В бесконечном цикле do-while выходим через break:
            int i = 0;
            do
            {
                f_xy = f(x, y); dz = LevelCurves - f_xy;
                if (Math.Abs(dz) < tolerance) break;

                //Анализируем направление:
                if (Math.Sign(dz) != direction)
                {
                    //Изменяем направление. Уменьшаем delta:
                    delta = delta / 2;
                    direction = Math.Sign(dz);
                }

                //Рассчитываем градиент:
                Gradient(x, y, ref dx, ref dy);
                if ((Math.Abs(dx) + Math.Abs(dy)) < 0.001) break;

                //Перемещаемся направо:
                x = x + dx * delta * (float)direction;
                y = y + dy * delta * (float)direction;
            }
            while (i < 1);
        }

//Рассчитываем градиент в этой точке:
        private void Gradient(float x, float y, ref float dx, ref float dy)
        {
            float dist = 0;
            dx = df_dx(x, y); dy = df_dy(x, y);
            dist = Convert.ToSingle(Math.Sqrt(dx * dx + dy * dy));

            if (Math.Abs(dist) < 0.0001)
            {
                dx = 0; dy = 0;
            }
            else
            {
                dx = dx / dist; dy = dy / dist;
            }
        }

//Рисуем линию уровня f(x, y) = LevelCurves:
        private void PlotLevelCurve(Graphics g,float LevelCurves, float x_min, float x_max, float y_min,float y_max, float step_size,float start_x, float start_y,float tolerance)
        {
            int num_points = 0;
            float x0 = 0, y0 = 0, x1, y1, x2, y2, dx = 0, dy = 0;

            //Находим точку (x0, y0) на линии уровня LevelCurves:
            FindPointOnCurve(ref x0, ref y0, LevelCurves,
            start_x, start_y, tolerance);

            //Начало:
            num_points = 1;

            //Следующая линия уровня LevelCurves:
            x2 = x0; y2 = y0;

            //В бесконечном цикле do-while выходим через break:
            int i = 0;
            do
            {

                x1 = x2; y1 = y2;

                //Находим следующую точку на линии:
                Gradient(x2, y2, ref dx, ref dy);
                if ((Math.Abs(dx) + Math.Abs(dy)) < 0.001) break;
                    x2 = x2 + dy * step_size;

                y2 = y2 - dx * step_size;
                FindPointOnCurve(ref x2, ref y2, LevelCurves, x2, y2, tolerance);

                //Рисуем до этой точки:
                g.DrawLine(myPen, x1, y1, x2, y2);
                num_points = num_points + 1;

                //Смотрим,находится ли точка вне области рисования:
                if (x2 < x_min) break;
                if (x2 > x_max) break;
                if (y2 < y_min) break;
                if (y2 > y_max) break;

                //Если мы ушли более чем на 4 точки, то смотрим не пришли ли мы в начало:
                if (num_points >= 4)
                {
                    if (Math.Sqrt((x0 - x2) * (x0 - x2) + (y0 - y2) * (y0 - y2)) <= step_size * 1.1)
                    {
                        g.DrawLine(myPen, x2, y2, x0, y0);
                        break;
                    }
                }
            } while (i < 1);
        }















        /*
#include "Math.hpp"
#include "math.h"
....................
void __fastcall TForm1::FormPaint(TObject *Sender)
{
    int  h_1=Form1->ClientHeight;
    int  w_1=Form1->ClientWidth;
    float zoom = 3.5;
	float w = 1/zoom;
	for (int i=0;i<w_1;i++)
	{
		for (int j=0;j<h_1;j++)
		{
		    float x = (w_1/2-i)/zoom;
			float y = (h_1/2-j)/zoom;
			int colour = SimpleRoundTo(5*(x+y)*(x*y)/(x*x*y*y+1)+sqrt(x*x*y*y+1));
		 	if (((x > -w) && (x < w)) ||
			 ((y > -w) && (y < w)))
			 {colour = 0xFFFFFF;}
                        if ((GetRValue(colour)>230)&&(GetRValue(colour)<255))
                         {colour=0;}
                        else
                        {colour=0xFFFFFF;}
			Form1->Canvas->Pixels[i][j]=colour;
		}
	}
}
         */
    }
}
