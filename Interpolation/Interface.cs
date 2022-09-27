namespace Interpolation;

public static class Interface
{
    /// <summary>
    /// m + 1 number of points in table
    /// </summary>
    private static int numberOfValues;
    private static (int a, int b) segment;
    private static double interpolationPoint;

    /// <summary>
    /// n degree of interpolation polynomial
    /// </summary>
    private static int degreeOfPolynomial;
    private static readonly List<(double x, double result)> interpolationTable = new();
    private static readonly Func<double, double> func = x =>
    {
        return Math.Sin(x) - (x * x / 2);
    };

    public static void Run()
    {
        GetNumberOfValues();
        GetBoundaries();
        BuildTable();

        Console.WriteLine("Initial table of points");
        interpolationTable.ForEach(item => Console.WriteLine($"{item.x} -- {item.result}"));

        do
        {
            GetPoint();
            GetDegree();

            var interpolation = new Interpolation(interpolationPoint, degreeOfPolynomial, interpolationTable);
            Console.WriteLine($"\nThe value of interpolation polynomial. Lagrange form: {interpolation.LagrangeResult}");
            Console.WriteLine($"The error of interpolation: {Math.Abs(interpolation.LagrangeResult - func(interpolationPoint)):N20}");

            Console.WriteLine($"\nThe value of interpolation polynomial. Newtons form: {interpolation.NewtonResult}");
            Console.WriteLine($"The error of interpolation: {Math.Abs(interpolation.NewtonResult - func(interpolationPoint)):N20}");

            Console.WriteLine("\nWould you like to change interpolation point and polynomial degree?\nY - start over\nN - exit");
            while (true)
            {
                var read = Console.ReadLine() ?? "";
                if (read == "Y")
                {
                    break;
                }
                if (read == "N")
                {
                    return;
                }
            }
        } while (true);
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
        if (int.TryParse(Console.ReadLine(), out numberOfValues) == false || numberOfValues < 2)
        {
            Console.WriteLine("Invalid input. Please, try again");
            GetNumberOfValues();
            return;
        }
    }

    private static void GetBoundaries()
    {
        Console.WriteLine("\nPlease enter a and b -- boundaries of the segment");
        var read = Console.ReadLine() ?? "";
        var input = read.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (int.TryParse(input[0], out segment.a) == false || int.TryParse(input[1], out segment.b) == false)
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

    private static void GetPoint()
    {
        Console.WriteLine("\nPlease, enter interpolation point");
        if (double.TryParse(Console.ReadLine(), out interpolationPoint) == false)
        {
            Console.WriteLine("Invalid input. Please, try again");
            GetPoint();
            return;
        }
    }

    private static void GetDegree()
    {
        Console.WriteLine("\nPlease, enter degree of polynomial. It should be less than number of values");
        if (int.TryParse(Console.ReadLine(), out degreeOfPolynomial) == false || degreeOfPolynomial >= numberOfValues)
        {
            Console.WriteLine("Invalid input. Please, try again");
            GetDegree();
            return;
        }
    }
}
