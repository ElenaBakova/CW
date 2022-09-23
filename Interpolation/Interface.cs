namespace Interpolation;

public static class Interface
{
    // m + 1
    private static int numberOfValues;
    private static (int a, int b) segment;
    private static double interpolationPoint;
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

        GetPoint();
        GetDegree();
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
        Console.WriteLine("Please enter number of values in table");
        if (int.TryParse(Console.ReadLine(), out numberOfValues) == false)
        {
            Console.WriteLine("Invalid input. Please, try again");
            GetNumberOfValues();
            return;
        }
    }

    private static void GetBoundaries()
    {
        Console.WriteLine("Please enter a and b -- boundaries of the segment");
        var input = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
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
        Console.WriteLine("Please, enter interpolation point");
        if (double.TryParse(Console.ReadLine(), out interpolationPoint) == false)
        {
            Console.WriteLine("Invalid input. Please, try again");
            GetPoint();
            return;
        }
    }

    private static void GetDegree()
    {
        Console.WriteLine("Please, enter degree of polynomial. It should be less than number of values");
        if (int.TryParse(Console.ReadLine(), out degreeOfPolynomial) == false || degreeOfPolynomial > numberOfValues)
        {
            Console.WriteLine("Invalid input. Please, try again");
            GetDegree();
            return;
        }
    }
}
