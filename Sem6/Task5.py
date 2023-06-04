import math

import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from scipy.linalg import hilbert as hilbert


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


def error(x, y):
    return np.linalg.norm(x - y)


def plot(A_data, title, xticks):
    A_df = pd.DataFrame(A_data, index=xticks)

    plt.figure(figsize=(15, 10), dpi=80)
    for key in A_df.keys():
        plt.plot(A_df[key], label='A, ' + key)

    plt.title(title)
    plt.legend(title='Критерий')
    plt.xlabel('Alpha', fontsize=20)
    plt.ylabel('Порядок числа обусловленности', fontsize=20)
    plt.yscale('log')
    plt.xscale('log')
    plt.show()


def regularize(A, print_conds=False, matrix_name='A', build_plot=False):
    N = A.shape[0]
    x = np.ones((N, 1))
    b = A @ x
    E = np.eye(N)

    min_error = -1
    l_power = -12
    r_power = 0
    for power in range(l_power, r_power):
        alpha = 10 ** power
        A_r = A + alpha * E
        x_actual = np.linalg.solve(A_r, b)
        e = error(x, x_actual)
        if min_error == -1 or min_error > e:
            min_error = e
            min_alpha = alpha
        print(f'Отклонение: {e} для альфа = {alpha}')
    print(f'Наименьшая ошибка для альфа: {min_alpha}\n')

    if build_plot:
        a_key = 'Угловой критерий'
        v_key = 'Объемный критерий'
        s_key = 'Спектральный критерий'
        alpha_str = 'Alpha'
        A_data = {a_key: [], v_key: [], s_key: []}
        for power in range(l_power, r_power):
            alpha = 10 ** power
            A_r = A + alpha * E

            A_data[a_key].append(cond_a(A_r))
            A_data[v_key].append(cond_v(A_r))
            A_data[s_key].append(cond_s(A_r))

        plot(A_data, 'Зависимость чисел обусловленности от альфа', [10 ** p for p in range(l_power, r_power)])


regularize(hilbert(2), build_plot=True)
regularize(hilbert(17), build_plot=True)

temp = np.array([[3.278164, 1.046583, -1.378574], [1.046583, 2.975937, 0.934251], [-1.378574, 0.934251, 4.836173]])
regularize(temp, build_plot=True)
