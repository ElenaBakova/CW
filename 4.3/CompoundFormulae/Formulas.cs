namespace CompoundFormulae;

public class Formulas
{
    private static readonly Func<double, double> function = x => Math.Log(1 + x);
    private static readonly Func<double, double> antiderivative = x => (1 + x) * Math.Log(1 + x) - x;

    /*private static readonly Func<double, double> function = x => x;
    private static readonly Func<double, double> antiderivative = x => x * x / 2;*/

    /*private static readonly Func<double, double> function = x => x * x;
    private static readonly Func<double, double> antiderivative = x => x * x * x / 3;*/

    /*private static readonly Func<double, double> function = x => x * x * x;
    private static readonly Func<double, double> antiderivative = x => x * x * x * x / 4;*/

    public double PreciseValue = antiderivative(1) - antiderivative(0);
    public double LeftRectangle { get; init; }
    public double RightRectangle { get; init; }
    public double MiddleRectangle { get; init; }
    public double Trapezoidal { get; init; }
    public double Simpsons { get; init; }
    public double ThreeEighths { get; init; }

    public Formulas((double a, double b) limits)
    {
        LeftRectangle = (limits.b - limits.a) * function(limits.a);
        RightRectangle = (limits.b - limits.a) * function(limits.b);
        MiddleRectangle = (limits.b - limits.a) * function((limits.a + limits.b) / 2);
        Trapezoidal = (limits.b - limits.a) / 2 * (function(limits.a) + function(limits.b));

        double h = (limits.b - limits.a) / 3.0;
        Simpsons = h / 2 * (function(limits.a) + function(limits.b) + (4 * function((limits.a + limits.b) / 2)));
        ThreeEighths = (limits.b - limits.a) * ((function(limits.a) / 8.0) + (function(limits.b) / 8.0)
            + (3.0 / 8 * function(limits.a + h)) + (3.0 / 8 * function(limits.a + (2 * h))));
    }
}