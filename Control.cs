using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoonWalker
{
    class Control
    {
        private Obstacle[] CurrentLocation = new Obstacle[4];

        public void main(Data data)
        {
            SetLocation(data.map);
            Coord xy = data.XY;
            Direction direction = DetectDirection(DetectDirections(DetectQuarter(xy)));
        }

        /// <summary>
        /// Создаёт карту с единичным радиусом видимости
        /// </summary>
        /// <param name="map">Исходная карта</param>
        void SetLocation(Obstacle[,] map)
        {
            // верх
            CurrentLocation[0] = map[8, 7];
            // Лево
            CurrentLocation[1] = map[7, 8];
            // Право
            CurrentLocation[2] = map[9, 8];
            // Низ
            CurrentLocation[3] = map[8, 9];
        }

        /// <summary>
        /// Расчитывает приоритетный список направлений передвижения
        /// </summary>
        /// <param name="q">Четверть, в которой находится луноход</param>
        /// <returns>Направление движения</returns>
        Direction[] DetectDirections(Quarter q)
        {
            Direction[] dir = new Direction[4];

            switch (q)
            {
                case Quarter.First:
                    dir = new[] {Direction.Left, Direction.Up, Direction.Right, Direction.Down};
                    break;
                case Quarter.Second:
                    dir = new[] { Direction.Left, Direction.Down, Direction.Right, Direction.Up };
                    break;
                case Quarter.F_S:
                    dir = new[] { Direction.Left, Direction.Up, Direction.Down, Direction.Right};
                    break;
                case Quarter.Third:
                    dir = new[] { Direction.Right, Direction.Down, Direction.Left, Direction.Up };
                    break;
                case Quarter.S_T:
                    dir = new[] { Direction.Down, Direction.Left, Direction.Right, Direction.Up };
                    break;
                case Quarter.Fourth:
                    dir = new[] { Direction.Right, Direction.Up, Direction.Left, Direction.Down };
                    break;
                case Quarter.T_Fth:
                    dir = new[] { Direction.Right, Direction.Up, Direction.Down, Direction.Left };
                    break;
                case Quarter.Fth_F:
                    dir = new[] { Direction.Up, Direction.Left, Direction.Right, Direction.Down };
                    break;
                case Quarter.Nexus:
                    dir = null;
                    break;
                default:
                    throw new ArgumentException("Неправильная четверть!");
            }
            return dir;
        }

        /// <summary>
        /// Определяет оптимальное направление движения
        /// </summary>
        /// <param name="directions">приоритетный список направлений передвижения</param>
        /// <returns>оптимальное направление движения</returns>
        Direction DetectDirection(Direction[] directions)
        {
            for (int i = 0; i < 4; i++)
            {
                if (CanMove(directions[i]))
                {
                    return directions[i];
                }
            }
            return Direction.Unknown;
        }

        /// <summary>
        /// Распознаёт четверть координатной плоскости по полученным координатам точки
        /// </summary>
        /// <param name="xy">Координаты точки</param>
        /// <returns>Четверть, в которой находится точка</returns>
        Quarter DetectQuarter(Coord xy)
        {
            int x = xy.X;
            int y = xy.Y;

            if (x > 0)
            {
                if (y > 0)
                {
                    return Quarter.Second;
                }
                else if (y < 0)
                {
                    return Quarter.First;
                }
                else
                {
                    return Quarter.F_S;
                }
            }
            else if (x < 0)
            {
                if (y > 0)
                {
                    return Quarter.Third;
                }
                else if (y < 0)
                {
                    return Quarter.Fourth;
                }
                else
                {
                    return Quarter.T_Fth;
                }
            }
            else
            {
                if (y > 0)
                {
                    return Quarter.S_T;
                }
                else if (y < 0)
                {
                    return Quarter.Fth_F;
                }
                else
                {
                    return Quarter.Nexus;
                }
            }
        }

        /// <summary>
        /// Проверяет можно ли двигаться в указанном направлении
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns>Возможность двигаться</returns>
        bool CanMove(Direction direction)
        {
            var checkingPlace = (int)direction;
            if (CurrentLocation[checkingPlace] == Obstacle.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Четверть координатной плоскости
    /// </summary>
    enum Quarter
    {
        First = 1,
        Second,
        Third,
        Fourth,
        F_S,
        S_T,
        T_Fth,
        Fth_F,
        Nexus = 0
    }
}
