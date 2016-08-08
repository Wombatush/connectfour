namespace ConnectFour.Model
{
    internal interface IDimensionReader
    {
        void Read(out int columns, out int rows);
    }
}