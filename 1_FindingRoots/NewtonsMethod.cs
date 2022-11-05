namespace FindingRoots;

public class NewtonsMethod
{
    // Number of iterations -- m
    public int Iterations { get; private set; } = 0;

    // The result X
    public double Result { get; private set; }

    // Discrepancy
    public double Delta { get; private set; }

    public NewtonsMethod(double x0, double eps, int power, Func<double, double> func, Func<double, double> funcDerivative)
    {
        double oldResult;
        Result = x0;
        do
        {
            oldResult = Result;
            Result = oldResult - (power * func(oldResult) / funcDerivative(oldResult));
            Iterations++;
        } while (Math.Abs(oldResult - Result) > eps);
        Delta = Math.Abs(func(Result) - 0);
    }
}
