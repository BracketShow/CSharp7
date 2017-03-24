using static System.Console;

namespace CSharp7Features
{
    public static class LocalFunctions
    {
        private static void SortOld(int[] numbers)
        {
            for(int i = 0; i < numbers.Length - 1; i++)
            {
                for (int j = i + 1; j < numbers.Length; j++)
                {
                    if (numbers[i] > numbers[j])
                    {
                        var temp = numbers[j];
                        numbers[j] = numbers[i];
                        numbers[i] = temp;
                    }
                }
            }
        }

        private static void SortNew(int[] numbers)
        {
            for (int i = 0; i < numbers.Length - 1; i++)
            {
                for (int j = i + 1; j < numbers.Length; j++)
                {
                    if (numbers[i] > numbers[j])
                    {
                        Swap(i, j);
                    }
                }
            }

            void Swap(int a, int b)
            {
                var temp = numbers[b];
                numbers[b] = numbers[a];
                numbers[a] = temp;
            }
        }

        public static void Foo()
        {
            // Old Way
            {
                var numbers = new[] { 5, 3, 1, 7, 8, 9, 0, 2, 4, 6 };
                WriteLine(string.Join(", ", numbers));
                SortOld(numbers);
                WriteLine(string.Join(", ", numbers));
            }

            // New Way
            {
                var numbers = new[] { 5, 3, 1, 7, 8, 9, 0, 2, 4, 6 };
                WriteLine(string.Join(", ", numbers));
                SortNew(numbers);
                WriteLine(string.Join(", ", numbers));
            }
        }
    }
}
