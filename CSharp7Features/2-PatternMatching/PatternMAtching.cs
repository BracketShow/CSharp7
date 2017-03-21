using System;
using static System.Console;

namespace CSharp7Features
{
    public static class PatternMatching
    {
        abstract class Shape
        {
            public abstract double Area();
        }

        class Circle : Shape
        {
            public Circle(double radius) => Radius = radius;
            public double Radius { get; set; }
            public override double Area() => Math.PI * (Radius * Radius);
        }

        class Rectangle : Shape
        {
            public Rectangle(double length, double height) => (Length, Height) = (length, height);
            public double Length { get; set; }
            public double Height { get; set; }
            public override double Area() => Length * Height;
        }

        private static void Show(Shape shape)
        {
            switch (shape)
            {
                case Circle c:
                    WriteLine($"circle with radius {c.Radius}, Area: {c.Area()}");
                    break;
                case Rectangle r when (r.Length == r.Height):
                    WriteLine($"{r.Length} x {r.Height} square, Area: {r.Area()}");
                    break;
                case Rectangle r:
                    WriteLine($"{r.Length} x {r.Height} rectangle, Area: {r.Area()}");
                    break;
                default:
                    WriteLine("<unknown shape>");
                    break;
                case null:
                    throw new ArgumentNullException(nameof(shape));
            }
        }

        public static void OldWay()
        {
            object o = 1;
            if (o is int)
            {
                var i = (int)o;
                WriteLine($"o is int = {i}");
            }
        }

        public static void NewWay()
        {
            object o = 1;
            if (o is int i) WriteLine($"o is int = {i}");
        }

        public static void Foo()
        {
            OldWay();
            NewWay();

            Show(new Circle(5));
            Show(new Rectangle(3, 5));
            Show(new Rectangle(4, 4));
        }
    }
}