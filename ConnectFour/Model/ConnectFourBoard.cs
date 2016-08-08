using System.Collections.Generic;
using System.Linq;

namespace ConnectFour.Model
{
    internal sealed class ConnectFourBoard : IConnectFourBoard
    {
        public const int MinColumns = 4;
        public const int MaxColumns = 10;
        public const int MinRows = 4;
        public const int MaxRows = 10;

        private const int MaxCountToWin = 4;

        private Player?[] board = new Player?[0];

        public int Rows { get; private set; }

        public int Columns { get; private set; }
        
        public void Reset(int columnCount, int rowCount)
        {
            Columns = columnCount;
            Rows = rowCount;
            board = new Player?[Columns * Rows];
        }

        public TurnResult Turn(Player player, int columnIdx)
        {
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

        private bool IsWinByColumn(Player player, int columnIdx)
        {
            var winStr = new string(player.PlayerChar, MaxCountToWin);
            var columnStr = new string(Enumerable.Range(0, Rows).Select(rowIdx => this.GetCellChar(columnIdx, rowIdx)).ToArray());
            return columnStr.Contains(winStr);
        }

        private bool IsWinByRow(Player player, int rowIdx)
        {
            var winStr = new string(player.PlayerChar, MaxCountToWin);
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

            var winStr = new string(player.PlayerChar, MaxCountToWin);
            var rowStrOne = new string(listOne.ToArray());
            var rowStrTwo = new string(listTwo.ToArray());
            return rowStrOne.Contains(winStr) || rowStrTwo.Contains(winStr);
        }

        public Player? GetCell(int columnIdx, int rowIdx)
        {
            Argument.IsGreaterThanOrEqualTo(columnIdx, 0, nameof(columnIdx));
            Argument.IsGreaterThanOrEqualTo(rowIdx, 0, nameof(rowIdx));
            Argument.IsLessThan(columnIdx, Columns, nameof(columnIdx));
            Argument.IsLessThan(rowIdx, Rows, nameof(rowIdx));

            return board[GetIndex(columnIdx, rowIdx)];
        }

        private void SetCell(int column, int row, Player player)
        {
            Argument.IsGreaterThanOrEqualTo(column, 0, nameof(column));
            Argument.IsGreaterThanOrEqualTo(row, 0, nameof(row));
            Argument.IsLessThan(column, Columns, nameof(column));
            Argument.IsLessThan(row, Rows, nameof(row));

            board[GetIndex(column, row)] = player;
        }

        private bool AreAnyFourConnected()
        {
            // by row
            for (var row = 0; row < Rows; ++row)
            {
                var lastPlayer = default(Player?);
                var lastCount = 0;
                for (var column = 0; column < Columns; ++column)
                {
                    var player = GetCell(column, row);
                    if (lastPlayer != null && lastPlayer.Value.PlayerChar == player?.PlayerChar)
                    {
                        if (++lastCount >= MaxCountToWin)
                        {
                            return true;
                        }

                        continue;
                    }

                    lastPlayer = player;
                    lastCount = 1;
                }

            }

            // by column
            for (var column = 0; column < Columns; ++column)
            {
                var lastPlayer = default(Player?);
                var lastCount = 0;
                for (var row = 0; row < Rows; ++row)
                {
                    var player = GetCell(column, row);
                    if (lastPlayer != null && lastPlayer.Value.PlayerChar == player?.PlayerChar)
                    {
                        if (++lastCount >= MaxCountToWin)
                        {
                            return true;
                        }

                        continue;
                    }

                    lastPlayer = player;
                    lastCount = 1;
                }
            }

            return false;
        }

        private int GetIndex(int column, int row)
        {
            Argument.IsGreaterThanOrEqualTo(column, 0, nameof(column));
            Argument.IsGreaterThanOrEqualTo(row, 0, nameof(row));
            Argument.IsLessThan(column, Columns, nameof(column));
            Argument.IsLessThan(row, Rows, nameof(row));

            return row * Columns + column;
        }
    }
}