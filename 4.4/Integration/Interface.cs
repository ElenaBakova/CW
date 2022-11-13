namespace CompoundFormulae;

public class Interface
{
    // [A, B]
    private static (double a, double b) limits;

    private static int numberOfSegments = 0;

    public static void Run()
    {
        GetLimits();
        GetNumberOfSegments();
        var integrals = new Formulas(limits, numberOfSegments);
        PrintValues(integrals);

        Console.WriteLine("\nВвести новые данные?\nY - да\nN - нет");
        while (true)
        {
            string read = Console.ReadLine() ?? "";
            if (read == "Y" || read == "y")
            {
                Run();
                return;
            }
            else if (read == "N" || read == "n")
            {
                return;
            }
        }
    }

    private static void PrintValues(Formulas integrals)
    {
        Console.WriteLine($"Длина отрезка разбиения (h): {(limits.b - limits.a) / (numberOfSegments * 1.0)}");

        double j = integrals.PreciseValue;

        Console.WriteLine(
                $"\nТочное значение: {integrals.PreciseValue:N15}\n" +
                $"Формула левых прямоугольников: {integrals.LeftRectangle:N15}\n" +
                $"Формула правых прямоугольников: {integrals.RightRectangle:N15}\n" +
                $"Формула средних прямоугольников: {integrals.MiddleRectangle:N15}\n" +
                $"Формула трапеций: {integrals.Trapezoidal:N15}\n" +
                $"Формула Симпсона: {integrals.Simpsons:N15}\n");

        Console.WriteLine("Абсолютные фактические погрешности:");
        Console.WriteLine(
                $"Формула левых прямоугольников: {Math.Abs(j - integrals.LeftRectangle):N15}\n" +
                $"Формула правых прямоугольников: {Math.Abs(j - integrals.RightRectangle):N15}\n" +
                $"Формула средних прямоугольников: {Math.Abs(j - integrals.MiddleRectangle):N15}\n" +
                $"Формула трапеций: {Math.Abs(j - integrals.Trapezoidal):N15}\n" +
                $"Формула Симпсона: {Math.Abs(j - integrals.Simpsons):N15}\n");

        Console.WriteLine("Относительные фактические погрешности:");
        Console.WriteLine(
                $"Формула левых прямоугольников: {Math.Abs(j - integrals.LeftRectangle) / Math.Abs(j):N15}\n" +
                $"Формула правых прямоугольников: {Math.Abs(j - integrals.RightRectangle) / Math.Abs(j):N15}\n" +
                $"Формула средних прямоугольников: {Math.Abs(j - integrals.MiddleRectangle) / Math.Abs(j):N15}\n" +
                $"Формула трапеций: {Math.Abs(j - integrals.Trapezoidal) / Math.Abs(j):N15}\n" +
                $"Формула Симпсона: {Math.Abs(j - integrals.Simpsons) / Math.Abs(j):N15}\n");

        Console.WriteLine("Теоретические погрешности:");
        Console.WriteLine(
                $"Формула левых и правых прямоугольников: {integrals.LeftError:N15}\n" +
                $"Формула средних прямоугольников: {integrals.MiddleError:N15}\n" +
                $"Формула трапеций: {integrals.TrapezoidalError:N15}\n" +
                $"Формула Симпсона: {integrals.SimpsonsError:N15}\n");
    }

    private static void GetLimits()
    {
        Console.Write("Введите левый конец отрезка интегрирования: ");
        while (!double.TryParse(Console.ReadLine(), out limits.a))
        {
            Console.Write("Некорректное значение: введите вещественное число: ");
        }

        Console.Write("Введите правый конец отрезка интегрирования: ");
        while (!double.TryParse(Console.ReadLine(), out limits.b) || limits.b <= limits.a)
        {
            Console.Write("Некорректное значение: введите вещественное число, большее левого конца отрезка интегрирования: ");
        }
    }

    private static void GetNumberOfSegments()
    {
        Console.Write("Введите число промежутков деления отрезка интегрирования: ");

        //var numberOfSegments = 0;
        while (!int.TryParse(Console.ReadLine(), out numberOfSegments) || numberOfSegments <= 0)
        {
            Console.Write("Некорректное значение: введите положительное целое число: ");
        }
        //segmentLength = (limits.b - limits.a) / numberOfSegments;
    }
}