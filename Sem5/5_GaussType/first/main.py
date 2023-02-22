import math
import secant

from sympy import Symbol, integrate, solve

x = Symbol('x')
y = Symbol('y')
z = Symbol('z')


def function(param):
    return math.cos(param) * math.sqrt(param)


# def function(param):
#     return 1

# def function(param):
#     return param

# def function(param):
#     return param * param

# def function(param):
#     return param * param * param


def integrate_my_func(my_function, from_value, to_value):
    return integrate(my_function, (x, from_value, to_value)).evalf()


def solve_system():
    return solve([x + y + z - 2 / 3, y / 2 + z - 2 / 5, y / 4 + z - 2 / 7])


# def check_formula():
#     solution = solve_system()
#     ashes = (solution[x], solution[y], solution[z])
#
#     def poly(a): return 0.125 * a * a + 4 * a - 3
#
#     poly_string = "0.125 * x * x + 4 * x - 3"
#
#     res = 0
#     for i in range(3):
#         res += ashes[i] * poly(points[i])
#     print(f"Точное значение для полинома: {integrate(poly_string, (x, 0, 1))}")
#     print(f"Интеграл для полинома: {res}")

def interpolation_formula():
    points = (0, 1 / 2, 1)
    solution = solve_system()
    ashes = (solution[x], solution[y], solution[z])

    print(f"Узлы: {points}")
    print("Моменты: (2/3, 2/5, 2/7)")
    print(f"Коэффициенты: {ashes}\n")

    def f(a): return math.cos(a)

    res = 0
    for i in range(3):
        res += ashes[i] * f(points[i])
    return res


def moments():
    return (
        integrate("1", (x, 0, 1)).evalf(),
        integrate("x", (x, 0, 1)).evalf(),
        integrate("x*x", (x, 0, 1)).evalf(),
        integrate("x*x*x", (x, 0, 1)).evalf(),
    )


def highest_precision():
    mu = (2 / 3, 2 / 5, 2 / 7, 2 / 9)
    print(f"Моменты: {mu}")

    a1 = (mu[0] * mu[3] - mu[2] * mu[1]) / (mu[1] * mu[1] - mu[2] * mu[0])
    a2 = (mu[2] * mu[2] - mu[3] * mu[1]) / (mu[1] * mu[1] - mu[2] * mu[0])

    def polynom(a): return a * a + a1 * a + a2

    roots = secant.count(0, 1, polynom)
    print(f"Корни x_k: {roots}")

    coeff1 = 1 / (roots[0] - roots[1]) * (mu[1] - roots[1] * mu[0])
    coeff2 = 1 / (roots[1] - roots[0]) * (mu[1] - roots[0] * mu[0])

    # print(f"A1 + A2 = mu_0: {coeff1 + coeff2 == mu[0]}")

    def f(a): return math.cos(a)
    # def f(a): return a * a * a

    return coeff1 * f(roots[0]) + coeff2 * f(roots[1])


if __name__ == '__main__':
    print("Лабораторная работа 5.1\nВариант 1\nПостроение квадратурной формулы наивысшей алгебраической степени "
          "точности\n")
    function_string = "cos(x)*sqrt(x)"
    lower = 0
    upper = 1
    print(f"Интегрируемая функция: {function_string}")
    print("Весовая функция: sqrt(x)")
    print("Интегрирование от 0 до 1\n")

    precise_val = integrate_my_func(function_string, lower, upper)
    print(f"1) 'Точное' значение интеграла: {precise_val}\n")

    print("3) Интерполяционная формула")
    formula_val = interpolation_formula()
    print(f"Значение интеграла по интерполяционной формуле: {formula_val}")
    print(f"Фактическая погрешность: {abs(formula_val - precise_val)}\n")
    # check_formula()

    print("5) Формула наивысшей алгебраической степени точности")
    kfnast = highest_precision()
    print(f"\nЗначение интеграла по формуле типа Гаусса: {kfnast}")
    print(f"Фактическая погрешность: {abs(kfnast - precise_val)}\n")
