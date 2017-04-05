using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPR1
{
    public class Monte_Carlo
    {
        private double min;
        private List<ExtremumCoordinates> extremumCoordinatesList;

        //public
        public Monte_Carlo()
        {
            extremumCoordinatesList = new List<ExtremumCoordinates>();
            min = Double.MaxValue;
        }

        public List<ExtremumCoordinates> getExtremumCoordinatesList()
        {
            return extremumCoordinatesList;
        }

        //private
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
                ksi = randomValue();
                x1 = ximin + ksi * (ximax - ximin);

                ksi = randomValue();
                x2 = ximin + ksi * (ximax - ximin);

                double result = f(x1, x2);
                double first_border = 2 * x1 * x1 + 3 * x2 * x2;
                if (borderOn(x1, x2))
                {
                    if (result < min)
                    {
                        min = result;
                        ExtremumCoordinates extremumCoordinates = new ExtremumCoordinates();
                        extremumCoordinates.Add(x1, x2, result);
                        extremumCoordinatesList.Add(extremumCoordinates);
                    }
                }
                i++;
            } while (i < 1210);
            return min;
        }

        private bool borderOn(double x1, double x2)
        {
            /*  double first_border = Math.Pow(x1, 2) - 6 * x1 + 4 * x2 - 11;
             double second_border = 3*x2 - x1*x2 + Math.Exp(x1-3) - 1;
             if (first_border >= 0 && second_border >= 0)*/
            double first_border = 2 * Math.Pow(x1, 2) + 3 * Math.Pow(x2, 2);
            if (first_border <= 6 && x1 >= 0 && x2 >= 0)
            {
                return true;
            }
            return false;
        }

        private double randomValue()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            System.Threading.Thread.Sleep(1);
            double ksi = Math.Round(rand.NextDouble(), 2);
            return ksi;
        }

        private double f(double x1, double x2)
        {
            //return Math.Pow(x1, 2) + Math.Pow(x2, 2) - 16 * x1 - 10 * x2;
            //return 2 * x1 - Math.Pow(x1, 2) + Math.Pow(x2, 2);
            return (-6 * x1 + 2 * Math.Pow(x2, 2) - 2 * x1 * x2 + 2 * Math.Pow(x2, 2));
        }
    }
}