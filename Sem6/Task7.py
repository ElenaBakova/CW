import math
from math import sqrt

import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from numpy import sort
from scipy.linalg import hilbert as hilbert


# сумма всех элементов строки i без диагонального
def find_r_i(A: np.array, i: int):
    n = A.shape[0]
    r_i = 0
    for j in range(n):
        if j == i:
            continue
        r_i += abs(A[i, j])
    return r_i


def find_r_list(A: np.array):
    n = A.shape[0]
    r = []
    for i in range(n):
        r.append(find_r_i(A, i))
    return r


def check_gershgorin(A: np.array, eigen_values: list) -> bool:
    n = A.shape[0]
    r = find_r_list(A)

    for eigen_value in eigen_values:
        answer = False
        for i in range(n):
            if abs(eigen_value - A[i, i]) <= r[i]:
                answer = True
                break
        if not answer:
            return False
    return True


#
# def is_upper_triangular(A: np.array): return np.allclose(A, np.triu(A))
#
#
# def is_diagonal(A: np.array): return np.allclose(A, np.tril(np.triu(A)))


def find_sin_cos(A: np.array, i: int, j: int):
    x = -2 * A[i, j]
    y = A[i, i] - A[j, j]
    if y == 0:
        cos = 1 / sqrt(2)
        sin = 1 / sqrt(2)
    else:
        cos = math.sqrt(0.5 * (1 + abs(y) / math.sqrt(x ** 2 + y ** 2)))
        sin = np.sign(x * y) * abs(x) / (2 * cos * math.sqrt(x ** 2 + y ** 2))
    return sin, cos


def create_rotational_matrix(n, sin, cos, i, j) -> np.array:
    matrix = np.eye(n)
    matrix[i, i] = matrix[j, j] = cos
    if i > j:
        matrix[i, j] = sin
        matrix[j, i] = -sin
    else:
        matrix[i, j] = -sin
        matrix[j, i] = sin
    return matrix


# for the case when i < j
def find_max_abs_non_diagonal_element_with_position(A: np.array):
    max_element = (-math.inf, (-1, -1))
    n = A.shape[0]
    for i in range(n):
        for j in range(n):
            if i == j:
                continue
            if abs(A[i, j]) > max_element[0]:
                max_element = (abs(A[i, j]), (i, j))
    # print(max_element[0])
    return max_element[1]


class NextNonDiagonalElementFinder:
    def __init__(self, n):
        assert n >= 2, "n must be greater than 1"
        self.indices_list = []
        self.current_index_pair_pointer = 0

        for i in range(n):
            for j in range(n):
                if i != j:
                    self.indices_list.append((i, j))

    def get_next_index_pair(self):
        if self.current_index_pair_pointer == len(self.indices_list):
            self.current_index_pair_pointer = 0
        value = self.indices_list[self.current_index_pair_pointer]
        self.current_index_pair_pointer += 1
        return value


# strategy:
# 1 -> max upper non-diagonal
# 2 -> next numbered non-diagonal
def jacobi(A: np.array, strategy: int, epsilon=10 ** 0):
    if not np.allclose(A, A.T):
        raise ValueError('Матрица должна быть симметричной')

    n = A.shape[0]
    R = A.copy()
    iteration_steps = 0
    next_index_pair_finder = NextNonDiagonalElementFinder(n)

    while len([r for r in find_r_list(R) if r >= epsilon]) > 0 and iteration_steps < 1000:
        if strategy == 1:
            i, j = find_max_abs_non_diagonal_element_with_position(R)
        else:
            i, j = next_index_pair_finder.get_next_index_pair()
        sin, cos = find_sin_cos(R, i, j)
        rotational_matrix = create_rotational_matrix(n, sin, cos, i, j)

        # print(iteration_steps)
        # print(f"R list: {find_r_list(R)}")
        # print(f"R:\n{R}")
        # print(f"rotational {i} {j}:\n{rotational_matrix}")
        # print()

        R = rotational_matrix.T @ R @ rotational_matrix
        iteration_steps += 1
    assert check_gershgorin(A, list(sort(np.diag(R)))), "Gershgorin fails"
    return R, iteration_steps


def check(A: np.array, strategy: int, epsilon=10 ** 0, info=False):
    exp_eigs = sort(np.linalg.eig(A)[0])
    R, iteration_steps = jacobi(A, strategy, epsilon)
    if info:
        print(f"Количество итераций: {iteration_steps}")
    act_eigs = sort(np.diag(R))
    if info:
        print(f"Ожидается: {exp_eigs}")
        print(f"Получено: {act_eigs}")
        print(f"Погрешность: {[np.linalg.norm(expected - actual) for (expected, actual) in zip(exp_eigs, act_eigs)]}")
    return iteration_steps, act_eigs


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


def get_data(gen, strategy=1):
    data = {}
    for size in range(2, 12):
        key = f'{size}x{size}'
        data[key] = []
        A = gen(size)

        for eps in np.logspace(-2, -6, num=5):
            actual, k = jacobi(A, strategy, eps)
            data[key].append(k)
    return data


def gen_symmetric(N):
    a = np.random.rand(N, N)
    return np.tril(a) + np.tril(a, -1).T


if __name__ == "__main__":
    A = np.array([[6, 5, 0],
                  [5, 1, 4],
                  [0, 4, 3]], float)
    epsilon = 10 ** (-2)
    check(A, 1, epsilon, True)

    space = np.logspace(-2, -6, num=5)
    plot(get_data(hilbert, 1), 'Максимальный по модулю элемент, матрица Гильберта', space)
    plot(get_data(gen_symmetric, 1), 'Максимальный по модулю элемент, случайная симметричная матрица', space)
    plot(get_data(hilbert, 2), 'Циклический выбор, матрица Гильберта', space)
    plot(get_data(gen_symmetric, 2), 'Циклический выбор, случайная симметричная матрица', space)
