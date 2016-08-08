using ConnectFour.Model;
using ConnectFour.Wrappers;

namespace ConnectFour
{
    class Program
    {
        static void Main(string[] args)
        {
            var playerReds = new Player("Reds", 'R');
            var playerYellows = new Player("Yellows", 'Y');
            var console = new ConsoleWrapper();
            var board = new ConnectFourBoard();
            var dimensionReader = new DimensionReader(console, console);
            var turnReader = new TurnReader(console, console);
            var boardPrinter = new BoardPrinter(console);
            var game = new ConnectFourGame(
                console,
                board,
                dimensionReader,
                turnReader,
                boardPrinter,
                playerYellows,
                playerReds);

            game.Loop();
        }
    }
}
