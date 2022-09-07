using FindingRoots;

int A = 0;
int B = 0;
int N = 0;

while (true)
{
    Console.WriteLine("Please enter A - left bound and B - right bound");
    var read = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
    if (read.Length < 2 || !int.TryParse(read[0], out A) || !int.TryParse(read[1], out B))
    {
        Console.WriteLine("Invalid input\nPlease try again\n");
        continue;
    }

    Console.WriteLine("Please enter N");
    read = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
    if (read.Length < 1 || !int.TryParse(read[0], out N) || N < 0)
    {
        Console.WriteLine("Invalid input\nPlease try again\n");
        continue;
    }
    //Console.WriteLine($"{A} {B} {N}");

    var separation = new RootsSeparation(A, B, N, x => x - 10);// * Math.Sin(x));
}
