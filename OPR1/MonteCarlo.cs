using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opr_all
{
    public class MonteCarlo
    {
        private double min;
        private List<Extremums> extremumsList;

        public MonteCarlo()
        {
            extremumsList = new List<Extremums>();
            min = Double.MaxValue;
        }

        public List<Extremums> getExtremumsList()
        {
            return extremumsList;
        }

        public double monteCarlo()
        {
            double x1;
            double x2;
            double ximin;
            double ximax;
            ximin = -1;
            ximax = 4;

            int i = 1;
            do
            {
                double ksi;
                ksi = getRandomValue();
                x1 = ximin + ksi * (ximax - ximin);

                ksi = getRandomValue();
                x2 = ximin + ksi * (ximax - ximin);

                double result = f(x1, x2);
                double first_border = 2 * x1 * x1 + 3 * x2 * x2;
                if (condidions(x1, x2))
                {
                    if (result < min)
                    {
                        min = result;
                        Extremums extremums = new Extremums();
                        extremums.Add(x1, x2, result);
                        extremumsList.Add(extremums);
                    }
                }
                i++;
            } while (i < 1210);
            return min;
        }

        private bool condidions(double _x1, double _x2)
        {
            if (_x1 + _x2 <= 2 && _x1 >= 0 && _x2 >= 0)
            {
                return true;
            }
            return false;
        }

        private double getRandomValue()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            System.Threading.Thread.Sleep(1);
            double ksi = Math.Round(rand.NextDouble(), 2);
            return ksi;
        }

        private double f(double _x1, double _x2)
        {
            return (-6 * _x1 + 2 * Math.Pow(_x2, 2) - 2 * _x1 * _x2 + 2 * Math.Pow(_x2, 2));
        }
    }
}