namespace QuadratureFormulae;

public class Interface
{
    private static (double a, double b) limits;

    public static void Run()
    {
        GetLimits();
        var integrals = new Formulae(limits);
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

    private static void PrintValues(Formulae integrals)
    {
        double j = integrals.PreciseValue;

        Console.WriteLine(
                $"\nТочное значение: {integrals.PreciseValue}\n" +
                $"Формула левых прямоугольников: {integrals.LeftRectangle}\n" +
                $"Формула правых прямоугольников: {integrals.RightRectangle}\n" +
                $"Формула средних прямоугольников: {integrals.MiddleRectangle}\n" +
                $"Формула трапеций: {integrals.Trapezoidal}\n" +
                $"Формула Симпсона: {integrals.Simpsons}\n" +
                $"Формула трех восьмых: {integrals.ThreeEighths}\n");

        Console.WriteLine("Погрешности");
        Console.WriteLine(
                $"Формула левых прямоугольников: {Math.Abs(j - integrals.LeftRectangle)}\n" +
                $"Формула правых прямоугольников: {Math.Abs(j - integrals.RightRectangle)}\n" +
                $"Формула средних прямоугольников: {Math.Abs(j - integrals.MiddleRectangle)}\n" +
                $"Формула трапеций: {Math.Abs(j - integrals.Trapezoidal)}\n" +
                $"Формула Симпсона: {Math.Abs(j - integrals.Simpsons)}\n" +
                $"Формула трех восьмых: {Math.Abs(j - integrals.ThreeEighths)}\n");
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
}
