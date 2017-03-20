using static System.Console;

namespace CSharp7Features
{
    public static class Literals
    {
        public static void Foo()
        {
            var i = 57_033;
            var x = 0xDE_C9;
            var b = 0b1101_1110_1100_1001;

            WriteLine($"Integer: {i:X} ({i})");
            WriteLine($"Hexadecimal: {x:X} ({i})");
            WriteLine($"Binary: {b:X} ({i})");
        }
    }
}
