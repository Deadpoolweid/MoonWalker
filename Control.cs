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

        public Action main(Data data)
        {
            SetLocation(data.map);
            Coord xy = data.XY;
            Direction direction = DetectDirection(DetectDirections(DetectQuarter(xy)));
            return ChooseAction(data,direction);
        }

        static Action ChooseAction(Data data, Direction d)
        {
            if (data.d == Direction.Up)
            {
                switch ((int) d)
                {
                    case 1:
                        return Action.B;
                    case 2:
                        return Action.L;
                    case 3:
                        return Action.R;
                }
            }
            else if (data.d == Direction.Down)
            {
                switch ((int) d)
                {
                    case 0:
                        return Action.B;
                    case 2:
                        return Action.R;
                    case 3:
                        return Action.L;
                }
            }
            else if (data.d == Direction.Left)
            {
                switch ((int) d)
                {
                    case 0:
                        return Action.R;
                    case 1:
                        return Action.L;
                    case 3:
                        return Action.B;
                }
            }
            else if (data.d == Direction.Right)
            {
                switch ((int) d)
                {
                    case 0:
                        return Action.L;
                    case 1:
                        return Action.R;
                    case 2:
                        return Action.B;
                }
            }
            else if (data.d == Direction.Unknown)
            {
                throw new ArgumentException("Направильно задано начальное направление.");
            }
            return Action.F;
        }

        /// <summary>
        /// Создаёт карту с единичным радиусом видимости
        /// </summary>
        /// <param name="map">Исходная карта</param>
        private void SetLocation(Obstacle[,] map)
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
        private Direction[] DetectDirections(Quarter q)
        {
            Direction[] dir = new Direction[4];

            switch (q)
            {
                case Quarter.First:
                    dir = new[] {Direction.Left, Direction.Up, Direction.Right, Direction.Down};
                    break;
                case Quarter.Second:
                    dir = new[] {Direction.Left, Direction.Down, Direction.Right, Direction.Up};
                    break;
                case Quarter.F_S:
                    dir = new[] {Direction.Left, Direction.Up, Direction.Down, Direction.Right};
                    break;
                case Quarter.Third:
                    dir = new[] {Direction.Right, Direction.Down, Direction.Left, Direction.Up};
                    break;
                case Quarter.S_T:
                    dir = new[] {Direction.Down, Direction.Left, Direction.Right, Direction.Up};
                    break;
                case Quarter.Fourth:
                    dir = new[] {Direction.Right, Direction.Up, Direction.Left, Direction.Down};
                    break;
                case Quarter.T_Fth:
                    dir = new[] {Direction.Right, Direction.Up, Direction.Down, Direction.Left};
                    break;
                case Quarter.Fth_F:
                    dir = new[] {Direction.Up, Direction.Left, Direction.Right, Direction.Down};
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
        private Direction DetectDirection(Direction[] directions)
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
        private Quarter DetectQuarter(Coord xy)
        {
            int x = xy.X;
            int y = xy.Y;

            if (x > 0)
            {
                if (y > 0)
                {
                    return Quarter.Second;
                }
                if (y < 0)
                {
                    return Quarter.First;
                }
                return Quarter.F_S;
            }
            if (x < 0)
            {
                if (y > 0)
                {
                    return Quarter.Third;
                }
                if (y < 0)
                {
                    return Quarter.Fourth;
                }
                return Quarter.T_Fth;
            }
            if (y > 0)
            {
                return Quarter.S_T;
            }
            if (y < 0)
            {
                return Quarter.Fth_F;
            }
            return Quarter.Nexus;
        }

        /// <summary>
        /// Проверяет можно ли двигаться в указанном направлении
        /// </summary>
        /// <param name="direction">Направление движения</param>
        /// <returns>Возможность двигаться</returns>
        private bool CanMove(Direction direction)
        {
            var checkingPlace = (int) direction;
            if (CurrentLocation[checkingPlace] == Obstacle.Empty)
            {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Четверть координатной плоскости
    /// </summary>
    internal enum Quarter
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
