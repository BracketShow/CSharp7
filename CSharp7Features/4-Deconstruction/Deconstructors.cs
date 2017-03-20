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

            public void Deconstruct(out string firstName, out string middleName, out string lastName)
            {
                firstName = FirstName;
                middleName = MiddleName;
                lastName = LastName;
            }
        }

        public static void Foo()
        {
            var person = new Person
            {
                FirstName = "Bob",
                MiddleName = "R.",
                LastName = "Smith"
            };

            var _ = DateTime.Now;

            { // Old way
                string firstName;
                string middleName;
                string lastName;
                person.Deconstruct(out firstName, out middleName, out lastName);
                WriteLine($"{lastName}, {firstName} ({_})");
            }

            { // New way
                var (firstName, _, lastName) = person;
                WriteLine($"{lastName}, {firstName} ({_})");
            }
        }
    }

    public static class PersonExtensions
    {
        public static void Deconstruct(this Deconstructors.Person person, out string firstName, out string middleName, out string lastName)
        {
            firstName = person.FirstName;
            middleName = person.MiddleName;
            lastName = person.LastName;
        }
    }

}