import functools
from math import sin, exp

import matplotlib.pyplot as plt
import numpy as np
import scipy as sp
import scipy.integrate
import scipy.special
import seaborn as sns

import Task1

plt.style.use('default')
sns.color_palette('bright')


# ---------------------Метод Галеркина------------------------
@functools.lru_cache(maxsize=None)
def jacobi(n):
    return lambda x: (1 - x ** 2) * sp.special.eval_jacobi(n, 1, 1, x)


def df(f, ord=1):
    return lambda x0: sp.misc.derivative(f, x0, n=ord, dx=1e-2)


# скалярное произведение
# integrate f(x)*g(x) from a to b
def dot(a, b, f, g):
    integrand = lambda x: f(x) * g(x)
    return sp.integrate.quad(integrand, a, b)[0]


# L = p(x) * d^2/dx^2(w) + q(x) * d/dx(w) + r(x) * w(x)
def galerkin(a, b, p, q, r, f, N):
    L = lambda w: lambda x: p(x) * df(w, 2)(x) + q(x) * df(w)(x) + r(x) * w(x)
    L = np.vectorize(L)
    # набор базисных функций -- многочленов якоби, w
    w = [jacobi(i) for i in range(N)]
    Lw = L(w)
    lhs = np.zeros((N, N))
    rhs = np.zeros((N, 1))

    for i in range(N):
        for j in range(N):
            lhs[i, j] = dot(a, b, Lw[j], w[i])
        rhs[i] = dot(a, b, f, w[i])

    # Решаем систему sum((Lw_j, w_i), c_j) = (f, w_i)
    c = np.linalg.solve(lhs, rhs)
    return lambda x: sum(c[i] * w[i](x) for i in range(N))


def plot(a, b, p, q, r, f, N, start_from=2, expected=None):
    fix, ax = plt.subplots(1, 2, figsize=(20, 10), dpi=80)
    g = np.linspace(a, b, 100)
    solutions_ax = ax[0]
    solutions_ax.title.set_text('Сравнение решения при разных N')
    solutions_ax.set_xlabel('x', fontsize=20)
    solutions_ax.set_ylabel('y', fontsize=20)
    for n in range(start_from, N + 1):
        actual = galerkin(a, b, p, q, r, f, n)
        solutions_ax.plot(g, actual(g), label=f'N={n}')
    solutions_ax.legend(prop={'size': 13})

    expected_ax = ax[1]
    if expected is not None:
        gr = np.linspace(a, b, intervals[-1] + 1)
        expected_ax.plot(gr, expected, label=f'Точное решение')
    expected_ax.title.set_text(f'Сравнение точного решения и решения найденного при N={N}')
    expected_ax.set_xlabel('x', fontsize=20)
    expected_ax.set_ylabel('y', fontsize=20)
    expected_ax.plot(g, actual(g), label=f'Решение для N={N}')
    expected_ax.legend(prop={'size': 13})
    plt.show()


if __name__ == "__main__":
    # -1/(x - 3)u" + (1 + x/2)u' + exp(x/2)u = 2 - x, u(-1) = u(1) = 0
    p = lambda x: -1 / (x - 3)
    q = lambda x: (1 + x / 2)
    r = lambda x: exp(x / 2)
    f = lambda x: 2 - x

    plot(-1, 1, p, q, r, f, 5)

    # (1 + sin(x))u" + e^(-x)u' = (e^(-x) * sin(2 * pi * x)), u(-1)=u(1)=0
    p = lambda x: 1
    q = lambda x: exp(-x) / (1 + sin(x))
    r = lambda x: 0
    f = lambda x: exp(-x) * sin(2 * np.pi * x) / (1 + sin(x))
    expected, intervals, err = Task1.solve(-1, 1, q, r, f, 0, 0)

    plot(-1, 1, p, q, r, f, 5, expected=expected)
