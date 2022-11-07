import math

from sympy import Symbol, integrate, solve


def function(x):
    return math.cos(x) * math.sqrt(x)


def integrate_my_func(my_function, from_value, to_value):
    x = Symbol('x')
    return integrate(my_function, (x, from_value, to_value)).evalf()


def simpsons(from_value, to_value):
    return (to_value - from_value) / 6 * (function(from_value) + function(to_value)
                                          + (4 * function((to_value + from_value) / 2)))


def solve_system():
    x = Symbol('x')
    y = Symbol('y')
    z = Symbol('z')
    return solve([x + y + z - 2 / 5, y / 2 + z - 2 / 7, y / 4 + z - 2 / 9])


def interpolation_formula():
    solution = solve_system()
    x = Symbol('x')
    y = Symbol('y')
    z = Symbol('z')
    ashes = (solution[x], solution[y], solution[z])
    points = (0, 1 / 2, 1)
    res = 0
    for i in range(3):
        res += ashes[i] * function(points[i])
    return res


if __name__ == '__main__':
    print("Лабораторная работа 4.1\nВариант 1\nПриближённое вычисление интеграла по составным квадратурным формулам\n")
    function_string = "cos(x)*sqrt(x)"
    lower = 0
    upper = 1
    print(f"Интегрируемая функция: {function_string}\n")

    precise_val = integrate_my_func(function_string, lower, upper)
    print(f"'Точное' значение интеграла: {precise_val}\n")

    simpsons_val = simpsons(lower, upper)
    print(f"Значение интеграла по формуле Симпсона с тремя узлами: {simpsons_val}\n");
    print(f"Абсолютная фактическая погрешность: {abs(simpsons_val - precise_val)}\n")

    print(f"Значение интеграла по интерполяционной формуле : {interpolation_formula()}\n")
