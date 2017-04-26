using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPR1
{
    class HookeJeevesPenalty : HookeJeeves
    {
        private String typePenalty;
        
        public HookeJeevesPenalty()
        {
            typePenalty = "Штраф квадрата срезки";
        }
        
        public double f(double _x1, double _x2)
        {
            int r;
             r = 500;
             //return 2 * x1 - Math.Pow(x1, 2) + Math.Pow(x2, 2) + r * g(x1, x2);
             return (-6 * _x1 + 2 * Math.Pow(_x2, 2) - 2 * _x1 * _x2 + 2 * Math.Pow(_x2, 2) + r * g(_x1, _x2));
        }

        private double g(double _x1,double _x2)
        {
            double g = 0;
            double[] condition ={-_x1 - _x2 + 2, _x1, _x2};

            if (typePenalty.Equals("Штраф квадрата срезки"))
            {
                for (int i = 0; i < condition.Length; i++)
                {
                    if (condition[i] >= 0)
                    {
                        g += 0;
                    }
                    else
                    {
                        g += Math.Pow(condition[i],2);
                    }
                }
            }
            return g;
        }

        private int randomValue()
        {
            Random rand = new Random();
            System.Threading.Thread.Sleep(1);
            int ksi = rand.Next();
            return ksi;
        }
    }
}
