using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{

    class HookeJeevesWithShtrafFun : HookeJeeves
    {
        private String typePenalty;

        const int COUNT_OF_BORDER = 3;

        public HookeJeevesWithShtrafFun()
        {
            typePenalty = "Штраф квадрата срезки";
        }
        
        override double f(double x1, double x2){
            int r;
             r = 300;
             return 2 * x1 - Math.Pow(x1, 2) + Math.Pow(x2, 2) + r * g(x1, x2);
        }

        private double g(double x1,double x2){
            double g = 0;
            double[] border ={-2 * Math.Pow(x1, 2) - 3 * Math.Pow(x2, 2) + 6,
                                 x1,
                                 x2};

            if (typePenalty.Equals("Штраф квадрата срезки"))
            {
                for (int m = 0; m < COUNT_OF_BORDER; m++)
                {
                    if (border[m] >= 0)
                    {
                        g += 0;
                    }
                    else
                    {
                        g += Math.Pow(border[m],2);
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
