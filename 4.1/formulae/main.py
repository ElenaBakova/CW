import math

from sympy import Symbol, integrate, solve

x = Symbol('x')
y = Symbol('y')
z = Symbol('z')


def function(param):
    return math.cos(param) * math.sqrt(param)


def integrate_my_func(my_function, from_value, to_value):
    return integrate(my_function, (x, from_value, to_value)).evalf()


def simpsons(from_value, to_value):
    return (to_value - from_value) / 6 * (function(from_value) + function(to_value)
                                          + (4 * function((to_value + from_value) / 2)))


def solve_system():
    return solve([x + y + z - 2 / 3, y / 2 + z - 2 / 5, y / 4 + z - 2 / 7])


def check_formula():
    solution = solve_system()
    ashes = (solution[x], solution[y], solution[z])

    def poly(a): return 0.125 * a * a + 4 * a - 3

    points = (0, 1 / 2, 1)
    res = 0
    for i in range(3):
        res += ashes[i] * poly(points[i])
    print(f"Integral for polynom: {res}")


def interpolation_formula():
    solution = solve_system()
    ashes = (solution[x], solution[y], solution[z])
    points = (0, 1 / 2, 1)

    def f(a): return math.cos(a)

    res = 0
    for i in range(3):
        res += ashes[i] * f(points[i])
    return res


if __name__ == '__main__':
    print(solve_system())
    print("Лабораторная работа 4.1\nВариант 1\nПриближённое вычисление интеграла по составным квадратурным формулам\n")
    function_string = "cos(x)*sqrt(x)"
    lower = 0
    upper = 1
    print(f"Интегрируемая функция: {function_string}\n")

    precise_val = integrate_my_func(function_string, lower, upper)
    print(f"1) 'Точное' значение интеграла: {precise_val}\n")

    simpsons_val = simpsons(lower, upper)
    print(f"2) Значение интеграла по формуле Симпсона: {simpsons_val}")
    print(f"Фактическая погрешность: {abs(simpsons_val - precise_val)}\n")

    formula_val = interpolation_formula()
    print(f"3) Значение интеграла по интерполяционной формуле: {formula_val}")
    print(f"Фактическая погрешность: {abs(formula_val - precise_val)}\n")
    # check_formula()
