using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoonWalker
{
    class Control
    {
        /// <summary>
        /// Карта видимости
        /// </summary>
        private Obstacle[,] map = new Obstacle[15,15];

        private Coord StartPosition;

        /// <summary>
        /// Есть ли финиш в области видимости
        /// </summary>
        private bool FinishIsOnHorizont;

        /// <summary>
        /// Список точек, путешествие в которые невозможно
        /// </summary>
        private List<Coord> BlackList = new List<Coord>(); 

        public Action main(Data data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            StartPosition = data.XY;
            SetLocation(data.map);

            BlackList.Add(StartPosition);
            //// В единицу времени важен только обзор на одну клетку вперёд, поэтому
            //// Получаем список препятствий, находящихся непосредственно рядом

            //Coord xy = data.XY;

            ////DetectBlackList(xy);
            //// Определяем, находится ли точка финиша в зоне видимости
            //FinishIsOnHorizont = Math.Abs(xy.X) < 16 && Math.Abs(xy.Y) < 15;
            //// Приблизительная или точная точка финиша маршрута
            //Coord point = new Coord();
            //if (FinishIsOnHorizont)
            //{
            //    // Нас интересует именно начало координат, как конечная точка маршрута
            //    point = new Coord(0, 0);
            //}
            //else
            //{
            //    // Определяем четверть местонахождения лунохода,
            //    // Расчитываем список точек, которые могут являться конечной целью с данной областью видимости
            //    Coord[] points = CalculatePoints(DetectQuarter(xy));

            //    int min = 2001;

            //    // Находим точку с минимальным радиусом - это и будет приблизительная точка финиша текущего маршрута
            //    foreach (var _point in points)
            //    {
            //        int radius = CalculateRadius(_point);
            //        if (radius > min)
            //        {
            //            point = _point;
            //            min = radius;
            //        }
            //    } 
            //}

            Direction direction = CalculateDirection();
            // Выбираем действие, которое лучше всего сделать и отправляем его далее по конвееру
            return ChooseAction(data,direction);
        }

        /// <summary>
        /// Выбор действия лунохода, относительно выбранного направления движения
        /// </summary>
        /// <param name="data"></param>
        /// <param name="d">Направление движения</param>
        /// <returns>Действие</returns>
        static Action ChooseAction(Data data, Direction d)
        {
            if (data.d == Direction.Up)
            {
                switch ((int) d)
                {
                    case 1:
                        return Action.L;
                    case 2:
                        return Action.R;
                    case 3:
                        return Action.B;
                }
            }
            else if (data.d == Direction.Left)
            {
                switch ((int) d)
                {
                    case 0:
                        return Action.R;
                    case 2:
                        return Action.B;
                    case 3:
                        return Action.L;
                }
            }
            else if (data.d == Direction.Right)
            {
                switch ((int) d)
                {
                    case 0:
                        return Action.L;
                    case 1:
                        return Action.B;
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
                    case 1:
                        return Action.R;
                    case 2:
                        return Action.L;
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
            this.map = map;
        }

        /// <summary>
        /// Расчитывает приоритетный список точек, ближайших или являющихся центром координат
        /// </summary>
        /// <param name="q">Четверть, в которой находится луноход</param>
        /// <returns>Список точек</returns>
        private Coord[] CalculatePoints(Quarter q)
        {
            List<Coord> points;

            switch (q)
            {
                case Quarter.First:
                    points = AddPoints(8, 15, 8, 15);
                    break;
                case Quarter.Second:
                    points = AddPoints(8, 15, 0, 8);
                    break;
                case Quarter.F_S:
                    points = AddPoints(8, 15, 0, 15);
                    break;
                case Quarter.Third:
                    points = AddPoints(0, 8, 0, 8);
                    break;
                case Quarter.S_T:
                    points = AddPoints(0, 15, 0, 8);
                    break;
                case Quarter.Fourth:
                    points = AddPoints(0, 8, 8, 15);
                    break;
                case Quarter.T_Fth:
                    points = AddPoints(0, 8, 0, 15);
                    break;
                case Quarter.Fth_F:
                    points = AddPoints(0, 15, 8, 15);
                    break;
                case Quarter.Nexus:
                    points = null;
                    break;
                default:
                    throw new ArgumentException("Неправильная четверть!");
            }
            return points.ToArray();
        }

        /// <summary>
        /// Создаёт список возможных точек B из карты видимости
        /// </summary>
        /// <param name="x_for">Х от</param>
        /// <param name="x_to">Х до</param>
        /// <param name="y_for">У от</param>
        /// <param name="y_to">У до</param>
        /// <returns>Список возможных точек B</returns>
        private List<Coord> AddPoints(int x_for, int x_to, int y_for, int y_to)
        {
            List<Coord> points = new List<Coord>();
            for (int i = y_for-1; i < y_to-1; i++)
            {
                for (int j = x_for-1; j < x_to-1; j++)
                {
                    if (map[j, i] == Obstacle.Empty)
                    {
                        points.Add(new Coord(j, i));
                    }
                }
            }
            return points;
        }

        /// <summary>
        /// Рассчитывает расстояние в клетках, которое необходимо пройти с указанной точки до финиша
        /// </summary>
        /// <param name="point">Начальная точка</param>
        /// <returns>Расстояние</returns>
        private int CalculateRadius(Coord point)
        {
            return Math.Abs(point.X) + Math.Abs(point.Y);
        }

        private Direction CalculateDirection()
        {
            PointInfo startInfo = new PointInfo(CalculateRadius(StartPosition), 0);
            Coord startCoord = StartPosition;

            // Ближайшие к луноходу точки
            Coord[] _points = new Coord[4];
            for (int i = 0; i < 4; i++)
            {
                //Up
                _points[0] = new Coord(startCoord.X, startCoord.Y + 1);
                //Left
                _points[1] = new Coord(startCoord.X - 1, startCoord.Y);
                //Right
                _points[2] = new Coord(startCoord.X + 1, startCoord.Y);
                //Down
                _points[3] = new Coord(startCoord.X, startCoord.Y - 1);
            }

            // Добавление надёжных точек в возможный план маршрута
            List<Coord> OneBlockVision = _points.Where(coord => IsFree(coord)).ToList();

            PointInfo[] Infos = new PointInfo[OneBlockVision.Count];
            for (int i = 0; i < Infos.Count(); i++)
            {
                Infos[i] = new PointInfo(CalculateRadius(OneBlockVision[i]),startInfo.MoveCost);
            }

            int min = 2001;
            int k = 0;
            int min_k = 5;
            foreach (var inf in Infos)
            {
                int cost = inf.CalculateCost();
                //if (cost == min)
                //{
                //    if (_points[k].X < _points[min_k].X || _points[k].Y < _points[min_k].Y)
                //    {
                //        min_k = k;
                //    } 
                //}
                if (cost < min)
                {
                    if (BlackList.Contains(_points[k]))
                    {
                        k++;
                        continue;
                    }
                    min = cost;
                    min_k = k;
                }
                k++;
            }
            Direction d = (Direction)min_k;
            BlackList.Add(_points[min_k]);
            return  d;
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
        private bool IsFree(Coord point)
        {
            Coord PointLocation = CalculateMapLocation(point, StartPosition);
            return map[PointLocation.X - 1, PointLocation.Y - 1] == Obstacle.Empty;
        }

        /// <summary>
        ///  Создаёт список точек, местонахождение в которых невозможно
        /// </summary>
        /// <param name="startCoord">Координаты местонахождения лунохода</param>
        private void DetectBlackList(Coord startCoord)
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (map[j, i] == Obstacle.Hole || map[j, i] == Obstacle.Mooners || map[j, i] == Obstacle.Rock)
                    {
                        BlackList.Add(CalculateCoord(j+1,i+1,startCoord));
                    }
                }
            }
        }

        /// <summary>
        /// Считает координаты точки по указанным данным из области видимости
        /// </summary>
        /// <param name="x">Номер столбца</param>
        /// <param name="y">Номер строки</param>
        /// <param name="start">Координаты центральной точки</param>
        /// <returns>Координаты искомой точки</returns>
        private Coord CalculateCoord(int x, int y, Coord start)
        {
            return new Coord(start.X+(x-8), start.Y - (y-8));
        }

        /// <summary>
        /// Считает положение точки на карте видимости по указанным координатам
        /// </summary>
        /// <param name="point">Координата точки</param>
        /// <param name="start">Местонахождение лунохода</param>
        /// <returns>Положение точки на карте видимости</returns>
        private Coord CalculateMapLocation(Coord point, Coord start)
        {
            return new Coord(point.X - start.X + 8, start.Y-point.Y+8 );
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

    class PointInfo
    {
        // Цена за передвижение на данную клетку
        public int MoveCost;

        // Цена за достижение финиша(без учёта преград)
        private int FinishCost;

        /// <summary>
        /// Создаёт новую информацию о точке
        /// </summary>
        /// <param name="ThisPointRadius">Радиус новой точки</param>
        /// <param name="StartPointMoveCost">Стоимость передвижения стартовой точки</param>
        public PointInfo(int ThisPointRadius, int StartPointMoveCost)
        {
            this.MoveCost = StartPointMoveCost + 1;
            this.FinishCost = ThisPointRadius;
        }

        public int CalculateCost()
        {
            return MoveCost + FinishCost;
        }
    }

}


