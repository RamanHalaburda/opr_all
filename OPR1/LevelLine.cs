using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace opr_all
{
    class LevelLine
    {
        private Chart chart;
        private Series series;
        Random rand = new Random((int)DateTime.Now.Ticks);

        public LevelLine(Chart _chart)
        {
            this.chart = _chart;
            _chart.Series.Clear();
            _chart.Legends.Clear();
        }

        // Этот метод выводит на график точку экстемума на основонии полученых координат от вызывающей программы.
        public void pointExtremum(double x1, double x2)
        {
            //Выводим на график точку с получеными координатами
            this.series = new Series("extremum");
            series.Points.AddXY(x1, x2);
            try
            {
                setSeries();
                series.Color = System.Drawing.Color.Red;
                chart.Series.Add(series);
            }
            catch (ArgumentException)
            {
                return;
            }
        }

        private double f(float _x1, float _x2)
        {
            return (-6 * _x1 + 2 * Math.Pow(_x2, 2) - 2 * _x1 * _x2 + 2 * Math.Pow(_x2, 2));
        }


        public void drawLine(double extremum)
        {
            this.series = new Series("f(x) = " + extremum);
            for (float x1 = -1; x1 <= 4; x1 += 0.001f)
            {
                for (float x2 = -1; x2 <= 4; x2 += 0.002f)
                {
                    if (Math.Round(f(x1, x2), 3) == Math.Round(extremum, 3))
                    {
                        series.Points.AddXY(x1, x2);
                    }
                }
            }

            setSeries();
            try
            {
                chart.Series.Add(series);
            }
            catch (ArgumentException)
            {
                return;
            }
        }

        private void clearSeries()
        {
            while (chart.Series.Count > 0) { chart.Series.RemoveAt(0); }
        }

        private void setSeries()
        {
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series.Color = System.Drawing.Color.FromArgb(255, getRandomValue(), 0, getRandomValue());
            series.BorderWidth = 1;
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "F1";
        }

        private double getX2(double _x1, double _extremum)
        {
            return Math.Round(Math.Sqrt(Math.Pow(_x1, 2) - 2 * _x1 + _extremum), 3);
        }

        private int getRandomValue() // returning Ksi
        { 
            return rand.Next(255);
        }

        private bool conditions(double _x1, double _x2)
        {
            if (_x1 + _x2 <= 2 && _x1 >= 0 && _x2 >= 0)
            {
                return true;
            }
            return false;
        }
    }
}