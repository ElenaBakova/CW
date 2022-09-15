using FindingRoots;

const double EPS = 1e-6;

Func<double, double> func = x =>
{
    return x - (10 * Math.Sin(x));
};

Func<double, double> funcDerivative = x =>
{
    return 1 - (10 * Math.Cos(x));
};

Func<double, double> funcSecondDerivative = x =>
{
    return 10 * Math.Sin(x);
};

static (bool res, double A, double B, double N) ReadData()
{
    Console.WriteLine("Please enter A - left bound and B - right bound");
    string[]? read = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
    if (read.Length < 2 || !double.TryParse(read[0], out double A) || !double.TryParse(read[1], out double B) || A > B)
    {
        Console.WriteLine("Invalid input\nPlease try again\n");
        return (false, 0, 0, 0);
    }

    Console.WriteLine("Please enter N: N >= 2");
    read = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
    if (read.Length < 1 || !double.TryParse(read[0], out double N) || N < 3)
    {
        Console.WriteLine("Invalid input\nPlease try again\n");
        return (false, 0, 0, 0);
    }
    //Console.WriteLine($"{A} {B} {N}");
    return (true, A, B, N);
}

static double FindInitPoint(double left, double right, Func<double, double> func, Func<double, double> funcSecondDerivative)
{
    double point = left;
    double step = Math.Abs(right + left) / 10000;
    for (; point < right; point += step)
    {
        if (func(point) * funcSecondDerivative(point) > 0)
        {
            return point;
        }
    }
    return right;
}

Console.WriteLine("Численные методы решения нелинейных уравнений");
Console.WriteLine($"A = -5, B = 3, function: x - 10*sin(x), e = 1e-6\n");

while (true)
{
    (bool res, double A, double B, double N) = ReadData();
    if (!res)
    {
        continue;
    }

    RootSeparation? separation = new(A, B, N, func);
    separation.Result.ForEach(segment => Console.WriteLine($"[{segment.left:N5}; {segment.right:N5}]"));
    Console.WriteLine($"Found {separation.Result.Count} segments\nWould you like to continue?\nY - continue\nN - try again");
    string? ans = Console.ReadLine();
    if (ans is null || ans.Length == 0 || ans[0] == 'N' || ans[0] == 'n')
    {
        continue;
    }

    Console.WriteLine("-------------------Starting bisection method-------------------");
    foreach ((double left, double right) in separation.Result)
    {
        Bisection? bisection = new(left, right, EPS, func);
        Console.WriteLine($"Initial approximation: {(left + right) / 2:N3}");
        Console.WriteLine($"Iterations: {bisection.Iterations}\nApproximate root: {bisection.Result:N15}");
        Console.WriteLine($"Discrepancy: {bisection.Delta:N15}\nLast segment length: {bisection.LastSegmentLength:N15}\n");
    }

    Console.WriteLine("-------------------Starting Newtons method-------------------");
    foreach ((double left, double right) in separation.Result)
    {
        double initPoint = FindInitPoint(left, right, func, funcSecondDerivative);

        NewtonsMethod? newtonsMethod = new(initPoint, EPS, 1, func, funcDerivative);
        Console.WriteLine($"Initial approximation: {initPoint:N3}\nIterations: {newtonsMethod.Iterations}");
        Console.WriteLine($"Approximate root: {newtonsMethod.Result:N15}\nDiscrepancy: {newtonsMethod.Delta:N15}\n");
    }

    Console.WriteLine("-------------------Starting modified Newtons method-------------------");
    foreach ((double left, double right) in separation.Result)
    {
        double initPoint = FindInitPoint(left, right, func, funcSecondDerivative);

        ModifiedNewton? modifiedNewton = new(initPoint, EPS, func, funcDerivative(initPoint));
        Console.WriteLine($"Initial approximation: {initPoint:N3}\nIterations: {modifiedNewton.Iterations}");
        Console.WriteLine($"Approximate root: {modifiedNewton.Result:N15}\nDiscrepancy: {modifiedNewton.Delta:N15}\n");
    }

    Console.WriteLine("-------------------Starting secant method-------------------");
    foreach ((double left, double right) in separation.Result)
    {
        SecantMethod? secantMethod = new(left, right, EPS, func);
        Console.WriteLine($"Initial approximation: {left:N3} and {right:N3}\nIterations: {secantMethod.Iterations}");
        Console.WriteLine($"Approximate root: {secantMethod.Result:N15}\nDiscrepancy: {secantMethod.Delta:N15}\n");
    }
}