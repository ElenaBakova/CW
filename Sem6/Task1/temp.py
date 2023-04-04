import math

import numpy as np
import matplotlib.pyplot as plt


# set up the grid
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
def solve_bvp(a, b, p, q, r, f, N, alpha, beta):
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

    for n in range(1, N):
        x = g[n]
        A[n, n + 1] = 1.0 / h ** 2.0 + q(x) / (2.0 * h)  # u_n+1
        A[n, n] = -2.0 / h ** 2.0 - r(x)  # u_n
        A[n, n - 1] = 1.0 / h ** 2.0 - q(x) / (2.0 * h)  # u_n-1
        rhs[n] = f(x)

    return np.linalg.solve(A, rhs)


# r -- коэффициент сгущения сетки (здесь 2),
# p — теоретический порядок точности численного метода.
def error(v1, v2, r=2, p=1):
    delta = np.zeros((v2.size, 1))

    # For odd nodes: delta(x_{2n+1}) = (delta(x_{2n}) + delta(x_{2n+2})) / 2
    for i in range(0, v2.size):
        if (i % 2) == 0:
            delta[i] = (v2[i] - v1[i // 2]) / (r ** p - 1)
        else:
            delta[i] = (delta[i - 1] + delta[i + 1]) / 2

    return delta


def error_norm(v1, v2, r=2, p=1):
    return np.linalg.norm(error(v1, v2, r, p), ord=2) / math.sqrt(v2.size)


def solve(a, b, p, q, r, f, alpha, beta, eps=1e-3, max_iterations=100):
    n = 10

    v1 = solve_bvp(a, b, p, q, r, f, n, alpha, beta)
    v2 = solve_bvp(a, b, p, q, r, f, 2 * n, alpha, beta)
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
        v2 = solve_bvp(a, b, p, q, r, f, 2 * n, alpha, beta)
        intervals.append(2 * n)
        errors.append(error_norm(v1, v2))
        n *= 2
        i += 1

    # Уточняем решение
    v2 = v2 + error(v1, v2)
    return v2, intervals, errors


def plot(a, b, actual, intervals, errors, expected=None):
    fig, ax = plt.subplots(1, 2, figsize=(30, 10), dpi=80)
    N = intervals[-1] + 1
    solution_ax = ax[0]
    error_ax = ax[1]
    g = grid(a, b, N)
    solution_ax.title.set_text('Решение диффура')
    solution_ax.set_xlabel('x', fontsize=20)
    solution_ax.set_ylabel('y', fontsize=20)
    solution_ax.plot(g, actual, label='Найденное решение')

    if expected:
        solution_ax.plot(g, expected(g), label='Точное решение')
    solution_ax.legend(prop={'size': 20})

    error_ax.title.set_text('Зависимость ошибки от количества узлов сетки')
    error_ax.set_yscale('log')
    error_ax.set_xscale('log', basex=2)
    error_ax.set_xlabel('Количество узлов', fontsize=20)
    error_ax.set_ylabel('Оценка точности ответа', fontsize=20)
    error_ax.plot(intervals, errors)
