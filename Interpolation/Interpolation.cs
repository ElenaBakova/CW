namespace Interpolation;

public class Interpolation
{
    private readonly int numberOfValues;
    private readonly double interpolationPoint;
    private readonly int degreeOfPolynomial;
    private readonly List<(double x, double result)> interpolationTable;
    private readonly List<List<double>> dividedDifferences = new();

    private readonly Func<double, double> func = x =>
    {
        return Math.Sin(x) - (x * x / 2);
    };

    /// <summary>
    /// Result of Lagrange polynomial in the interpolation point
    /// </summary>
    public double LagrangeResult { get; private init; }

    /// <summary>
    /// Result of Newton polynomial in the interpolation point
    /// </summary>
    public double NewtonResult { get; private init; }

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

        dividedDifferences.Clear();
        for (var i = 0; i <= degreeOfPolynomial; i++)
        {
            dividedDifferences.Add(new List<double>());
        }
        GetDifferences();
        NewtonResult = Newton();
    }

    private double Newton()
    {
        double result = 0;

        for (int i = 0; i <= degreeOfPolynomial; i++)
        {
            result += dividedDifferences[i][0] * Product(i);
        }

        return result;
    }

    private double Product(int upperBound)
    {
        double result = 1;

        for (int i = 0; i < upperBound; i++)
        {
            result *= interpolationPoint - interpolationTable[i].x;
        }

        return result;
    }

    private void GetDifferences()
    {
        for (int i = 0; i <= degreeOfPolynomial; i++)
        {
            if (i == 0)
            {
                for (int j = 0; j <= degreeOfPolynomial; j++)
                {
                    dividedDifferences[i].Add(interpolationTable[j].result);
                }
            }
            else
            {
                for (int j = 0; j <= degreeOfPolynomial - i; j++)
                {
                    var item = (dividedDifferences[i - 1][j + 1] - dividedDifferences[i - 1][j]) / (interpolationTable[j + 1].x - interpolationTable[j].x);
                    dividedDifferences[i].Add(item);
                }
            }
        }
    }

    private double Lagrange()
    {
        double result = 0;

        for (int i = 0; i <= degreeOfPolynomial; i++)
        {
            result += ConditionalProduct(i, interpolationPoint) / ConditionalProduct(i, interpolationTable[i].x) * interpolationTable[i].result;
        }

        return result;
    }

    private double ConditionalProduct(int index, double point)
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
