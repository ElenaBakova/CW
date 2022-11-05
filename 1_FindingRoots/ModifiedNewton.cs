namespace FindingRoots;

public class ModifiedNewton
{
    // Number of iterations -- m
    public int Iterations { get; private set; } = 0;

    // The result X
    public double Result { get; private set; }

    // Discrepancy
    public double Delta { get; private set; }

    public ModifiedNewton(double x0, double eps, Func<double, double> func, double derivative)
    {
        double oldResult;
        Result = x0;
        do
        {
            oldResult = Result;
            Result = oldResult - (func(oldResult) / derivative);
            Iterations++;
        } while (Math.Abs(oldResult - Result) > eps);
        Delta = Math.Abs(func(Result) - 0);
    }
}
