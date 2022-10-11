namespace InversedInterpolation;

using Interpolation;
using FindingRoots;

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

    private static readonly List<(double x, double y)> interpolationTable = new();
    private static readonly Func<double, double> func = x => Math.Sin(x) - (x * x / 2);

    public static void Run()
    {
        GetNumberOfValues();
        GetBoundaries();
        BuildTable();

        Console.WriteLine("Initial table of points");
        interpolationTable.ForEach(item => Console.WriteLine($"{item.x} -- {item.y}"));

        GetValue();
        FirstMethod();
        SecondMethod();

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

    /// <summary>
    /// Finding X -- result of inversed interpolation
    /// </summary>
    private static void FirstMethod()
    {
        int inversedDegree = 0;
        GetDegree(ref inversedDegree);

        var instance = new Interpolation(pointValue, inversedDegree, interpolationTable.Select(value => (value.y, value.x)).ToList());
        var resultX = instance.NewtonResult;
        Console.WriteLine($"\nX -- result of inversed interpolation: {resultX:N15}");
        Console.WriteLine($"Error: {Math.Abs(func(resultX) - pointValue):N15}");
    }

    /// <summary>
    /// Finding roots of P_n = F
    /// </summary>
    private static void SecondMethod()
    {
        int degree = 0;
        double epsilon = 0;
        GetDegree(ref degree);
        GetEpsilon(ref epsilon);

        var polynomialFunction = new PolynomialFunction(degree, interpolationTable);
        Func<double, double> polynom = x => polynomialFunction.Newton(x) - pointValue;
        var separatedRoots = new RootSeparation(segment.a, segment.b, 100000, polynom);
        if (separatedRoots.Result == null || separatedRoots.Result.Count == 0)
        {
            Console.WriteLine("No segments was found");
        }
        else
        {
            foreach ((double left, double right) in separatedRoots.Result)
            {
                Bisection? bisection = new(segment.a, segment.b, epsilon, polynom);
                Console.WriteLine($"Approximate root of P_n = F: {bisection.Result:N15}");
                Console.WriteLine($"Error: {bisection.Delta:N15}\n");
            }
        }
    }

    private static void BuildTable()
    {
        interpolationTable.Clear();
        for (int i = 0; i < numberOfValues; i++)
        {
            double point = segment.a + (i * (segment.b - segment.a) / (numberOfValues - 1));
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
        string read = Console.ReadLine() ?? "";
        string[] input = read.Split(' ', StringSplitOptions.RemoveEmptyEntries);
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

    private static void GetDegree(ref int temp)
    {
        Console.WriteLine("\nPlease, enter degree of polynomial. It should be less than number of values");
        while (int.TryParse(Console.ReadLine(), out temp) == false || temp >= numberOfValues)
        {
            Console.WriteLine("Invalid input. Please, try again");
        }
    }
    
    private static void GetEpsilon(ref double temp)
    {
        Console.WriteLine("\nPlease, enter epsilon -- precision");
        while (double.TryParse(Console.ReadLine(), out temp) == false)
        {
            Console.WriteLine("Invalid input. Please, try again");
        }
    }
}
