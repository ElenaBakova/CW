namespace NumDifferentiation;

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

    private static readonly List<(double x, double result)> differentiationTable = new();
    private static readonly int k = 2;
    private static readonly Func<double, double> func = x =>
    {
        return Math.Exp(1.5 * k * x);
    };

    public static void Run()
    {
        GetNumberOfValues();
        GetStartPoint();
        GetStep();
        BuildTable();

        Console.WriteLine("Initial table of points");
        differentiationTable.ForEach(item => Console.WriteLine($"{item.x} -- {item.result}"));

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

    private static void GetNumberOfValues()
    {
        Console.WriteLine("\nPlease enter number of values in table");
        if (int.TryParse(Console.ReadLine(), out numberOfValues) == false || numberOfValues < 1)
        {
            Console.WriteLine("Invalid input. Please, try again");
            GetNumberOfValues();
        }
    }

    private static void GetStartPoint()
    {
        Console.WriteLine("\nPlease enter a -- start point");
        if (double.TryParse(Console.ReadLine(), out startPoint) == false)
        {
            Console.WriteLine("Invalid input. Please, try again");
            GetStartPoint();
        }
    }

    private static void GetStep()
    {
        Console.WriteLine("\nPlease enter h -- step");
        if (double.TryParse(Console.ReadLine(), out step) == false || step <= 0)
        {
            Console.WriteLine("Invalid input. Please, try again");
            GetStep();
        }
    }

    private static void BuildTable()
    {
        differentiationTable.Clear();
        for (int i = 0; i < numberOfValues; i++)
        {
            double point = startPoint + (i * step);
            differentiationTable.Add((point, func(point)));
        }
    }
}
