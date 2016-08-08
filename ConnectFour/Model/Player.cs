namespace ConnectFour.Model
{
    internal struct Player
    {
        public Player(string playerName, char playerChar)
        {
            Argument.IsNotNull(playerName, nameof(playerName));

            PlayerName = playerName;
            PlayerChar = playerChar;
        }

        public string PlayerName { get; }

        public char PlayerChar { get; }

        public override string ToString() => PlayerName;
    }
}