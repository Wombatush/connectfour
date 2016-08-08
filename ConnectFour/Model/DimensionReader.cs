namespace ConnectFour.Model
{
    using System.Linq;
    using ConnectFour.Wrappers;

    internal sealed class DimensionReader : IDimensionReader
    {
        private readonly IInputService inputService;
        private readonly IOutputService outputService;

        public DimensionReader(
            IInputService inputService, 
            IOutputService outputService)
        {
            Argument.IsNotNull(inputService, nameof(inputService));
            Argument.IsNotNull(outputService, nameof(outputService));

            this.inputService = inputService;
            this.outputService = outputService;
        }

        public void Read(out int columns, out int rows)
        {
            while (true)
            {
                var input = inputService.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    var splitted = input.Split().Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                    if (splitted.Length != 2)
                    {
                        outputService.WriteLine("Invalid entry: please enter two numbers separated by a white space");
                        continue;
                    }

                    if (!int.TryParse(splitted[0], out rows))
                    {
                        outputService.WriteLine("Invalid entry: the 'number of rows' cannot be parsed to number");
                        continue;
                    }

                    if (!int.TryParse(splitted[1], out columns))
                    {
                        outputService.WriteLine("Invalid entry: the 'number of columns' cannot be parsed to number");
                        continue;
                    }

                    break;
                }
            }
        }
    }
}