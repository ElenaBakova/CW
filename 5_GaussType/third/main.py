import numpy as np
import secant

from scipy import integrate


def legendre(x, n):
    if n == 0:
        return 1
    if n == 1:
        return x
    p0 = 1
    p1 = x
    for i in range(2, n + 1):
        p2 = ((2 * i - 1) / i) * p1 * x - ((i - 1) / i) * p0
        p0 = p1
        p1 = p2
    return p1


def function(x):
    return np.sin(x) * x ** (1 / 2)


def find_coef(n, X):
    A = []
    for ind in range(1, n + 1):
        p = legendre(X[ind - 1], n - 1)
        a = (2 * (1 - X[ind - 1] ** 2)) / (n ** 2 * p ** 2)
        A.append(a)
    return A


if __name__ == "__main__":
    print("Лабораторная работа 5.3\nВариант 1\nПриближённое вычисление интеграла при помощи составной КФ Гаусса\n")
    print("Интегрируемая функция: sin(x)*sqrt(x); Вес: sqrt(x)\n")

    user_input = input("Хотите продолжить? 'y' -- да, 'n' -- нет")
    while user_input != 'n':
        print('Введите промежуток интегрирования')
        a, b = [float(i) for i in input().split()]
        print("Введите количество узлов")
        n = int(input())
        print("Введите число промежутков деления отрезка")
        m = int(input())

        # находим узлы Лежандра методом секущих (t_k) n штук
        nodes = secant.count(-1, 1, legendre, n)

        # находим коэффициенты A_k
        C = find_coef(n, nodes)

        # X = gauss(-1, 1, n)
        # A = find_coef(n, X)
        # for j in range(n):
        #     print('узел {} = {},  коэффициент {} = {}'.format(j + 1, X[j], j + 1, A[j]))
        # print()
        result = 0
        h = (b - a) / m
        z = a
        for j in range(m):
            print('j = {}: [{} : {}]'.format(j, z, z + h))
            for k in range(1, n + 1):
                node = h / 2 * nodes[k - 1] + (2 * z + h) / 2
                coef = h / 2 * C[k - 1]
                result += coef * function(node)
                print('{} узел = {}, {} коэффициент = {}'.format(k, node, k, coef))
            z += h
            print()

        print()
        print("Значение интеграла:", result)
        precise_val = integrate.quad(function, a, b)
        print("Точное значение интеграла:", precise_val[0])
        print("Абсолютная погрешность:", abs(result - precise_val[0]))
        print()
        user_input = input("Хотите продолжить? 'y' -- да, 'n' -- нет")
