using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace OPR1
{
    class LevelLine
    {
        private Chart chart1;
        private Series series;

        public LevelLine(Chart chart1)
        {
            this.chart1 = chart1;
            chart1.Series.Clear();
            chart1.Legends.Clear();
        }

        /*Этот метод выводит на график точку экстемума
         на основонии полученых координат от вызывающей программы.*/
        public void pointExtremum(double x1 , double x2)
        {
         //Выводим на график точку с получеными координатами
            this.series = new Series("extremum");
            series.Points.AddXY(x1, x2);
            try
            {
                settingsSeries();
                series.Color = System.Drawing.Color.Red;
                chart1.Series.Add(series);
            }
            catch (ArgumentException)
            {
                return;
            }
        }

        private double f(float _x1, float _x2)
        {
            //return  Math.Pow(x1, 2) + Math.Pow(x2, 2) - 16 * x1 - 10 * x2;
            //return 2 * x1 - Math.Pow(x1, 2) + Math.Pow(x2, 2);
            return (-6 * _x1 + 2 * Math.Pow(_x2, 2) - 2 * _x1 * _x2 + 2 * Math.Pow(_x2, 2));
        }


        public void DrawLine(double extremum)
        {
            this.series = new Series("f(x) = " + extremum);
            for (float x1 = -1; x1 <= 4; x1 += 0.001f)
            {
                for (float x2 = -1; x2 <= 4; x2 += 0.002f)
                {
                    //double x2 = getX2(Math.Round(x1,4), extremum);
                    if (Math.Round(f(x1, x2),3) == Math.Round(extremum,3))
                    {
                        series.Points.AddXY(x1, x2);
                        //series.Points.AddXY(x1, -x2);
                    }
                }
            }

            settingsSeries();
            try
            {
                chart1.Series.Add(series);
            }
            catch (ArgumentException)
            {
                return;
            }
        }

        private void clearSeries(){
            while (chart1.Series.Count > 0) { chart1.Series.RemoveAt(0); }
        }

        private void settingsSeries()
        {
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point; // тут сами поизменяет/повыбирайте тип вывода графика
            series.Color = System.Drawing.Color.FromArgb(255, randomValue(), 0, randomValue());
            series.BorderWidth = 1;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "F1";
        }

        private double getX2(double x1, double extremum)
        {
            return Math.Round(Math.Sqrt(Math.Pow(x1, 2) - 2 * x1 + extremum),3);
        }

        private int randomValue()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            System.Threading.Thread.Sleep(1);
            int ksi = rand.Next(255);
            return ksi;
        }

        private bool borderOn(double x1, double x2)
        {
            double first_border = 2 * Math.Pow(x1, 2) + 3 * Math.Pow(x2, 2);
            if (first_border <= 6 && x1 >= 0 && x2 >= 0)
            {
            /*double first_border = Math.Pow(x1, 2) - 6 * x1 + 4 * x2 - 11;
            double second_border = 3*x2 - x1*x2 + Math.Exp(x1-3) - 1;
            if (first_border >= 0 && second_border >= 0){*/
                return true;
            }
            return false;
        }
    }
}
