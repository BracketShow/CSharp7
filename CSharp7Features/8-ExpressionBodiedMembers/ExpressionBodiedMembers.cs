using System.Collections.Concurrent;
using System.Collections.Generic;
using static System.Console;

namespace CSharp7Features
{
    public static class ExpressionBodiedMembers
    {
        class Person
        {
            private static IList<string> names = new List<string>();
            private int id => names.Count - 1;


            public Person(string name) => names.Add(name); // constructors
            ~Person() => names.RemoveAt(id);
            public string Name
            {
                get => names[id];                                 // getters
                set => names[id] = value;                         // setters
            }
        }

        public static void Foo()
        {
            var person = new Person("test");
        }
    }
}
