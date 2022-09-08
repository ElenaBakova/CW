namespace FindingRoots;

public class Bisection
{
    public int Iterations { get; private set; } = 0;
    public double Result { get; private set; }
    public double FuncResult { get; private set; }

    public Bisection(double a, double b, double eps, Func<double, double> func)
    {
        //double mid;
        do
        {
            double mid = (a + b) / 2;
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
        FuncResult = func(Result);
    }
}
