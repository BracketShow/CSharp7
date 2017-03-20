using System;
using static System.Console;

namespace CSharp7Features
{
    public static class RefReturns
    {
        public static ref int Find(int number, int[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] == number)
                {
                    return ref numbers[i]; // return the storage location, not the value
                }
            }
            throw new IndexOutOfRangeException($"{nameof(number)} not found");
        }

        public static void Foo()
        {
            int[] array = { 1, 15, -39, 0, 7, 14, -12 };

            WriteLine(string.Join(", ", array));
            ref int found = ref Find(7, array); // aliases 7's place in the array
            found = 9; // replaces 7 with 9 in the array
            WriteLine(string.Join(", ", array));
        }
    }
}
