import math

import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from scipy.linalg import hilbert as hilbert


# vary epsilon from -2 to -5 (power of 10)

def power_iteration_method(A: np.array, epsilon=10 ** (-3)):
    n = A.shape[0]
    x_k_1 = np.ones(n)
    x_k = None
    previous_value = None
    current_value = None
    iteration_steps = 0
    while previous_value is None or x_k is None or (current_value - previous_value) >= epsilon:
        if current_value is not None:
            previous_value = math.sqrt((x_k_1 @ x_k_1) / (x_k @ x_k))
        x_k = x_k_1
        x_k_1 = A @ x_k_1
        current_value = math.sqrt((x_k_1 @ x_k_1) / (x_k @ x_k))
        iteration_steps += 1
    return (x_k_1[0] / x_k[0]), iteration_steps


def scalar_product_method(A: np.array, epsilon=10 ** (-3)):
    n = A.shape[0]

    x = np.ones(n)
    previous_x = None

    y = np.ones(n)
    previous_y = None
    previous_value = None
    current_value = None

    iteration_steps = 0
    while previous_value is None or previous_x is None or (current_value - previous_value) >= epsilon:
        if current_value is not None:
            previous_value = (x @ y) / (previous_x @ y)
        previous_x = x
        x = A @ x
        current_value = (x @ y) / (previous_x @ y)

        previous_y = y
        y = A.T @ y

        iteration_steps += 1
    return (x[0] / previous_x[0]), iteration_steps


def check(A: np.array):
    eigs, vectors = np.linalg.eig(A)
    expected = max(eigs, key=abs)
    print(f"Ожидаемый максимум: {expected}\n")

    eps = 10 ** (-3)

    actual, iteration_steps = power_iteration_method(A, eps)
    print(f"Максимум, найденный степенным методом: {actual}. \nКол-во итераций: {iteration_steps}")
    discrepancy = abs(expected - actual)
    print(f"Погрешность : {discrepancy}\n")
    print()
    actual, iteration_steps = scalar_product_method(A, eps)
    print(f"Максимум, найденный методом скалярных произведений: {actual}. \nКол-во итераций: {iteration_steps}")
    discrepancy = abs(expected - actual)
    print(f"Погрешность: {discrepancy}")


def plot(data, title, xticks):
    df = pd.DataFrame(data, index=xticks)

    plt.figure(figsize=(15, 10), dpi=80)

    for key in df.keys():
        plt.plot(df[key], label=key, marker='o')

    plt.title(title)
    plt.legend(title='Размерность матрицы')
    plt.xlabel('Епсилон', fontsize=20)
    plt.ylabel('Количество итераций', fontsize=20)
    plt.xscale('log')
    plt.show()


def get_data(method):
    data = {}
    for size in range(2, 12):
        key = f'{size}x{size}'
        data[key] = []
        A = np.random.rand(size, size)

        for eps in np.logspace(-2, -6, num=5):
            eig, k = method(A, eps)
            data[key].append(k)
    return data


if __name__ == "__main__":
    check(hilbert(6))

    plot(get_data(power_iteration_method), 'Степенной метод', np.logspace(-2, -6, num=5))
    plot(get_data(scalar_product_method), 'Метод скалярного произведения', np.logspace(-2, -6, num=5))
