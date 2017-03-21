using System.Collections.Concurrent;
using System.Collections.Generic;
using static System.Console;

namespace CSharp7Features
{
    public static class ExpressionBodiedMembers
    {
        class Person
        {
            private static ConcurrentDictionary<int, string> names = new ConcurrentDictionary<int, string>();
            private int id = names.Count;

            public Person(string name) => names.TryAdd(id, name); // constructors
            ~Person() => names.TryRemove(id, out _);              // destructors
            public string Name
            {
                get => names[id];                                 // getters
                set => names[id] = value;                         // setters
            }
        }

        public static void Foo()
        {
            var person1 = new Person("test1");
            WriteLine($"Person: {person1.Name}");
            var person2 = new Person("test2");
            WriteLine($"Person: {person2.Name}");
        }
    }
}
