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

    /*private static readonly Func<double, double> function = x => Math.Exp(x);
    private static readonly Func<double, double> antiderivative = x => Math.Exp(x);
    private static Func<double, double> Derivative(int order)
    {
        return order switch
        {
            _ => (x) => Math.Exp(x)
        };
    }*/

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

    public double PreciseValue { get; init; }
    public double LeftRectangle { get; init; }
    public double RightRectangle { get; init; }
    public double MiddleRectangle { get; init; }
    public double Trapezoidal { get; init; }
    public double Simpsons { get; init; }

    public double LeftError { get; init; }
    public double RightError { get; init; }
    public double MiddleError { get; init; }
    public double TrapezoidalError { get; init; }
    public double SimpsonsError { get; init; }

    private static double TheoreticalError(double left, double right, double constant, int ast, double segmentLength)
    {
        return (right - left) * MonotonousFunctionAbsMax(Derivative(ast), left, right) * constant * Math.Pow(segmentLength, ast);
    }

    private static double MonotonousFunctionAbsMax(Func<double, double> function, double left, double right)
        => Math.Max(Math.Abs(function(left)), Math.Abs(function(right)));

    public Formulas((double a, double b) limits, int numberOfSegments)
    {
        double hValue = (limits.b - limits.a) / numberOfSegments;

        PreciseValue = antiderivative(limits.b) - antiderivative(limits.a);

        double firstValue = 0.0;
        double lastValue = function(limits.b);
        double wSum = 0.0;
        double qSum = 0.0;
        for (var i = 0; i < numberOfSegments; ++i)
        {
            var value = function(limits.a + hValue * i);

            if (i == 0)
            {
                firstValue = value;
            }
            else
            {
                wSum += value;
            }

            qSum += function(limits.a + hValue * i + hValue / 2);
        }

        LeftRectangle = hValue * (firstValue + wSum);
        RightRectangle = hValue * (lastValue + wSum);
        MiddleRectangle = hValue * qSum;
        Trapezoidal = hValue * 0.5 * (firstValue + wSum * 2 + lastValue);
        Simpsons = hValue / 6.0 * (firstValue + wSum * 2 + lastValue + 4 * qSum);

        LeftError = TheoreticalError(limits.a, limits.b, 0.5, 1, hValue);
        RightError = LeftError;
        MiddleError = TheoreticalError(limits.a, limits.b, 1 / 24.0, 2, hValue);
        TrapezoidalError = TheoreticalError(limits.a, limits.b, 1 / 12.0, 2, hValue);
        SimpsonsError = TheoreticalError(limits.a, limits.b, 1 / 2880.0, 4, hValue);
    }
}