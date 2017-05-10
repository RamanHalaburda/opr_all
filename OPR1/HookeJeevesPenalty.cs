using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opr_all
{
    class HookeJeevesPenalty : HookeJeeves
    {
        private String typePenalty;
        private double x1;
        private double x2;
        private double step;
        private double extremum;
        private Direction direction;

        private enum Direction
        {
            Right,
            Left,
            Up,
            Down
        }
        
        public HookeJeevesPenalty()
        {
            typePenalty = "Штраф квадрата срезки";
        }
        
        public double F(double _x1, double _x2)
        {
             int R = 500;
             return (-6 * _x1 + 2 * Math.Pow(_x2, 2) - 2 * _x1 * _x2 + 2 * Math.Pow(_x2, 2) + R * G(_x1, _x2));
        }

        private bool condition(double _x1, double _x2)
        {
            if (_x1 + _x2 <= 2 && _x1 >= 0 && _x2 >= 0)
            {
                return true;
            }
            return false;
        }

        private double G(double _x1,double _x2) // returning penalty
        {
            double g = 0;
            double[] conditions = {-_x1 - _x2 + 2, _x1, _x2};

            if (typePenalty.Equals("Штраф квадрата срезки"))
            {
                for (int i = 0; i < conditions.Length; i++)
                {
                    if (conditions[i] >= 0)
                    {
                        g += 0;
                    }
                    else
                    {
                        g += Math.Pow(conditions[i],2);
                    }
                }
            }
            return g;
        }

        public List<Extremums> extremumFunctionWithPenalty(double _x1_begin, double _x2_begin, double _step_begin, double _Eps)
        {
            List<Extremums> extremumsList = new List<Extremums>();

            x1 = _x1_begin;
            x2 = _x2_begin;

            step = _step_begin;
            extremum = F(x1, x2);

            do
            {
                double extremumOld = extremum;
                basisPointNext();

                if (extremumOld.Equals(extremum))
                {
                    step = step / 10;
                }

                else
                {
                    x1 = Math.Round(x1, 3);
                    x2 = Math.Round(x2, 3);
                    extremum = Math.Round(extremum, 3);
                    Extremums basisPoint = new Extremums();
                    basisPoint.Add(x1, x2, extremum);
                    extremumsList.Add(basisPoint);
                }
            } while (step > _Eps);
            return extremumsList;
        }

        private void basisPointNext()
        {
            if (minFunctionValue(direction)) return;
            for (Direction dir = Direction.Right; dir <= Direction.Down; dir++)
            {
                if (!dir.Equals(direction))
                {
                    if (minFunctionValue(dir)) return;
                }
            }
        }

        private bool minFunctionValue(Direction direction)
        {
            double x1WithStep = x1, x2WithStep = x2;
            if (Direction.Right.Equals(direction))
            {
                x1WithStep -= step;
            }
            else if (Direction.Left.Equals(direction))
            {
                x1WithStep += step;
            }
            else if (Direction.Up.Equals(direction))
            {
                x2WithStep += step;
            }
            else if (Direction.Down.Equals(direction))
            {
                x2WithStep -= step;
            }

            if (condition(x1WithStep, x2WithStep))
            {
                double basisPointNewFunction = F(x1WithStep, x2WithStep);
                if (basisPointNewFunction < extremum)
                {
                    x1 = x1WithStep;
                    x2 = x2WithStep;
                    extremum = basisPointNewFunction;
                    this.direction = direction;
                    return true;
                }
            }
            return false;
        }
    }
}
