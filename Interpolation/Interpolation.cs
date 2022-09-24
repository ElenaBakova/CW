namespace Interpolation;

public class Interpolation
{
    private readonly int numberOfValues;
    private readonly double interpolationPoint;
    private readonly int degreeOfPolynomial;
    private readonly List<(double x, double result)> interpolationTable;
    private readonly Func<double, double> func = x =>
    {
        return Math.Sin(x) - (x * x / 2);
    };

    public Interpolation(int numberOfValues, double point, int degree, List<(double x, double result)> values)
    {
        this.numberOfValues = numberOfValues;
        interpolationPoint = point;
        degreeOfPolynomial = degree;
        interpolationTable = values;
        interpolationTable.Sort(((double, double) point1, (double, double) point2) =>
                    Math.Abs(point1.Item1 - interpolationPoint).CompareTo(Math.Abs(point2.Item1 - interpolationPoint)));
        Console.WriteLine("\nSorted table of points");
        interpolationTable.ForEach(item => Console.WriteLine($"{item.x} -- {item.result}"));
    }
}
