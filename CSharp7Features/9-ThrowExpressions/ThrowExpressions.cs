using System;
using static System.Console;

namespace CSharp7Features
{
    public static class ThrowExpressions
    {
        class Person
        {
            public string Name { get; }
            public Person(string name) => Name = name ?? throw new ArgumentNullException(nameof(name));
            public string GetFirstName() => throw new NotImplementedException();
            public string GetLastName()
            {
                var parts = Name.Split(' ');
                return (parts.Length > 1) ? parts[1] : throw new InvalidOperationException("No space!");
            }
        }

        public static void Foo()
        {
            try
            {
                new Person(null);
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            try
            {
                new Person("Eric").GetFirstName();
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            try
            {
                new Person("Eric").GetLastName();
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }
        }
    }
}
