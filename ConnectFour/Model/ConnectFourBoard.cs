namespace ConnectFour.Model
{
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class ConnectFourBoard : IConnectFourBoard
    {
        public const int MinColumns = 4;
        public const int MaxColumns = 10;
        public const int MinRows = 4;
        public const int MaxRows = 10;

        internal const int CountToWin = 4;

        private Player?[] board = new Player?[MinRows * MinColumns];

        public int Rows { get; private set; } = MinRows;

        public int Columns { get; private set; } = MinColumns;

        public void Reset(int columnCount, int rowCount)
        {
            Argument.IsGreaterThanOrEqualTo(columnCount, MinColumns, nameof(columnCount));
            Argument.IsGreaterThanOrEqualTo(rowCount, MinRows, nameof(rowCount));
            Argument.IsLessThanOrEqualTo(columnCount, MaxColumns, nameof(columnCount));
            Argument.IsLessThanOrEqualTo(rowCount, MaxRows, nameof(rowCount));

            Columns = columnCount;
            Rows = rowCount;
            board = new Player?[Columns * Rows];
        }

        public TurnResult Turn(Player player, int columnIdx)
        {
            Argument.IsGreaterThanOrEqualTo(columnIdx, 0, nameof(columnIdx));
            Argument.IsLessThan(columnIdx, Columns, nameof(columnIdx));

            var rowIdx = Rows;

            while (--rowIdx >= 0)
            {
                var cell = GetCell(columnIdx, rowIdx);
                if (cell.HasValue)
                {
                    continue;
                }

                SetCell(columnIdx, rowIdx, player);

                if (IsWinByRow(player, rowIdx) 
                 || IsWinByColumn(player, columnIdx)
                 || IsWinByDiagonal(player, columnIdx, rowIdx))
                {
                    return TurnResult.Win;
                }

                if (board.All(x => x.HasValue))
                {
                    return TurnResult.Draw;
                }

                return TurnResult.Success;
            }

            return TurnResult.Invalid;
        }

        public Player? GetCell(int columnIdx, int rowIdx)
        {
            Argument.IsGreaterThanOrEqualTo(columnIdx, 0, nameof(columnIdx));
            Argument.IsGreaterThanOrEqualTo(rowIdx, 0, nameof(rowIdx));
            Argument.IsLessThan(columnIdx, Columns, nameof(columnIdx));
            Argument.IsLessThan(rowIdx, Rows, nameof(rowIdx));

            return board[GetIndex(columnIdx, rowIdx)];
        }

        private bool IsWinByColumn(Player player, int columnIdx)
        {
            var winStr = new string(player.PlayerChar, CountToWin);
            var columnStr = new string(Enumerable.Range(0, Rows).Select(rowIdx => this.GetCellChar(columnIdx, rowIdx)).ToArray());
            return columnStr.Contains(winStr);
        }

        private bool IsWinByRow(Player player, int rowIdx)
        {
            var winStr = new string(player.PlayerChar, CountToWin);
            var rowStr = new string(Enumerable.Range(0, Columns).Select(columnIdx => this.GetCellChar(columnIdx, rowIdx)).ToArray());
            return rowStr.Contains(winStr);
        }

        private bool IsWinByDiagonal(Player player, int columnIdx, int rowIdx)
        {
            var current = this.GetCellChar(columnIdx, rowIdx);
            var listOne = new List<char>(new[] { current });
            var listTwo = new List<char>(new[] { current });

            var localColumnIdx = columnIdx;
            var localRowIdx = rowIdx;
            while (++localColumnIdx < Columns && ++localRowIdx < Rows)
            {
                listOne.Add(this.GetCellChar(localColumnIdx, localRowIdx));
            }

            localColumnIdx = columnIdx;
            localRowIdx = rowIdx;
            while (++localColumnIdx < Columns && --localRowIdx >= 0)
            {
                listTwo.Add(this.GetCellChar(localColumnIdx, localRowIdx));
            }

            localColumnIdx = columnIdx;
            localRowIdx = rowIdx;
            while (--localColumnIdx >= 0 && --localRowIdx >= 0)
            {
                listOne.Insert(0, this.GetCellChar(localColumnIdx, localRowIdx));
            }

            localColumnIdx = columnIdx;
            localRowIdx = rowIdx;
            while (--localColumnIdx >= 0 && ++localRowIdx < Rows)
            {
                listTwo.Insert(0, this.GetCellChar(localColumnIdx, localRowIdx));
            }

            var winStr = new string(player.PlayerChar, CountToWin);
            var rowStrOne = new string(listOne.ToArray());
            var rowStrTwo = new string(listTwo.ToArray());
            return rowStrOne.Contains(winStr) || rowStrTwo.Contains(winStr);
        }

        private void SetCell(int columnIdx, int rowIdx, Player player)
        {
            board[GetIndex(columnIdx, rowIdx)] = player;
        }

        private int GetIndex(int column, int row)
        {
            return row * Columns + column;
        }
    }
}