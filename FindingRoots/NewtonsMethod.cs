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
        double oldResult = x0;
        do
        {
            Result = oldResult - (power * func(oldResult) / funcDerivative(oldResult));
            oldResult = Result;
            Iterations++;
        } while (Math.Abs(func(Result)) > eps);
        Delta = Math.Abs(func(Result) - 0);
    }
}
