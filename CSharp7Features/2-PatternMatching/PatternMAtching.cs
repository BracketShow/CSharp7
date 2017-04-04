using System;
using static System.Console;

namespace CSharp7Features
{
    public static class PatternMatching
    {
        private static void ShowOld(Shape shape)
        {
            switch (shape.GetType().Name)
            {
                case "Circle":
                    var c = (Circle)shape;
                    WriteLine($"circle with radius {c.Radius}, Area: {c.Area()}");
                    break;
                case "Rectangle":
                    var r = (Rectangle)shape;
                    if (r.Length == r.Height)
                    {
                        WriteLine($"{r.Length} x {r.Height} square, Area: {r.Area()}");
                    }
                    else
                    {
                        WriteLine($"{r.Length} x {r.Height} rectangle, Area: {r.Area()}");
                    }
                    break;
                default:
                    WriteLine("<unknown shape>");
                    break;
                case null:
                    throw new ArgumentNullException(nameof(shape));
            }
        }

        private static void ShowNew(Shape shape)
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

            ShowOld(new Circle(5));
            ShowOld(new Rectangle(3, 5));
            ShowOld(new Rectangle(4, 4));

            ShowNew(new Circle(5));
            ShowNew(new Rectangle(3, 5));
            ShowNew(new Rectangle(4, 4));
        }
    }

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


}