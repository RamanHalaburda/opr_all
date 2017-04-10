using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPR1
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

        private List<ExtremumCoordinates> extremumCoordinatesList;

        public HookeJeeves()
        {
            extremumCoordinatesList = new List<ExtremumCoordinates>();
        }

        /* Этот метод предназначен для расчета методом Хука-Дживса экстремума функции с двумя переменными.
         * Метод передаются координаты начальной точки, шага и точность от вызывающей программы.
         * Метод возращает значение экстремума и её координаты.*/
        public ExtremumCoordinates extremumFunction(double x1_begin, double x2_begin, double h_begin, double e)
        {
            // Установить значение координат функции на начальные коорднаты и значение шага на начальный шаг.
            double x1 = x1_begin;
            double x2 = x2_begin;

            double h = h_begin;
            //Direction direction = Direction.Up;
            ExtremumCoordinates basisPoint = new ExtremumCoordinates();
            basisPoint.Add(x1, x2, f(x1,x2));
            ExtremumCoordinates basisPointBegin = new ExtremumCoordinates();
            basisPointBegin.Add(x1, x2, f(x1, x2));
            extremumCoordinatesList.Add(basisPointBegin);
            do
            {
                //* Вычислить значение функции в начальных точках и присвоить его базисной точке
                ExtremumCoordinates basisPointNew = new ExtremumCoordinates();
                basisPointNew.Add(basisPoint.getX1(), basisPoint.getX2(), basisPoint.getExtremum());
                x1 = Math.Round(basisPoint.getX1(),3);
                x2 = Math.Round(basisPoint.getX2(),3);
                //* Если значение функции с прибавленым шагом по оси x1 меньше значения функции в базисной точке, 
                //* то заменяем значение новой базисной точки функцией с прибавленым шагом,
                //* иначе если значение функции с шагом в противоположную сторону по оси x1 меньше значения функции в базисной точке, 
                //* то заменяем значение базисной точки функцией с с шагом в противоположную сторону.
                double function = f(x1 + h, x2);
                double xz = basisPointNew.getExtremum();
                if (function < basisPointNew.getExtremum() && condition(x1 + h, x2))
                {
                    basisPointNew.Add(x1 + h, x2, f(x1 + h, x2));
                }
                function = f(x1 - h, x2);
                if (function < basisPointNew.getExtremum() && condition(x1 - h, x2))
                {
                    basisPointNew.Add(x1 - h, x2, f(x1 - h, x2));
                }
                //* Если значение функции в базисной точке не изменилось, то проделываем все тоже самое, только по оси x2.
                if (basisPoint.Equal(basisPointNew))
                {
                    if (f(x1, x2 + h) < basisPointNew.getExtremum() && condition(x1, x2+ h))
                    {
                        basisPointNew.Add(x1, x2 + h, f(x1, x2 + h));
                    }
                    if (f(x1 - h, x2) < basisPointNew.getExtremum() && condition(x1, x2 - h))
                    {
                        basisPointNew.Add(x1, x2 - h, f(x1, x2 - h));
                    }
                }
                //* Если значение функции в базисной точке не изменилось, то уменьшаем шаг в 10 раз
                //* и возращаемся в начала функции.
                if (basisPoint.Equal(basisPointNew))
                {
                    h = h / 10;
                }
                //* Иначе если значение функции в базисной точке уменьшелось, то устанавливаем новое значение базисной точки
                //* и возращаемся в начало функции.
                else
                {
                    extremumCoordinatesList.Add(basisPointNew);
                    basisPoint.Add(basisPointNew.getX1(), basisPointNew.getX2(), basisPointNew.getExtremum());                    
                }               
            } while (h > e);
            return basisPoint;
        }

        /*
         Этот метод предназначен для сравнения двух значений функций. 
         * Он примимает на вход координаты базисной точки, значение функции базисной точки, шаг и направление расчета новой функции.
         * На вывод передает значение новых координат и значения функции по этим координатам;
         
         Если направление равно "вправо", то к координате x1 добавляем шаг и расчитваем значение функции.
         Если точка входит в ограничения высчитываем функцию с добавлением шага.
         Если значение функции с добавленым шагом меньше значения функции в базисной точке, то заменить значение координаты.
         
         Если направление равно "влево", то от координате x1 отнимаем шаг и расчитваем значение функции.
         Если точка входит в ограничения высчитываем функцию с отнятым шагом.
         Если значение функции с добавленым шагом меньше значения функции в базисной точке, то заменить значение координаты. 
         */
        /*
        private bool borderOn(double x1, double x2)
        {
            double first_border = 2 * Math.Pow(x1, 2) + 3 * Math.Pow(x2, 2);
            if (first_border <= 6 && x1 >= 0 && x2 >= 0)
            {
                return true;
            }
            return false;
        }*/

        Boolean condition(double _x1, double _x2)
        {
            if (_x1 + _x2 <= 2 && _x1 >= 0 && _x2 >= 0)
            {
                return true;
            }
            return false;
        }

        private double f(double _x1, double _x2)
        {
            //return 2 * x1 - Math.Pow(x1, 2) + Math.Pow(x2, 2);        
            return (-6 * _x1 + 2 * Math.Pow(_x2, 2) - 2 * _x1 * _x2 + 2 * Math.Pow(_x2, 2));
        }

        public List<ExtremumCoordinates> getExtremumCoordinatesList()
        {
            return extremumCoordinatesList;
        }
    }
}
