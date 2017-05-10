using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opr_all
{
    class HookeJeeves
    {
        enum Direction
        {
            Up,
            Down,
            Right,
            Left
        }

        private List<Extremums> extremumsList;

        public HookeJeeves()
        {
            extremumsList = new List<Extremums>();
        }

        /* Этот метод предназначен для расчета методом Хука-Дживса экстремума функции с двумя переменными.
         * Метод передаются координаты начальной точки, шага и точность от вызывающей программы.
         * Метод возращает значение экстремума и её координаты.*/
        public Extremums extremumFunction(double _x1_begin, double _x2_begin, double _step_begin, double _eps)
        {
            // Установить значение координат функции на начальные коорднаты и значение шага на начальный шаг.
            double x1 = _x1_begin;
            double x2 = _x2_begin;

            double step = _step_begin;
            Extremums basisPoint = new Extremums();
            basisPoint.Add(x1, x2, f(x1,x2));
            Extremums basisPointBegin = new Extremums();
            basisPointBegin.Add(x1, x2, f(x1, x2));
            extremumsList.Add(basisPointBegin);
            do
            {
                //* Вычислить значение функции в начальных точках и присвоить его базисной точке
                Extremums basisPointNew = new Extremums();
                basisPointNew.Add(basisPoint.getX1(), basisPoint.getX2(), basisPoint.getExtremum());
                x1 = Math.Round(basisPoint.getX1(),3);
                x2 = Math.Round(basisPoint.getX2(),3);
                //* Если значение функции с прибавленым шагом по оси x1 меньше значения функции в базисной точке, 
                //* то заменяем значение новой базисной точки функцией с прибавленым шагом,
                //* иначе если значение функции с шагом в противоположную сторону по оси x1 меньше значения функции в базисной точке, 
                //* то заменяем значение базисной точки функцией с с шагом в противоположную сторону.
                double function = f(x1 + step, x2);
                double xz = basisPointNew.getExtremum();
                if (function < basisPointNew.getExtremum() && condition(x1 + step, x2))
                {
                    basisPointNew.Add(x1 + step, x2, f(x1 + step, x2));
                }
                function = f(x1 - step, x2);
                if (function < basisPointNew.getExtremum() && condition(x1 - step, x2))
                {
                    basisPointNew.Add(x1 - step, x2, f(x1 - step, x2));
                }
                //* Если значение функции в базисной точке не изменилось, то проделываем все тоже самое, только по оси x2.
                if (basisPoint.Equal(basisPointNew))
                {
                    if (f(x1, x2 + step) < basisPointNew.getExtremum() && condition(x1, x2 + step))
                    {
                        basisPointNew.Add(x1, x2 + step, f(x1, x2 + step));
                    }
                    if (f(x1 - step, x2) < basisPointNew.getExtremum() && condition(x1, x2 - step))
                    {
                        basisPointNew.Add(x1, x2 - step, f(x1, x2 - step));
                    }
                }
                //* Если значение функции в базисной точке не изменилось, то уменьшаем шаг в 10 раз и возращаемся в начала функции.
                if (basisPoint.Equal(basisPointNew))
                {
                    step = step / 10;
                }
                //* Иначе если значение функции в базисной точке уменьшелось, то устанавливаем новое значение базисной точки и возращаемся в начало функции.
                else
                {
                    extremumsList.Add(basisPointNew);
                    basisPoint.Add(basisPointNew.getX1(), basisPointNew.getX2(), basisPointNew.getExtremum());                    
                }               
            } while (step > _eps);
            return basisPoint;
        }

        /*
         Этот метод предназначен для сравнения двух значений функций. 
         Он примимает на вход координаты базисной точки, значение функции базисной точки, шаг и направление расчета новой функции.
         На вывод передает значение новых координат и значения функции по этим координатам;         
         Если направление равно "вправо", то к координате x1 добавляем шаг и расчитваем значение функции.
         Если точка входит в ограничения высчитываем функцию с добавлением шага.
         Если значение функции с добавленым шагом меньше значения функции в базисной точке, то заменить значение координаты.         
         Если направление равно "влево", то от координате x1 отнимаем шаг и расчитваем значение функции.
         Если точка входит в ограничения высчитываем функцию с отнятым шагом.
         Если значение функции с добавленым шагом меньше значения функции в базисной точке, то заменить значение координаты. 
         */

        private bool condition(double _x1, double _x2)
        {
            if (_x1 + _x2 <= 2 && _x1 >= 0 && _x2 >= 0)
            {
                return true;
            }
            return false;
        }

        private double f(double _x1, double _x2)
        {      
            return (-6 * _x1 + 2 * Math.Pow(_x2, 2) - 2 * _x1 * _x2 + 2 * Math.Pow(_x2, 2));
        }

        public List<Extremums> getExtremumsList()
        {
            return extremumsList;
        }
    }
}
