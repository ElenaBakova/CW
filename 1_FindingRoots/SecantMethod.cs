namespace FindingRoots;

public class SecantMethod
{
    // Number of iterations -- m
    public int Iterations { get; private set; } = 0;

    // The result X
    public double Result { get; private set; }

    // Discrepancy
    public double Delta { get; private set; }

    public SecantMethod(double b, double a, double eps, Func<double, double> func)
    {
        double temp;
        do
        {
            temp = a;
            a -= func(a) / (func(a) - func(b)) * (a - b);
            b = temp;
            Iterations++;
        } while (Math.Abs(a - b) > eps);
        Result = a;
        Delta = Math.Abs(func(Result) - 0);
    }
}
