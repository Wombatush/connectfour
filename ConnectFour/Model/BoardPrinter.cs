namespace ConnectFour.Model
{
    using ConnectFour.Wrappers;

    internal sealed class BoardPrinter : IBoardPrinter
    {
        private readonly IOutputService outputService;

        public BoardPrinter(IOutputService outputService)
        {
            Argument.IsNotNull(outputService, nameof(outputService));

            this.outputService = outputService;
        }

        public void Print(IConnectFourBoard board)
        {
            Argument.IsNotNull(board, nameof(board));

            for (var y = 0; y < board.Rows; ++y)
            {
                for (var x = 0; x < board.Columns; ++x)
                {
                    var symbol = board.GetCellChar(x, y);

                    outputService.Write("{0}", symbol);
                }

                outputService.WriteLine();
            }
        }
    }
}