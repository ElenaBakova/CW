namespace FindingRoots;

public class Bisection
{
    // Number of iterations -- m
    public int Iterations { get; private set; } = 0;

    // The result X
    public double Result { get; private set; }

    // Discrepancy
    public double Delta { get; private set; }

    // Length of the last sement: |x_m - x_m-1|
    public double LastSegmentLength { get; private set; }

    public Bisection(double a, double b, double eps, Func<double, double> func)
    {
        double mid;
        do
        {
            mid = (a + b) / 2;
            if (func(a) * func(mid) <= 0)
            {
                b = mid;
            }
            else
            {
                a = mid;
            }
            Iterations++;
        } while (b - a > 2 * eps);
        Result = (a + b) / 2;
        Delta = Math.Abs(func(Result) - 0);
        LastSegmentLength = Math.Abs(Result - mid);
    }
}
