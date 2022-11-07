using _4._1;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("Лабораторная работа 4.1\nВариант 1\nПриближённое вычисление интеграла по составным квадратурным формулам\n\n");

var data = new InitialData();

Console.WriteLine($"Интегрируемая функция: {InitialData.FuncString}\n");
Console.WriteLine($"'Точное' значение интеграла: {data.Antiderivative}\n\n");

Console.WriteLine($"Значение интеграла по формуле Симпсона с тремя узлами: {data.Simpsons}\n");
Console.WriteLine($"Абсолютная фактическая погрешность: {Math.Abs(data.Simpsons - data.Antiderivative)}\n");