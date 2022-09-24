using Interpolation;

Console.WriteLine("Problem of algebraic interpolation\nVariant 1");
Console.WriteLine("f(x) = sin(x) - x^2 / 2; m = 15; n = 7; a = 0; b = 1; x = 0.65\n");

while (true)
{
    Interface.Run();
    Console.WriteLine("\nWould you like to start over?\nY - start over\nN - exit");
    while (true)
    {
        var read = Console.ReadLine();
        if (read == "Y")
        {
            break;
        }
        if (read == "N")
        {
            return;
        }
    }
}