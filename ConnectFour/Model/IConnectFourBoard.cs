namespace ConnectFour.Model
{
    internal interface IConnectFourBoard
    {
        int Rows { get; }

        int Columns { get; }

        void Reset(int columnCount, int rowCount);

        TurnResult Turn(Player player, int columnIdx);

        Player? GetCell(int columnIdx, int rowIdx);
    }
}