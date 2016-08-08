using System;

namespace ConnectFour
{
    internal static class Argument
    {
        public static void IsNotNull<T>(T value, string paramName)
            where T : class
        {
            if (ReferenceEquals(value, null))
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void IsGreaterThanOrEqualTo<T>(T value, T comparand, string paramName)
            where T : IComparable<T>
        {
            if (value.CompareTo(comparand) < 0)
            {
                throw new ArgumentOutOfRangeException(paramName, value, $"Value has to be greater than or equal to {comparand}");
            }
        }

        public static void IsLessThan<T>(T value, T comparand, string paramName)
            where T : IComparable<T>
        {
            if (value.CompareTo(comparand) >= 0)
            {
                throw new ArgumentOutOfRangeException(paramName, value, $"Value has to be less than {comparand}");
            }
        }
    }
}