namespace MoonWalker
{
    class PointInfo
    {
        // ���� �� ������������ �� ������ ������
        public readonly int MoveCost;

        // ���� �� ���������� ������(��� ����� �������)
        private readonly int FinishCost;

        /// <summary>
        /// ������ ����� ���������� � �����
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