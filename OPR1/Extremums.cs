using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opr_all
{
    public class Extremums
    {
        private double x1;
        private double x2;
        private double extremum;

        public double getX1() { return x1; }
        public double getX2() { return x2; }
        public double getExtremum() { return extremum; }

        public bool Equal(Extremums extremums)
        {
            if (this.x1 == extremums.getX1()
                && this.x2 == extremums.getX2()
                && this.extremum == extremums.getExtremum())
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
    }
}
