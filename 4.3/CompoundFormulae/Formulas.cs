namespace CompoundFormulae;

public class Formulas
{
    private static readonly Func<double, double> function = x => Math.Log(1 + x);
    private static readonly Func<double, double> antiderivative = x => (1 + x) * Math.Log(1 + x) - x;
    private static Func<double, double> Derivative(int order)
    {
        return order switch
        {
            1 => x => 1 / (1 + x),
            2 => x => -1 / Math.Pow(1 + x, 2),
            3 => x => 2 / Math.Pow(1 + x, 3),
            4 => x => -6 / Math.Pow(1 + x, 4),
            _ => throw new ArgumentException()
        };
    }

    /*private static readonly Func<double, double> function = x => 100;
    private static readonly Func<double, double> antiderivative = x => 100 * x;
    private static Func<double, double> Derivative(int order)
    {
        return order switch
        {
            _ => x => 0.0
        };
    }*/

    /*private static readonly Func<double, double> function = x => 7 * x;
    private static readonly Func<double, double> antiderivative = x => 7 * Math.Pow(x, 2) / 2.0;
    private static Func<double, double> Derivative(int order)
    {
        return order switch
        {
            1 => x => 7,
            _ => x => 0.0
        };
    }*/

    /*private static readonly Func<double, double> function = x => 7 * x * x;
    private static readonly Func<double, double> antiderivative = x => 7 * Math.Pow(x, 3) / 3.0;
    private static Func<double, double> Derivative(int order)
    {
        return order switch
        {
            1 => x => 14 * x,
            2 => x => 14.0,
            _ => x => 0.0
        };
    }*/

    /*private static readonly Func<double, double> function = x => 8 * Math.Pow(x, 3) + 6 * Math.Pow(x, 2) + 1.0;
    private static readonly Func<double, double> antiderivative = x => 2 * Math.Pow(x, 4) + 2 * Math.Pow(x, 3) + x;
    private static Func<double, double> Derivative(int order)
    {
        return order switch
        {
            1 => x => 12 * x * (2 * x + 1),
            2 => x => 48 * x + 12,
            3 => x => 48.0,
            4 => x => 0.0,
            _ => throw new ArgumentException()
        };
    }*/

    private static double MonotonousFunctionAbsMax(Func<double, double> function, double left, double right)
        => Math.Max(Math.Abs(function(right)), Math.Abs(function(left)));

    private static double WeightFunction(double x) => 1;

    public double PreciseValue { get; init; }
    public double LeftRectangle { get; init; }
    public double RightRectangle { get; init; }
    public double MiddleRectangle { get; init; }
    public double Trapezoidal { get; init; }
    public double Simpsons { get; init; }

    public Formulas((double a, double b) limits, double segmentLength)
    {
        PreciseValue = antiderivative(limits.b) - antiderivative(limits.a);
        LeftRectangle = (limits.b - limits.a) * function(limits.a);
        RightRectangle = (limits.b - limits.a) * function(limits.b);
        MiddleRectangle = (limits.b - limits.a) * function((limits.a + limits.b) / 2);
        Trapezoidal = (limits.b - limits.a) / 2 * (function(limits.a) + function(limits.b));

        double h = (limits.b - limits.a) / 3.0;
        Simpsons = h / 2 * (function(limits.a) + function(limits.b) + (4 * function((limits.a + limits.b) / 2)));
    }
}