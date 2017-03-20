using System;

namespace CSharp7Features
{
    public static class OutVar 
    {
        public static void OldWay()
        {
            int index;
            if (Int32.TryParse("1", out index))
            {
                Console.WriteLine($"Index is {index}");
            }
        }
        public static void NewWay()
        {
            if (Int32.TryParse("1", out int index))
            {
                Console.WriteLine($"Index is {index}");
            }
        }

        public static void Foo()
        {
            OldWay();
            NewWay();
        }
    }
}