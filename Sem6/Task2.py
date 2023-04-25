import seaborn as sns
import numpy as np
import math
import pandas as pd
import matplotlib.pyplot as plt
from itertools import product
from scipy.linalg import hilbert
import unittest
from tabulate import tabulate
import numpy.testing


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


def iterative(A, b, E, get_iterations=False, pos_definitive=False, max_iter=100000):
    N = A.shape[0]
    alpha, beta = transform(A, b)

    # Проверяем условие сходимости (спектральный радиус < 1)
    p = max(abs(np.linalg.eigvals(alpha)))
    if p >= 1:
        raise ValueError(f'p(B) == {p} >= 1, метод простой итерации не сходится')

    x_prev = beta
    iter = 0
    while iter < max_iter:
        iter += 1
        x_next = alpha @ x_prev + beta
        if posteriori_error(x_next, x_prev, alpha) < E:
            break
        x_prev = x_next

    if get_iterations:
        return x_next, iter
    else:
        return x_next
