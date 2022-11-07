using System.Diagnostics;

namespace _4._1;
public class InitialData
{
    private static readonly Func<double, double> function = x => Math.Cos(x) * Math.Sqrt(x);
    private static readonly string functionString = "cos(x)*sqrt(x)";
    private static readonly (double a, double b) limits = new() { a = 0, b = 1};

    public static string FuncString => functionString;
    public double Antiderivative { get; init; }
    public double Simpsons { get; init; }
    public double InterpolationFormula { get; init; }

    public InitialData()
    {
        var temp = GetAntiderivative();
        Antiderivative = temp.Item1;
        Simpsons = (limits.b - limits.a) / 6 * (function(limits.a) + function(limits.b) + (4 * function((limits.b + limits.a) / 2)));
        
    }

    private void GetInterpolationFormula()
    {

    }

    private static (double, double) GetAntiderivative()
    {
        string scriptPath = Environment.CurrentDirectory + "\\..\\..\\..\\script";
        ProcessStartInfo startInfo = new("python");

        string directory = scriptPath;
        string script = "main.py";

        startInfo.WorkingDirectory = directory;
        startInfo.Arguments = $"{script} {functionString} {limits.a} {limits.b}";
        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = true;
        startInfo.RedirectStandardError = true;
        startInfo.RedirectStandardOutput = true;

        (double, double) result = new();
        using (Process process = Process.Start(startInfo))
        using (StreamReader reader = process.StandardOutput)
        {
            string line = reader.ReadLine();
            result.Item1 = double.Parse(line);
            line = reader.ReadLine();
            result.Item2 = double.Parse(line);
            /*string foo = reader.ReadToEnd();
            result += foo;*/
        }

        return result;
    }
}
