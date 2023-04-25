import math

import numpy as np
import matplotlib.pyplot as plt


# Set up the grid
def grid(a, b, N):
    return np.linspace(a, b, N)


# Solves a boundary value problem of the form
#     -p(x)y'' + q(x)y' + r(x)y = f(x), a <= x <= b,
#     y(a) = alpha, y(b) = beta, using a grid-based method.
#
#     Parameters
#     ----------
#     p : function
#          defining p(x) in the differential equation.
#     q : function
#          defining q(x) in the differential equation.
#     r : function
#          defining r(x) in the differential equation.
#     f : function
#          defining f(x) in the differential equation.
#     a : float
#         Left endpoint of the interval of interest.
#     b : float
#         Right endpoint of the interval of interest.
#     alpha : float
#         Boundary condition at the left endpoint.
#     beta : float
#         Boundary condition at the right endpoint.
#     N : int, optional
#         Number of intervals in the coarse grid, by default 10.
#
#     Returns
#     -------
#     x : array
#         Grid points.
#     y : array
#         Approximate solution to the boundary value problem.
def solve_bvp(a, b, q, r, f, N, alpha, beta):
    # Interval length
    h = (b - a) / N

    # Create grid points
    g = grid(a, b, N)

    rhs = np.zeros((N + 1, 1))
    A = np.zeros((N + 1, N + 1))

    # Boundary conditions
    A[0, 0] = 1
    A[N, N] = 1
    rhs[0] = alpha
    rhs[N] = beta

    for i in range(1, N):
        x_i = g[i]
        A[i, i + 1] = 1.0 / h ** 2.0 + q(x_i) / (2.0 * h)  # u_n+1
        A[i, i] = -2.0 / h ** 2.0 - r(x_i)  # u_n
        A[i, i - 1] = 1.0 / h ** 2.0 - q(x_i) / (2.0 * h)  # u_n-1
        rhs[i] = f(x_i)

    return np.linalg.solve(A, rhs)


# r -- коэффициент сгущения сетки (здесь 2),
# p — теоретический порядок точности численного метода.
def count_delta(v1, v2, r=2, p=1):
    delta = np.zeros((v2.size, 1))

    # For odd nodes: delta(x_{2n+1}) = (delta(x_{2n}) + delta(x_{2n+2})) / 2
    for i in range(0, v2.size):
        if (i % 2) == 0:
            delta[i] = (v2[i] - v1[i // 2]) / (r ** p - 1)
        else:
            delta[i] = (delta[i - 1] + delta[i + 1]) / 2.0

    return delta


def error_norm(v1, v2, r=2, p=1):
    return np.linalg.norm(count_delta(v1, v2, r, p), ord=2) / math.sqrt(v2.size)


def solve(a, b, q, r, f, alpha, beta, eps=1e-3, max_iterations=100):
    n = 10

    v1 = solve_bvp(a, b, q, r, f, n, alpha, beta)
    v2 = solve_bvp(a, b, q, r, f, 2 * n, alpha, beta)
    intervals = [2 * n]
    errors = [error_norm(v1, v2)]
    i = 1
    while i < max_iterations:
        # Check for convergence
        if error_norm(v1, v2) < eps:
            break
        if i == max_iterations:
            break
        v1 = v2
        # Count new value
        v2 = solve_bvp(a, b, q, r, f, 2 * n, alpha, beta)
        intervals.append(2 * n)
        errors.append(error_norm(v1, v2))
        n *= 2
        i += 1

    # Уточняем решение
    v2 = v2 + count_delta(v1, v2)
    return v2, intervals, errors


def plot(a, b, current, intervals, errors, expected=None):
    # fig, ax = plt.subplots(1, 2, figsize=(30, 10), dpi=80)
    fig, ax = plt.subplots(1, 1)
    N = intervals[-1] + 1
    solution_ax = ax
    # error_ax = ax[1]
    g = grid(a, b, N)
    solution_ax.set_xlabel('x', fontsize=20)
    solution_ax.set_ylabel('y', fontsize=20)
    solution_ax.plot(g, current, label='Найденное решение')

    # if expected:
    #     solution_ax.plot(g, expected(g), label='Точное решение')
    # solution_ax.legend(prop={'size': 20})
    #
    # error_ax.title.set_text('Зависимость ошибки от количества узлов сетки')
    # error_ax.set_yscale('log')
    # error_ax.set_xscale('log', base=2)
    # error_ax.set_xlabel('Количество узлов', fontsize=20)
    # error_ax.set_ylabel('Оценка точности ответа', fontsize=20)
    # error_ax.plot(intervals, errors)
    plt.show()


''''''


# Вариант 6, (x − 2)/(x + 2)u′′+xu′+ (1−sin(x))u=x^2, u(−1) = u(1) = 0.
def q(param): return param / (param - 2) * (param + 2)


def r(param): return (1 - math.sin(param)) / (param - 2) * (param + 2)


def f(param): return param ** 2 / (param - 2) * (param + 2)


a, b = -1, 1
alpha, beta = 0, 0
x, intervals, errors = solve(a, b, q, r, f, alpha, beta, 1e-6, 10)
plot(a, b, x, intervals, errors)
