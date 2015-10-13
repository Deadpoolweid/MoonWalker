namespace MoonWalker
{
    internal class PointInfo
    {
        // ���� �� ���������� ������(��� ����� �������)
        private readonly int FinishCost;
        // ���� �� ������������ �� ������ ������
        public readonly int MoveCost;

        /// <summary>
        ///     ������ ����� ���������� � �����
        /// </summary>
        /// <param name="thisPointRadius">������ ����� �����</param>
        /// <param name="startPointMoveCost">��������� ������������ ��������� �����</param>
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