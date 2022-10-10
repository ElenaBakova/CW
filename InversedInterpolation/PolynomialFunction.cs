namespace InversedInterpolation;

public class PolynomialFunction
{
    private readonly int degreeOfPolynomial;
    private readonly List<(double x, double result)> interpolationTable;
    private readonly List<List<double>> dividedDifferences = new();

    private double point;

    public PolynomialFunction(int degree, List<(double x, double result)> values)
    {
        degreeOfPolynomial = degree;
        interpolationTable = values;
    }

    public double Newton(double point)
    {
        this.point = point;
        interpolationTable.Sort(((double, double) point1, (double, double) point2) =>
                    Math.Abs(point1.Item1 - point).CompareTo(Math.Abs(point2.Item1 - point)));

        dividedDifferences.Clear();
        for (var i = 0; i <= degreeOfPolynomial; i++)
        {
            dividedDifferences.Add(new List<double>());
        }
        GetDifferences();
        return NewtonFunction();
    }

    private double NewtonFunction()
    {
        double result = 0;

        for (int i = 0; i <= degreeOfPolynomial; i++)
        {
            result += dividedDifferences[i][0] * Product(i);
            // Console.WriteLine($"---------------------{dividedDifferences[i][0]}");
        }

        return result;
    }

    private double Product(int upperBound)
    {
        double result = 1;

        for (int i = 0; i < upperBound; i++)
        {
            result *= point - interpolationTable[i].x;
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
                    var item = (dividedDifferences[i - 1][j + 1] - dividedDifferences[i - 1][j]) / (interpolationTable[j + i].x - interpolationTable[j].x);
                    dividedDifferences[i].Add(item);
                }
            }
        }
    }
}
