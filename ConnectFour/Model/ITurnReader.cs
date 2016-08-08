namespace ConnectFour.Model
{
    internal interface ITurnReader
    {
        void Read(out int column);
    }
}