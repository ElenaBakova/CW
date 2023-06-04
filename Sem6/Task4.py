import math

import matplotlib.pyplot as plt
import matplotlib.ticker as mtick
import numpy as np
from scipy.linalg import hilbert as hlbrt


# Числа обусловленности
# Спектральный критерий: |A|*|A|^(-1)
# cond_s > 10^4 считают плохим
def cond_s(A):
    return np.linalg.norm(A) * np.linalg.norm(np.linalg.inv(A))


# Объемный критерий (критерий Ортеги)
def cond_v(A):
    N = A.shape[0]
    det = abs(np.linalg.det(A))
    return np.prod([math.sqrt(sum([A[n, m] ** 2 for m in range(N)])) for n in range(N)]) / det


# Угловой критерий: max_{n}(|a_n|*|c_n|), c = A^(-1)
def cond_a(A):
    N = A.shape[0]
    C = np.linalg.inv(A)
    return max([np.linalg.norm(A[n, :]) * np.linalg.norm(C[:, n]) for n in range(N)])


def lu_decomposition(A):
    N = A.shape[0]
    U = np.zeros((N, N))
    L = np.eye(N)

    for i in range(N):
        for j in range(N):
            m = min(i, j)
            s = sum([L[i, k] * U[k, j] for k in range(m)])
            if i <= j:
                U[i, j] = A[i, j] - s
            else:
                L[i, j] = (A[i, j] - s) / U[j, j]

    return L, U


# to solve Ly = b
def forward_sub(L, b):
    n = L.shape[0]
    y = np.zeros(n)
    for i in range(n):
        tmp = b[i]
        for j in range(i):
            tmp -= L[i, j] * y[j]
        y[i] = tmp / L[i, i]
    return y


# to solve Ux = b
def back_sub(U, b):
    n = U.shape[0]
    x = np.zeros(n)
    for i in range(n - 1, -1, -1):
        tmp = b[i]
        for j in range(i + 1, n):
            tmp -= U[i, j] * x[j]
        x[i] = tmp / U[i, i]
    return x


def solve_with_lu(A, b):
    L, U = lu_decomposition(A)
    y = forward_sub(L, b)
    x = back_sub(U, y)
    return x


def plot(hilbert=False):
    n_values = list(range(2, 13))
    errors = []
    for n in n_values:
        A = np.random.rand(n, n)
        b = np.random.rand(n)
        if (hilbert):
            A = hlbrt(n)
            x = np.ones((n, 1))
            b = A @ x
        x_exact = np.linalg.solve(A, b)
        x_approx = solve_with_lu(A, b)
        error = np.linalg.norm(x_exact - x_approx)
        errors.append(error)

    _, ax = plt.subplots()
    plt.plot(n_values, errors)
    plt.xticks()
    plt.yscale("log")
    ax.yaxis.set_major_formatter(mtick.FormatStrFormatter('%.2e'));
    plt.xlabel('Размер матрицы')
    plt.ylabel('Ошибка')
    if (hilbert):
        plt.title('Погрешность решения СЛАУ для матриц Гильберта при различных N')
    else:
        plt.title('Погрешность решения СЛАУ при различных N')
    plt.show()


def print_conds(matrix, matrix_name):
    a = cond_a(matrix)
    v = cond_v(matrix)
    s = cond_s(matrix)

    print(f'''
    Числа обусловленности матрицы {matrix_name}:
        Угловой критерий: {a}
        Объемный критерий: {v}
        Спектральный критерий: {s}
    ''')

    return a, v, s


if __name__ == "__main__":
    # Вариант 1
    A = np.array([[3.278164, 1.046583, -1.378574], [1.046583, 2.975937, 0.934251], [-1.378574, 0.934251, 4.836173]])
    b = np.array([-0.527466, 2.526877, 5.165441])
    print_conds(A, 'A')

    L, U = lu_decomposition(A)

    print_conds(L, 'L')
    print_conds(U, 'U')

    solution = solve_with_lu(A, b)

    print(f'Решение: {solution}')

    plot()
    plot(hilbert=True)
