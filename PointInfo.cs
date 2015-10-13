namespace MoonWalker
{
    class PointInfo
    {
        // Цена за передвижение на данную клетку
        public readonly int MoveCost;

        // Цена за достижение финиша(без учёта преград)
        private readonly int FinishCost;

        /// <summary>
        /// Создаёт новую информацию о точке
        /// </summary>
        /// <param name="thisPointRadius">Радиус новой точки</param>
        /// <param name="startPointMoveCost">Стоимость передвижения стартовой точки</param>
        public PointInfo(int thisPointRadius, int startPointMoveCost)
        {
            MoveCost = startPointMoveCost + 1;
            FinishCost = thisPointRadius;
        }

        public int CalculateCost()
        {
            return MoveCost + FinishCost;
        }
    }
}