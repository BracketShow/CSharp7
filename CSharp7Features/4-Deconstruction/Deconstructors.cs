using System;
using static System.Console;

namespace CSharp7Features
{
    public class Deconstructors
    {
        public class Person
        {
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }

            public Person(string firstName, string middleName, string lastName)
            {
                FirstName = firstName;
                MiddleName = middleName;
                LastName = lastName;
            }

            public void Deconstruct(out string firstName, out string middleName, out string lastName)
            {
                firstName = FirstName;
                middleName = MiddleName;
                lastName = LastName;
            }
        }

        public static void Foo()
        {
            var person = new Person ("Bob", "R.", "Smith");

            var _ = DateTime.Now;

            { // Old way
                string firstName;
                string middleName;
                string lastName;
                person.Deconstruct(out firstName, out middleName, out lastName);
                WriteLine($"{lastName}, {firstName} ({_})");
            }

            { // New way - deconstructing declaration
                (string firstName, string _, string lastName) = person;
                WriteLine($"{lastName}, {firstName} ({_})");
            }

            { // New way - var inside
                (var firstName, var _, var lastName) = person;
                WriteLine($"{lastName}, {firstName} ({_})");
            }

            { // New way - var outside
                var (firstName, _, lastName) = person;
                WriteLine($"{lastName}, {firstName} ({_})");
            }

            { // New way - deconstructing assignment
                string firstName, middleName, lastName;
                (firstName, middleName, lastName) = person;
                WriteLine($"{lastName}, {firstName} ({_})");
            }

        }
    }

    //public static class PersonExtensions
    //{
    //    public static void Deconstruct(this Deconstructors.Person person, out string firstName, out string middleName, out string lastName)
    //    {
    //        firstName = person.FirstName;
    //        middleName = person.MiddleName;
    //        lastName = person.LastName;
    //    }
    //}

}