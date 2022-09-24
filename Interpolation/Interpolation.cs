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

    public double LagrangeResult { get; private init; }

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

        LagrangeResult = Lagrange();

    }

    private double Lagrange()
    {
        double result = 0;

        for (int i = 0; i <= degreeOfPolynomial; i++)
        {
            result += Product(i, interpolationPoint) / Product(i, interpolationTable[i].x) * interpolationTable[i].result;
        }

        return result;
    }

    private double Product(int index, double point)
    {
        double result = 1;

        for (int i = 0; i <= degreeOfPolynomial; i++)
        {
            if (index != i)
            {
                result *= point - interpolationTable[i].x;
            }
        }

        return result;
    }
}
