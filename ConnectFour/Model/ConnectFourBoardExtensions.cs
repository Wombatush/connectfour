namespace ConnectFour.Model
{
    internal static class ConnectFourBoardExtensions
    {
        public static char GetCellChar(this IConnectFourBoard self, int columnIdx, int rowIdx, char emptyChar = 'o')
        {
            return self.GetCell(columnIdx, rowIdx)?.PlayerChar ?? emptyChar;
        }
    }
}