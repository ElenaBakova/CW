import matplotlib.pyplot as plt
import numpy as np
import numpy.testing
import pandas as pd
import seaborn as sns
from scipy.linalg import hilbert


def error(x1, x2):
    return np.linalg.norm(x1 - x2)


def posteriori_error(x1, x2, B):
    B_norm = np.linalg.norm(B, ord=2)
    return B_norm / (1 - B_norm) * error(x1, x2)


def transform(A, b):
    N = A.shape[0]
    alpha = np.zeros((N, N))
    beta = np.zeros((N, 1))
    for i in range(N):
        beta[i] = b[i] / A[i, i]
        for j in range(N):
            alpha[i, j] = 0 if i == j else -A[i, j] / A[i, i]

    return alpha, beta


def transform_for_positive(A, b):
    N = A.shape[0]
    m = min(A[k, k] - sum(abs(A[k, j]) if k != j else 0 for j in range(N)) for k in range(N))
    m = max(m, 0)
    M = max(A[k, k] + sum(abs(A[k, j]) if k != j else 0 for j in range(N)) for k in range(N))
    alpha = 2 / (m + M)
    B = np.identity(N) - alpha * A
    C = alpha * b
    return B, C


# Метод простых итераций
def iterative(A, b, eps, get_iterations=False, positive_def=False, max_iter=100000):
    N = A.shape[0]
    # Для самосопряженных положительно определенных матриц
    if positive_def:
        alpha, beta = transform_for_positive(A, b)
    else:
        alpha, beta = transform(A, b)
    # alpha, beta = transform(A, b)

    # Проверяем условие сходимости (спектральный радиус < 1)
    p = max(abs(np.linalg.eigvals(alpha)))
    if p >= 1:
        raise ValueError(f'p(B) == {p} >= 1, метод простой итерации не сходится')

    x_prev = beta
    iter = 0
    while iter < max_iter:
        iter += 1
        x_next = alpha @ x_prev + beta
        if posteriori_error(x_next, x_prev, alpha) < eps:
            break
        x_prev = x_next

    if get_iterations:
        return x_next, iter
    else:
        return x_next


# Метод Зейделя
def seidel(A, b, eps, get_iterations=False, max_iter=100000):
    N = A.shape[0]
    alpha, beta = transform(A, b)

    k = 0
    x_prev = np.zeros((N, 1))
    while k < max_iter:
        k += 1
        x_next = np.copy(x_prev)
        for i in range(N):
            x_next[i] = sum(alpha[i, j] * x_next[j] for j in range(i)) \
                        + sum(alpha[i, j] * x_prev[j] for j in range(i + 1, N)) + beta[i]
        if np.linalg.norm(x_prev - x_next) < eps / (10 ** N):
            break
        x_prev = x_next

    if get_iterations:
        return x_next, k
    else:
        return x_next


# -------------------------------------------------------------

plt.style.use('default')
sns.color_palette('bright')


def plot(data, title, xticks):
    df = pd.DataFrame(data, index=xticks)

    plt.figure(figsize=(15, 10), dpi=80)

    for key in df.keys():
        plt.plot(df[key], label=key, marker='o')

    plt.title(title)
    plt.legend(title='Размерность матрицы')
    plt.xlabel('Eps', fontsize=20)
    plt.ylabel('Количество итераций', fontsize=20)
    plt.xscale('log')
    plt.show()


def gen_diagonally_dominant(size):
    A = numpy.random.rand(size, size)
    for i in range(size):
        A[i, i] = sum([abs(A[i, j]) for j in range(size)]) + 1
    return A


data = {}
for size in range(2, 12):
    key = f'{size}x{size}'
    data[key] = []
    A = gen_diagonally_dominant(size)
    x = np.ones((size, 1))
    b = A @ x

    for eps in range(1, 13):
        actual, k = iterative(A, b, 10 ** -eps, True)
        data[key].append(k)

plot(data, 'Метод простой итерации на матрицах с диагональным преобладанием', [10 ** -p for p in range(1, 13)])

data = {}
for size in range(2, 5):
    key = f'{size}x{size}'
    data[key] = []
    A = hilbert(size)
    x = np.ones((size, 1))
    b = A @ x

    for eps in range(1, 8):
        actual, k = iterative(A, b, 10 ** -eps, get_iterations=True, positive_def=True)
        data[key].append(k)

plot(data, 'Метод простой итерации на матрицах Гильберта', [10 ** -p for p in range(1, 8)])

data = {}
for size in range(2, 12):
    key = f'{size}x{size}'
    data[key] = []
    A = gen_diagonally_dominant(size)
    x = np.ones((size, 1))
    b = A @ x

    for eps in range(1, 13):
        actual, k = seidel(A, b, 10 ** -eps, True)
        data[key].append(k)

plot(data, 'Метод Зейделя на матрицах с диагональным преобладанием', [10 ** -p for p in range(1, 13)])
