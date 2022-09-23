namespace Interpolation;

public static class Interface
{
    private static int numberOfValues;
    private static (int a, int b) segment;
    private static float interpolationPoint;

    public static void Run()
    {
        GetNumberOfValues();
        GetBoundaries();
        GetPoint();
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
    }

    private static void GetPoint()
    {
        Console.WriteLine("Please, enter interpolation point");
        if (float.TryParse(Console.ReadLine(), out interpolationPoint) == false)
        {
            Console.WriteLine("Invalid input. Please, try again");
            GetPoint();
            return;
        }
    }
}
