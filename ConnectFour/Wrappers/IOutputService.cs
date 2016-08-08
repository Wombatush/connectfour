namespace ConnectFour.Wrappers
{
    public interface IOutputService
    {
        void Write(string format, params object[] args);

        void WriteLine(string format = "", params object[] args);
    }
}