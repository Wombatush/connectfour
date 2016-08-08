namespace ConnectFour.Wrappers
{
    using System;

    internal sealed class ConsoleWrapper : IInputService, IOutputService
    {
        public ConsoleWrapper()
        {
            WriteLine("Press CTRL+C for exit");

            Console.CancelKeyPress += (sender, args) => Console.WriteLine("CTRL+C detected, terminating");
        }

        public string ReadLine() => Console.ReadLine();

        public void Write(string format, params object[] args) => Console.Write(format, args);

        public void WriteLine(string format, params object[] args) => Console.WriteLine(format, args);
    }
}