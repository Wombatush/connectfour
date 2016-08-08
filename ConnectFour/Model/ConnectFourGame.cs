namespace ConnectFour.Model
{
    using System;
    using ConnectFour.Wrappers;

    internal sealed class ConnectFourGame
    {
        private readonly IOutputService outputService;
        private readonly IConnectFourBoard board;
        private readonly IDimensionReader dimensionReader;
        private readonly ITurnReader turnReader;

        private readonly IBoardPrinter boardPrinter;

        private readonly Player[] players;

        public ConnectFourGame(
            IOutputService outputService,
            IConnectFourBoard board,
            IDimensionReader dimensionReader,
            ITurnReader turnReader,
            IBoardPrinter boardPrinter,
            params Player[] players)
        {
            Argument.IsNotNull(outputService, nameof(outputService));
            Argument.IsNotNull(board, nameof(board));
            Argument.IsNotNull(dimensionReader, nameof(dimensionReader));
            Argument.IsNotNull(turnReader, nameof(turnReader));
            Argument.IsNotNull(boardPrinter, nameof(boardPrinter));
            Argument.IsNotNull(players, nameof(players));

            this.outputService = outputService;
            this.board = board;
            this.dimensionReader = dimensionReader;
            this.turnReader = turnReader;
            this.boardPrinter = boardPrinter;
            this.players = players;
        }

        public void Loop()
        {
            while (true)
            {
                Play();
            }
        }

        private void Play()
        {
            int rows;
            int columns;
            ReadDimension(out columns, out rows);

            board.Reset(columns, rows);
            boardPrinter.Print(board);

            while (true)
            {
                foreach (var player in players)
                {
                    while (true)
                    {
                        int column;
                        ReadTurn(player, out column);

                        var result = board.Turn(player, column);
                        switch (result)
                        {
                            case TurnResult.Success:
                                boardPrinter.Print(board);
                                break;

                            case TurnResult.Invalid:
                                boardPrinter.Print(board);
                                outputService.WriteLine("Invalid entry: column is full");
                                continue;

                            case TurnResult.Draw:
                                boardPrinter.Print(board);
                                outputService.WriteLine("There is a draw");
                                return;

                            case TurnResult.Win:
                                boardPrinter.Print(board);
                                outputService.WriteLine("{0} WIN!", player);
                                return;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    }
                }
            }
        }

        private void ReadDimension(out int columns, out int rows)
        {
            while (true)
            {
                outputService.WriteLine();
                outputService.WriteLine("Please enter the board dimensions (number of rows, number of columns)");

                dimensionReader.Read(out columns, out rows);

                if (columns < ConnectFourBoard.MinColumns)
                {
                    outputService.WriteLine("Invalid entry: the 'number of columns' should be greater than or equal to {0}", ConnectFourBoard.MinColumns);
                    continue;
                }

                if (rows < ConnectFourBoard.MinRows)
                {
                    outputService.WriteLine("Invalid entry: the 'number of rows' should be greater than or equal to {0}", ConnectFourBoard.MinRows);
                    continue;
                }

                if (columns > ConnectFourBoard.MaxColumns)
                {
                    outputService.WriteLine("Invalid entry: the 'number of columns' should be less than or equal to {0}", ConnectFourBoard.MaxColumns);
                    continue;
                }

                if (rows > ConnectFourBoard.MaxRows)
                {
                    outputService.WriteLine("Invalid entry: the 'number of rows' should be less than or equal to {0}", ConnectFourBoard.MaxRows);
                    continue;
                }

                break;
            }
        }

        private void ReadTurn(Player player, out int columnIdx)
        {
            while (true)
            {
                outputService.WriteLine();
                outputService.WriteLine("{0} turn:", player);

                int column;
                turnReader.Read(out column);

                if (column < 1)
                {
                    outputService.WriteLine("Invalid entry: the 'turn' should be greater than or equal to 1");
                    continue;
                }

                if (column > board.Columns)
                {
                    outputService.WriteLine("Invalid entry: the 'turn' should be less than or equal to {0}", board.Columns);
                    continue;
                }

                columnIdx = --column;

                break;
            }
        }
    }
}