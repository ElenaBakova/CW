namespace InversedInterpolation;

using Interpolation;

public static class Interface
{
    /// <summary>
    /// m + 1 number of points in table
    /// </summary>
    private static int numberOfValues;

    /// <summary>
    /// Segment of interpolation
    /// </summary>
    private static (double a, double b) segment;

    /// <summary>
    /// F
    /// </summary>
    private static double pointValue;

    /// <summary>
    /// n degree of interpolation polynomial
    /// </summary>
    private static int degree;

    /// <summary>
    /// degree of inversed interpolation polynomial
    /// </summary>
    private static int inversedDegree;

    /// <summary>
    /// EPS
    /// </summary>
    private static double epsilon;

    private static readonly List<(double x, double y)> interpolationTable = new();
    private static readonly Func<double, double> func = x => (Math.Sin(x) - (x * x / 2));

    public static void Run()
    {
        GetNumberOfValues();
        GetBoundaries();
        BuildTable();

        Console.WriteLine("Initial table of points");
        interpolationTable.ForEach(item => Console.WriteLine($"{item.x} -- {item.y}"));

        FirstMethod();
        //for the second method
        //GetDegree(ref degree);

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

    private static void FirstMethod()
    {
        GetValue();
        GetDegree(ref inversedDegree);
        var instance = new Interpolation(pointValue, inversedDegree, interpolationTable.Select(value => (value.y, value.x)).ToList());
        var resultX = instance.LagrangeResult;
        Console.WriteLine($"\nX -- result of inversed interpolation: {resultX}");
        Console.WriteLine($"Error: {Math.Abs(func(resultX) - pointValue)}");
    }

    private static void BuildTable()
    {
        interpolationTable.Clear();
        for (int i = 0; i < numberOfValues; i++)
        {
            double point = segment.a + (i * ((segment.b * 1.0) - segment.a) / (numberOfValues - 1.0));
            interpolationTable.Add((point, func(point)));
        }
    }

    private static void GetNumberOfValues()
    {
        Console.WriteLine("\nPlease enter number of values in table");
        while (int.TryParse(Console.ReadLine(), out numberOfValues) == false || numberOfValues < 2)
        {
            Console.WriteLine("Invalid input. Please, try again");
        }
    }

    private static void GetBoundaries()
    {
        Console.WriteLine("\nPlease enter a and b -- boundaries of the segment");
        var read = Console.ReadLine() ?? "";
        var input = read.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (double.TryParse(input[0], out segment.a) == false || double.TryParse(input[1], out segment.b) == false)
        {
            Console.WriteLine("Invalid input. Please, try again");
            GetBoundaries();
            return;
        }
        if (segment.a > segment.b)
        {
            Console.WriteLine("Left bound should be less than right. Please, try again");
            GetBoundaries();
            return;
        }
    }

    private static void GetValue()
    {
        Console.WriteLine("\nPlease, enter value of function in interpolation point");
        while (double.TryParse(Console.ReadLine(), out pointValue) == false)
        {
            Console.WriteLine("Invalid input. Please, try again");
        }
    }

    /*private static void GetPoint()
    {
        Console.WriteLine("\nPlease, enter interpolation point");
        while (double.TryParse(Console.ReadLine(), out interpolationPoint) == false)
        {
            Console.WriteLine("Invalid input. Please, try again");
        }
    }*/

    private static void GetDegree(ref int temp)
    {
        Console.WriteLine("\nPlease, enter degree of polynomial. It should be less than number of values");
        while (int.TryParse(Console.ReadLine(), out temp) == false || temp >= numberOfValues)
        {
            Console.WriteLine("Invalid input. Please, try again");
        }
    }
}
