namespace CompoundFormulae;

public class Interface
{
    // [A, B]
    private static (double a, double b) limits;

    // m
    private static int numberOfSegments = 0;

    // l
    private static int parametr = 0;

    public static void Run()
    {
        GetLimits();
        GetNumberOfSegments();
        var integrals = new Formulas(limits, numberOfSegments);
        PrintValues(integrals, numberOfSegments, true);

        Console.WriteLine("---------------Значения с параметром m * l---------------");
        GetParam();
        var integralsNew = new Formulas(limits, numberOfSegments * parametr);
        PrintValues(integralsNew, numberOfSegments * parametr, false);

        Console.WriteLine("---------------Погрешности для уточненных значений---------------");
        PrintErrors(integrals, integralsNew);

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

    private static void PrintErrors(Formulas integrals, Formulas integralsNew)
    {
        double j = integrals.PreciseValue;
        double leftError = Math.Abs(j - Clarify(integrals.LeftRectangle, integralsNew.LeftRectangle, 1));
        double rightError = Math.Abs(j - Clarify(integrals.RightRectangle, integralsNew.RightRectangle, 1));
        double middleError = Math.Abs(j - Clarify(integrals.MiddleRectangle, integralsNew.MiddleRectangle, 2));
        double trapezoidalError = Math.Abs(j - Clarify(integrals.Trapezoidal, integralsNew.Trapezoidal, 2));
        double simpsonsError = Math.Abs(j - Clarify(integrals.Simpsons, integralsNew.Simpsons, 4));

        Console.WriteLine("Абсолютные фактические погрешности:");
        Console.WriteLine(
                $"Формула левых прямоугольников: {leftError:N15}\n" +
                $"Формула правых прямоугольников: {rightError:N15}\n" +
                $"Формула средних прямоугольников: {middleError:N15}\n" +
                $"Формула трапеций: {trapezoidalError:N15}\n" +
                $"Формула Симпсона: {simpsonsError:N15}\n");

        Console.WriteLine("Относительные фактические погрешности:");
        Console.WriteLine(
                $"Формула левых прямоугольников: {leftError / Math.Abs(j):N15}\n" +
                $"Формула правых прямоугольников: {rightError / Math.Abs(j):N15}\n" +
                $"Формула средних прямоугольников: {middleError / Math.Abs(j):N15}\n" +
                $"Формула трапеций: {trapezoidalError / Math.Abs(j):N15}\n" +
                $"Формула Симпсона: {simpsonsError / Math.Abs(j):N15}\n");
    }

    private static double Clarify(double jh, double jhl, int rValue)
    {
        double lPowr = Math.Pow(parametr, rValue * 1.0);
        return (lPowr * jhl - jh) / (lPowr - 1);
    }

    private static void PrintValues(Formulas integrals, int coefficient, bool flag)
    {
        Console.WriteLine($"\nДлина отрезка разбиения (h): {(limits.b - limits.a) / (coefficient * 1.0)}");

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

        if (!flag)
        {
            return;
        }

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

    private static void GetParam()
    {
        Console.Write("Введите параметр l: ");
        while (!int.TryParse(Console.ReadLine(), out parametr))
        {
            Console.Write("Некорректное значение");
        }
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