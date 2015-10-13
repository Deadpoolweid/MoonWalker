using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace MoonWalker
{
    /// <summary>
    ///     Ввод данных
    /// </summary>
    public static class Input
    {
        /// <summary>
        ///     Запрашивает данные у пользователя и записывает их в хранилище
        /// </summary>
        /// <returns>Экземпляр хранилища данных</returns>
        public static Data Core()
        {
            Contract.Ensures(Contract.Result<Data>() != null);
            // Создаём экземпляр хранилища данных
            var data = new Data();
            // Считываем первую строчку
            var line = ReadFirst();
            // Сохраняем координаты из первой строчки в хранилище
            data.XY = GetCoord(line);
            // Сохраняем направление из первой строчки в хранилище
            data.D = GetDirection(line);
            // Считываем карту видимости лунохода
            data.Map = GetMap(ReadMap());
            // Отправляем хранилище с данными для дальнейшей обработки
            return data;
        }

        /// <summary>
        ///     Запрашивает у пользователя координаты и направление
        /// </summary>
        /// <returns>Координаты лунохода</returns>
        private static string[] ReadFirst()
        {
            Contract.Ensures(Contract.Result<string[]>() != null);
            var rez = new string[3];
            var line = Console.ReadLine();
            if (line != null)
            {
                rez = Array.ConvertAll(line.Split(' '), Convert.ToString);
            }

            if (rez.Length > 3)
            {
                throw new ArgumentException("Неправильный ввод.");
            }
            return rez;
        }

        /// <summary>
        ///     Читает строку и переводит её в массив символов
        /// </summary>
        /// <returns>Массив символов из введённой строки</returns>
        private static IEnumerable<char> ReadNext()
        {
            Contract.Ensures(Contract.Result<IEnumerable<char>>() != null);
            var line = Console.ReadLine();
            char[] rez;
            if (line != null)
            {
                rez = line.ToCharArray();
            }
            else
            {
                return null;
            }


            if (rez.Length > 15 && rez.Length < 15)
            {
                throw new ArgumentException("Неправильный ввод.");
            }

            return rez;
        }

        /// <summary>
        ///     Запрашивает у пользователя карту видимости
        /// </summary>
        /// <returns>Символьный массив карты</returns>
        private static char[,] ReadMap()
        {
            var map = new char[15, 15];

            var q = 0;
            for (var i = 0; i < 15; i++)
            {
                var next = ReadNext();
                foreach (var variable in next)
                {
                    if (q > 14)
                    {
                        q = 0;
                    }
                    map[q, i] = variable;
                    q++;
                }
            }

            return map;
        }

        /// <summary>
        ///     Получает координаты из двух строк
        /// </summary>
        /// <param name="line">Строки с координатами</param>
        /// <returns>Координаты</returns>
        private static Coord GetCoord(string[] line)
        {
            Contract.Ensures(Contract.Result<Coord>() != null);
            Contract.Ensures(Math.Abs(int.Parse(line[0])) < 1000 && Math.Abs(int.Parse(line[1])) < 1000,
                "Значения координат находятся за пределами допустимых.\n");
            var coord = new Coord(int.Parse(line[0]), int.Parse(line[1]));
            return coord;
        }

        /// <summary>
        ///     Получает направление движения из строки
        /// </summary>
        /// <returns>Направление движения</returns>
        private static Direction GetDirection(string[] line)
        {
            Contract.Ensures(Enum.IsDefined(typeof (Direction), Contract.Result<Direction>()));
            int choose;
            var d = Convert.ToChar(line[2].ToUpper());
            switch (d)
            {
                case 'U':
                    choose = 0;
                    break;
                case 'L':
                    choose = 1;
                    break;
                case 'R':
                    choose = 2;
                    break;
                case 'D':
                    choose = 3;
                    break;
                default:
                    choose = 4;
                    break;
            }
            var p = (Direction) choose;
            if (p == Direction.Unknown)
            {
                throw new ArgumentException("Неизвестное направление.");
            }
            return p;
        }

        /// <summary>
        ///     Получает карту из массива символов
        /// </summary>
        /// <param name="map">Массив символов</param>
        /// <returns>Карта</returns>
        private static Obstacle[,] GetMap(char[,] map)
        {
            var obstacles = map;

            var rObstacles = new Obstacle[15, 15];

            for (var i = 0; i < 15; i++)
            {
                for (var j = 0; j < 15; j++)
                {
                    if (obstacles[j, i] == '.')
                    {
                        rObstacles[j, i] = Obstacle.Empty;
                    }
                    else if (obstacles[j, i] == '*')
                    {
                        rObstacles[j, i] = Obstacle.Rock;
                    }
                    else if (obstacles[j, i] == '0')
                    {
                        rObstacles[j, i] = Obstacle.Hole;
                    }
                    else if (obstacles[j, i] == '!')
                    {
                        rObstacles[j, i] = Obstacle.Mooners;
                    }
                    else
                    {
                        throw new ArgumentException("В области видимости присутствуют недопустимые символы: " +
                                                    obstacles[j, i]);
                    }
                }
            }

            return rObstacles;
        }
    }
}