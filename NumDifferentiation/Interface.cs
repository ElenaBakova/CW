﻿namespace NumDifferentiation;

public static class Interface
{
    /// <summary>
    /// m + 1 number of points in table
    /// </summary>
    private static int numberOfValues;

    /// <summary>
    /// a
    /// </summary>
    private static double startPoint;

    /// <summary>
    /// h
    /// </summary>
    private static double step;

    /// <summary>
    /// Initial function
    /// </summary>
    private static readonly Func<double, double> function = x => Math.Exp(3 * x);

    /// <summary>
    /// First derivative of function
    /// </summary>
    private static readonly Func<double, double> firstDerivative = x => 3 * Math.Exp(3 * x);

    /// <summary>
    /// Second derivative
    /// </summary>
    private static readonly Func<double, double> secondDerivative = x => 9 * Math.Exp(3 * x);

    private static List<double> firstDerivativeValues = new();
    private static List<double> secondDerivativeValues = new();

    private static readonly List<(double x, double y)> tableOfPoints = new();

    public static void Run()
    {
        GetNumberOfValues();
        GetStartPoint();
        GetStep();
        BuildTable();

        Console.WriteLine("Initial table of points");
        tableOfPoints.Sort((first, second) => first.x.CompareTo(second.x));
        tableOfPoints.ForEach(item => Console.WriteLine($"{item.x} -- {item.y}"));

        var instance = new Differentiation(tableOfPoints, function);
        firstDerivativeValues = instance.FirstDerivative();
        secondDerivativeValues = instance.SecondDerivative();

        PrintInfoTable();

        Console.WriteLine("\nWould you like to change data?\nY - start over\nN - exit");
        while (true)
        {
            string read = Console.ReadLine() ?? "";
            if (read == "Y")
            {
                Run();
                return;
            }
            else if (read == "N")
            {
                return;
            }
        }
    }

    private static void PrintInfoTable()
    {
        Console.WriteLine("  x  /  f(x)  / f'(x)nd /  |f'(x)-f'(x)nd|  /  f' error  / f''(x)nd /  |f''(x)-f''(x)nd|  /  f'' error");
        for (int i = 0; i < tableOfPoints.Count; i++)
        {
            var secondDerivativeOutput = "";
            if (i == 0 || i == tableOfPoints.Count - 1)
            {
                secondDerivativeOutput = $"  --  /  --  /  --";
            }
            else
            {
                var error = Math.Abs(secondDerivative(tableOfPoints[i].x) - secondDerivativeValues[i]);
                secondDerivativeOutput = $" {secondDerivativeValues[i]}  / {error} / {error / secondDerivative(tableOfPoints[i].x)}";
            }

            var temp = Math.Abs(firstDerivative(tableOfPoints[i].x) - firstDerivativeValues[i]);
            Console.WriteLine($" {tableOfPoints[i].x} / {tableOfPoints[i].y} / {firstDerivativeValues[i]} / {temp} / {temp / firstDerivative(tableOfPoints[i].x)} / {secondDerivativeOutput}");
        }
    }

    private static void GetNumberOfValues()
    {
        Console.WriteLine("\nPlease enter number of values in table");
        while (int.TryParse(Console.ReadLine(), out numberOfValues) == false && numberOfValues < 2)
        {
            Console.WriteLine("Invalid input. Please, try again");
        }
    }

    private static void GetStartPoint()
    {
        Console.WriteLine("\nPlease enter a -- start point");
        while (double.TryParse(Console.ReadLine(), out startPoint) == false)
        {
            Console.WriteLine("Invalid input. Please, try again");
        }
    }

    private static void GetStep()
    {
        Console.WriteLine("\nPlease enter h -- step");
        while (double.TryParse(Console.ReadLine(), out step) == false && step <= 0)
        {
            Console.WriteLine("Invalid input. Please, try again");
        }
    }

    private static void BuildTable()
    {
        tableOfPoints.Clear();
        for (int i = 0; i < numberOfValues; i++)
        {
            double point = startPoint + (i * step);
            tableOfPoints.Add((point, function(point)));
        }
    }
}