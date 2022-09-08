namespace FindingRoots;

/// <summary>
/// Class for separating roots
/// </summary>
public class RootSeparation
{
    public List<(double left, double right)> Result { get; set; } = new List<(double left, double right)> ();

    // private readonly double N;
    // private double counter = 0;
    private readonly double A;
    private readonly double B;
    private readonly Func<double, double> func;
    private readonly double step;

    /// <summary>
    /// Class constructor
    /// </summary>
    /// <param name="A">Left bound of the segment</param>
    /// <param name="B">Right bound of the segment</param>
    /// <param name="N">Constant value for counting step</param>
    /// <param name="func">Function</param>
    public RootSeparation(double A, double B, double N, Func<double, double> func)
    {
        //this.N = N;
        this.A = A;
        this.B = B;
        this.func = func;
        step = (B - A) / N;
        Separation();
    }

    public void Separation()
    {
        // List<(double left, double right)> boundaries = new();
        double x1 = A;
        double x2 = x1 + step;
        double y1 = func(x1);
        while (x2 <= B)
        {
            double y2 = func(x2);
            if (y1 * y2 <= 0)
            {
                // counter++;
                Result.Add((x1, x2));
            }
            x1 = x2;
            x2 += step;
            y1 = y2;
        }
    }
}
