import math
import secant
import sympy
import scipy
import numpy as np

from sympy import Symbol, integrate

x = Symbol('x')
y = Symbol('y')
z = Symbol('z')

function_string = "sin(x)/x"
lower = 0
upper = 2


def function(param): return math.sin(param) / param


def integrate_my_func(my_function, from_value, to_value):
    return sympy.integrate(my_function, (x, from_value, to_value)).evalf()


def polynoms(param, k):
    if k == 3:
        return param ** 5 + 8 * param ** 4 + 2 * param
    if k == 4:
        return 8 * param ** 7 + 9 * param ** 5 + 4 * param ** 3
    if k == 5:
        return 2 * param ** 9 + param ** 8 + 6 * param ** 7


def poly_string(k):
    if k == 3:
        return "x^5 + 8*x^4 + 2*x"
    if k == 4:
        return "8*x^7 + 9*x^5 + 4*x^3"
    if k == 5:
        return "2*x^9 + x^8 + 6*x^7"


def answers(k):
    return integrate(poly_string(k), (x, -1, 1)).evalf()


def legendre(param, n):
    if n == 0:
        return 1
    if n == 1:
        return param
    return (2 * n - 1) / n * legendre(param, n - 1) * param - (n - 1) / n * legendre(param, n - 2)


def find_coef(n, X):
    A = []
    for ind in range(1, n + 1):
        p = legendre(X[ind - 1], n - 1)
        a = (2 * (1 - X[ind - 1] ** 2)) / (n ** 2 * p ** 2)
        A.append(a)
    return A


if __name__ == '__main__':
    print("Лабораторная работа 5.2\nВариант 1\nПостроение КФ Гаусса, КФ Мелера\n")

    print("1. Узлы и коэффициенты КФ Гаусса при N = 1,...,8")
    for i in range(1, 9):
        # находим узлы Лежандра методом секущих (t_k) i штук
        nodes = secant.count(-1, 1, legendre, i)

        # находим коэффициенты A_k
        C = find_coef(i, nodes)
        print()
        print(f'N = {i}')
        for j in range(i):
            print(f'Узел = {nodes[j]} ---  Коэффициент = {C[j]}')

    print()
    print("2. Проверка точности на многочленах наивысшей степени для N = 3, 4, 5")
    print()
    for i in [3, 4, 5]:
        # находим узлы Лежандра методом секущих (t_k) i штук
        nodes = secant.count(-1, 1, legendre, i)

        # находим коэффициенты A_k
        C = find_coef(i, nodes)

        ans = 0
        for j in range(i):
            ans += polynoms(nodes[j], i) * C[j]
        precise_ans = answers(i)

        print(f'N = {i}')
        # print(polynoms(nodes[j], i))
        print("Значение:", ans)
        print("\"Точное\" значение:", precise_ans)
        print("Абсолютная погрешность:", abs(ans - precise_ans))
        print()

    print()
    print("3. Вычисление интеграла из задания")
    print(f"Интегрируемая функция: {function_string}")
    user_input = input("Хотите продолжить? 'y' -- да, 'n' -- нет")
    while user_input != 'n':
        print('Введите a и b (промежуток интегрирвоания)')
        a = float(input('a: '))
        b = float(input('b: '))
        print()
        for i in [2, 5, 6, 8]:
            # находим узлы Лежандра методом секущих (t_k) i штук
            nodes = secant.count(-1, 1, legendre, i)

            # находим коэффициенты A_k
            C = find_coef(i, nodes)

            ans = 0.0
            print(f'N = {i}')
            for j in range(i):
                ans += function(((b - a) / 2.0) * nodes[j] + ((b + a) / 2.0)) * C[j]
                print(f'Узел = {((b - a) / 2.0) * nodes[j] + ((b + a) / 2.0)},  Коэффициент = {C[j] * ((b - a) / 2.0)}')
            ans *= ((b - a) / 2)
            precise_ans = integrate_my_func(function_string, a, b)

            print()
            print("Значение:", ans)
            print("\"Точное\" значение:", precise_ans)
            print("Абсолютная погрешность:", abs(ans - precise_ans))
            print("Относительная погрешность:", abs((ans - precise_ans) / precise_ans) * 100)
            print()
        user_input = input("Хотите продолжить? 'y' -- да, 'n' -- нет")


    def function1(param):
        return math.cos(param)


    def function2(param):
        return np.cos(param) * (1 / (1 - param ** 2) ** (1 / 2))


    print()
    print("5. Приближенное вычисление интеграла с помощью КФ Меллера")
    user_input = input("Хотите продолжить? 'y' -- да, 'n' -- нет")
    while user_input != 'n':
        print('Введите значения N')
        n1 = int(input("N1: "))
        n2 = int(input("N2: "))
        n3 = int(input("N3: "))
        print()

        for i in [n1, n2, n3]:
            print(f"N = {i}")
            ans = 0
            for j in range(1, i + 1):
                x = math.cos(((2 * j - 1) / (2 * i)) * math.pi)
                ans += function1(x)
                print(f'Узел = {x} --- Коэффициент = {math.pi / i}')
            ans *= math.pi / i

            precise_ans = scipy.integrate.quad(function2, -1, 1)[0]
            print("Значение:", ans)
            print("\"Точное\" значение:", precise_ans)
            print("Абсолютная погрешность:", abs(ans - precise_ans))
            print("Относительная погрешность:", abs((ans - precise_ans) / precise_ans) * 100)
            print()
        user_input = input("Хотите продолжить? 'y' -- да, 'n' -- нет")
