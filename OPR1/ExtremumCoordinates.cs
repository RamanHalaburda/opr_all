using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPR1
{
    public class ExtremumCoordinates
    {
        //гетеры
        public double getX1()
        {
            return x1;
        }
        public double getX2()
        {
            return x2;
        }
        public double getExtremum()
        {
            return extremum;
        }

        public bool Equal(ExtremumCoordinates extremumCoordinates)
        {
            if (this.x1 == extremumCoordinates.getX1()
                && this.x2 == extremumCoordinates.getX2()
                && this.extremum == extremumCoordinates.getExtremum())
            {
                return true;
            }
            return false;
        }

        public void Add(double x1, double x2, double extremum)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.extremum = extremum;
        }
        //private
        private double x1;
        private double x2;
        private double extremum;
    }
}
