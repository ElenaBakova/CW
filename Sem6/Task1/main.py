import math

import sympy as smp
import numpy as np

a, b = -1.0, 1.0
E = 0.0000000001
N = 1
x = smp.Symbol('x', real=True)


# создает сетку от a до b с шагом (b - a) / N
def grid(a, b, N):
    return np.linspace(a, b, N)


def q(param: float):
    return (param + 2) * param / (param - 2)


def r(param: float):
    return (1 - math.sin(param)) * (param + 2) / (param - 2)


def f(param: float):
    return (param + 2) * param * param / (param - 2)


if __name__ == '__main__':
    print('Численное решение краевой задачи')
    print('Вариант 6')

    print(f'[a; b] = [{a}; {b}]')
    print(f'u({a}) = 0, u({b}) = 0')
    print('Введите эпсилон')
    E = float(input())
    print(f'(x-2)u''/(x+2) + xu\' + (1 - sin(x))u = x^2')

    print('Введите размерность сетки')
    N = int(input())

    h = (b - a) / N
    y = []
    u = []
    for i in range(N):
        y.append(a + i * h)
        u.append(smp.Symbol(f'u{i}'))

    matrix = []
    for i in range(0, N):
        matrix.append([0 for j in range(N)])
        matrix[i][i] = - 2.0 / (h ** 2) - r(y[i])  # u_n
        if i != N - 1:
            matrix[i][i + 1] = 1.0 / (h ** 2) + q(y[i]) / (2.0 * h)  # u_n+1
        if i != 0:
            matrix[i][i - 1] = 1.0 / (h ** 2) - q(y[i]) / (2.0 * h)  # u_n-1

    # print(matrix)

    matrix = np.array(matrix, dtype='float')
    right_side = np.array([f(y[j]) for j in range(N)], dtype='float')

    # print(right_side)

    result = np.linalg.solve(matrix, right_side)

    print(result)
