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

    private static readonly List<(double x, double y)> tableOfPoints = new();
    private static readonly int k = 2;
    private static readonly Func<double, double> func = x => Math.Exp(1.5 * k * x);

    public static void Run()
    {
        GetNumberOfValues();
        GetStartPoint();
        GetStep();
        BuildTable();

        Console.WriteLine("Initial table of points");
        tableOfPoints.ForEach(item => Console.WriteLine($"{item.x} -- {item.y}"));

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
            tableOfPoints.Add((point, func(point)));
        }
    }
}
