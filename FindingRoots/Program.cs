using FindingRoots;

const double EPS = 1e-6;

Func<double, double> func = x =>
{
    return x - (10 * Math.Sin(x));
};

static (bool res, double A, double B, double N) ReadData()
{
    Console.WriteLine("Please enter A - left bound and B - right bound");
    string[]? read = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
    if (read.Length < 2 || !double.TryParse(read[0], out double A) || !double.TryParse(read[1], out double B))
    {
        Console.WriteLine("Invalid input\nPlease try again\n");
        return (false, 0, 0, 0);
    }

    Console.WriteLine("Please enter N");
    read = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
    if (read.Length < 1 || !double.TryParse(read[0], out double N) || N < 0)
    {
        Console.WriteLine("Invalid input\nPlease try again\n");
        return (false, 0, 0, 0);
    }
    //Console.WriteLine($"{A} {B} {N}");
    return (true, A, B, N);
}

while (true)
{
    (bool res, double A, double B, double N) input = ReadData();
    if (!input.res)
    {
        continue;
    }
    RootSeparation? separation = new RootSeparation(input.A, input.B, input.N, func);
    separation.Result.ForEach(segment => Console.WriteLine($"[{segment.left}; {segment.right}]"));
    Console.WriteLine($"Found {separation.Result.Count} segments\nWould you like to continue?\nY - continue\nN - try again");
    var ans = Console.ReadLine();
    if (ans is null || ans.Length == 0 || ans[0] == 'N' || ans[0] == 'n')
    {
        continue;
    }

    Console.WriteLine("Starting bisection method");
}