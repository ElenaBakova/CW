using System;

namespace NumDifferentiation;

public class Differentiation
{
    /// <summary>
    /// Initial function
    /// </summary>
    private static readonly Func<double, double> function = x => Math.Exp(3 * x);

    /// <summary>
    /// First derivative of function
    /// </summary>
    private static readonly Func<double, double> firstDerivative = x => 3 * Math.Exp(3 * x);

    /// <summary>
    /// Second derivative
    /// </summary>
    private static readonly Func<double, double> secondDerivative = x => 9 * Math.Exp(3 * x);

    private static List<(double x, double y)> tableOfPoints = new();

    public Differentiation(List<(double x, double y)> table)
    {
        tableOfPoints = table;
    }

    public static List<(double x, double y)> FirstDerivative()
    {
        List<(double x, double y)> result = new();

        for (int i = 0; i < tableOfPoints.Count; i++)
        {
            result.Add((tableOfPoints[i].x, Derivative(i)));
        }

        return result;
    }

    public static List<(double x, double y)> SecondDerivative()
    {
        List<(double x, double y)> result = new();

        for (int i = 0; i < tableOfPoints.Count; i++)
        {
            if (i == 0 || i == tableOfPoints.Count - 1)
            {
                result.Add((tableOfPoints[i].x, -1));
            }
            else
            {
                var derivative = (tableOfPoints[i + 1].y - 2 * tableOfPoints[i].y + tableOfPoints[i - 1].y)
                                 / Math.Pow(tableOfPoints[i + 1].x - tableOfPoints[i].x, 2); ;
                result.Add((tableOfPoints[i].x, derivative));
            }
        }
        return result;
    }

    private static double Derivative(int index)
    {
        if (index == 0)
        {
            if (tableOfPoints.Count > 2)
            {
                return (-3 * tableOfPoints[0].y + 4 * tableOfPoints[1].y - tableOfPoints[2].y) / (tableOfPoints[2].x - tableOfPoints[0].x);
            }

            return (tableOfPoints[1].y - tableOfPoints[0].y) / (tableOfPoints[1].x - tableOfPoints[0].x);
        }

        if (index == tableOfPoints.Count - 1)
        {
            if (tableOfPoints.Count > 2)
            {
                return (3 * tableOfPoints[index].y - 4 * tableOfPoints[index - 1].y + tableOfPoints[index - 2].y)
                       / (tableOfPoints[index].x - tableOfPoints[index - 2].x);
            }

            return (tableOfPoints[index].y - tableOfPoints[index - 1].y) / (tableOfPoints[index].x - tableOfPoints[index - 1].x);
        }

        return (tableOfPoints[index + 1].y - tableOfPoints[index - 1].y) / (tableOfPoints[index + 1].x - tableOfPoints[index - 1].x);
    }
}
