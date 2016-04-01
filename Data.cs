namespace MoonWalker
{
    //test
    public class Data
    {
        /// <summary>
        ///     Направление движения
        /// </summary>
        public Direction D;

        /// <summary>
        ///     Карта луны(в пределах видимости)
        /// </summary>
        public Obstacle[,] Map = new Obstacle[15, 15];

        /// <summary>
        ///     Координаты лунохода
        /// </summary>
        public Coord XY;
    }

    /// <summary>
    ///     Всевозможные варианты объектов на карте
    /// </summary>
    public enum Obstacle
    {
        Empty,
        Rock,
        Hole,
        Mooners
    }

    /// <summary>
    ///     Направление движения лунохода
    /// </summary>
    public enum Direction
    {
        Up = 0,
        Left = 1,
        Right = 2,
        Down = 3,
        Unknown = 4
    }

    /// <summary>
    ///     Действие лунохода (Вперёд, назад, поворот направо, поворот налево)
    /// </summary>
    public enum Action
    {
        B,
        F,
        L,
        R
    }
}
