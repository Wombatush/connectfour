namespace ConnectFour.Model
{
    using ConnectFour.Wrappers;

    internal sealed class TurnReader : ITurnReader
    {
        private readonly IInputService inputService;
        private readonly IOutputService outputService;

        public TurnReader(
            IInputService inputService,
            IOutputService outputService)
        {
            Argument.IsNotNull(inputService, nameof(inputService));
            Argument.IsNotNull(outputService, nameof(outputService));

            this.inputService = inputService;
            this.outputService = outputService;
        }

        public void Read(out int column)
        {
            while (true)
            {
                var input = inputService.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (!int.TryParse(input, out column))
                    {
                        outputService.WriteLine("Invalid entry: the 'turn' cannot be parsed to number");
                        continue;
                    }

                    break;
                }
            }
        }
    }
}