using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication1
{
    class LevelLine
    {
        private Chart chart;
        private Series series;

        public LevelLine(Chart _ch)
        {
            this.chart = _ch;
            chart.Series.Clear();
        }

        public void DrawLine(double extremum)
        {            
            this.series = new Series("f(x) = " + extremum);
            for (double x1 = -1; x1 <= 4; x1 += 0.005)
            {
                for (double x2 = -1; x2 <= 4; x2 += 0.005)
                {
                //double x2 = getX2(Math.Round(x1,4), extremum);
                    double function = Math.Round(f(x1, x2), 3);
                    if (borderOn(x1, x2) == true && function == extremum)
                    {
                        series.Points.AddXY(x1, x2);
                        series.Points.AddXY(x1, -x2);
                    }
                }
            }
            settingsSeries();
            try
            {
                chart.Series.Add(series);
            }
            catch(ArgumentException)
            {
                return;
            }
        }

        private void clearSeries(){
            while (chart.Series.Count > 0) { chart.Series.RemoveAt(0); }
        }

        private void settingsSeries()
        {
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point; // тут сами поизменяет/повыбирайте тип вывода графика
            series.Color = System.Drawing.Color.FromArgb(randomValue(), randomValue(), randomValue(), randomValue());
            series.BorderWidth = 1;
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "F1";
        }

        private double getX2(double x1, double extremum)
        {
            return Math.Sqrt(Math.Pow(x1, 2) - 2 * x1 + extremum);
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
                return true;
            }
            return false;
        }

        private double f(double x1, double x2)
        {
            //return Math.Pow(x1, 2) + Math.Pow(x2, 2) - 16 * x1 - 10 * x2;
            return 2 * x1 - Math.Pow(x1, 2) + Math.Pow(x2, 2);
        }
    }
}
