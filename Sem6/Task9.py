from math import sin, cos, log

import matplotlib.pyplot as plt
import numpy as np
from matplotlib import cm


# ------------------- Явная схема ---------------------
# Задаем начальные г.у.
# Разбиваем область на сетку
# Итерационно вычисляем значения температуры на сетке
def grid(a, b, N):
    return np.linspace(a, b, N)


def imply_boundary_conditions(u, L, T, Nx, Nt):
    x_grid = grid(0, L, Nx + 1)  # Узлы по пространству
    t_grid = grid(0, T, Nt + 1)  # Узлы по времени

    # Инициализация массива для хранения значений температуры
    U = np.zeros((Nx + 1, Nt + 1))

    for i in range(Nx + 1):
        U[i, 0] = u(x_grid[i], 0)
    for i in range(Nt + 1):
        U[0, i] = u(x_grid[0], t_grid[i])
        U[Nx, i] = u(x_grid[Nx], t_grid[i])

    return x_grid, t_grid, U


# Nx -- количество узлов по пространству
# Nt -- Количество узлов по времени
def explicit(u, k, f, L, T, Nx, Nt, return_grids=True):
    # 0 <= x <= a, 0 <= t <= T
    x_grid, t_grid, U = imply_boundary_conditions(u=u, L=L, T=T, Nx=Nx, Nt=Nt)
    tau = T / Nt  # Шаг по времени
    h = L / Nx  # Шаг по пространству

    if not 2 * k * tau <= h ** 2:
        # print('Явная схема неустойчива!')
        is_stable = False
    else:
        is_stable = True

    for t in range(1, Nt + 1):
        for x in range(1, Nx):
            diff = U[x - 1, t - 1] - 2 * U[x, t - 1] + U[x + 1, t - 1]
            U[x, t] = U[x, t - 1] + tau * (k / h ** 2 * diff + f(x_grid[x], t_grid[t - 1]))

    if return_grids:
        return x_grid, t_grid, U, is_stable
    else:
        return U, is_stable


# ------------------ Неявная схема (\sigma = 1) --------------------
def implicit(u, k, f, L, T, Nx, Nt, return_grids=True):
    x_grid, t_grid, U = imply_boundary_conditions(u=u, L=L, T=T, Nx=Nx, Nt=Nt)
    tau = T / Nt  # Шаг по времени
    h = L / Nx  # Шаг по пространству

    for t in range(1, Nt + 1):
        lhs = np.zeros((Nx + 1, Nx + 1))
        rhs = np.zeros(Nx + 1)

        lhs[0, 0] = -(tau * k / h + 1)
        lhs[0, 1] = tau * k / h
        rhs[0] = -U[0, t - 1] - tau * f(x_grid[0], t_grid[t])

        lhs[Nx, Nx] = tau * k / h - 1
        lhs[Nx, Nx - 1] = -tau * k / h
        rhs[Nx] = -U[Nx, t - 1] - tau * f(x_grid[Nx], t_grid[t])

        coef = tau * k / h ** 2
        for x in range(1, Nx):
            lhs[x, x] = -2 * coef - 1
            lhs[x, x - 1] = lhs[x, x + 1] = coef

            rhs[x] = -U[x, t - 1] - tau * f(x_grid[x], t_grid[t])

        # Решение СЛАУ
        U[:, t] = np.linalg.solve(lhs, rhs)

    if return_grids:
        return x_grid, t_grid, U
    else:
        return U


# --------------------------- Plotting -----------------------------
def plot_for(ax, X, T, Z, title):
    ax.plot_surface(X, T, Z, cmap=cm.coolwarm, linewidth=0, antialiased=False)
    ax.set_title(title, fontsize=20)
    ax.set_xlabel('x', fontsize=20)
    ax.set_ylabel('t', fontsize=20)
    ax.set_zlabel('Z', fontsize=20)


def plot(u, k, f, L, T, Nx, Nt):
    fig, ax = plt.subplots(1, 2, subplot_kw={"projection": "3d"}, figsize=(30, 10), dpi=80)
    x_grid, t_grid, U_exp, is_stable = explicit(u=u, k=k, f=f, L=L, T=T, Nx=Nx, Nt=Nt)
    U_imp = implicit(u=u, k=k, f=f, L=L, T=T, Nx=Nx, Nt=Nt, return_grids=False)
    X, T = np.meshgrid(x_grid, t_grid)
    explicit_ax = ax[0]
    implicit_ax = ax[1]

    additional_info = '' if is_stable else '(неустойчива)'

    plot_for(explicit_ax, X, T, U_exp, f'Явная схема {additional_info}')
    plot_for(implicit_ax, X, T, U_imp, 'Неявная схема')

    plt.tight_layout()
    plt.show()


if __name__ == "__main__":
    # Коэффициент теплопроводности
    k = 0.01

    # Длина области
    L = 1

    # Время
    T = 1

    # u = lambda x, t: x ** 3 + t ** 3
    # f = lambda x, t: 3 * t ** 2 - k * 6 * x
    #
    # plot(u, k, f, L, T, 20, 20)
    # plot(u, k, f, L, T, 60, 60)
    #
    # u = lambda x, t: t * sin(x)
    # f = lambda x, t: (1 + k * t) * sin(x)
    #
    # plot(u, k, f, L, T, 20, 20)
    # plot(u, k, f, L, T, 66, 66)
    #
    # u = lambda x, t: cos(t) * log(x + 1)
    # f = lambda x, t: -sin(t) * log(x + 1) + k * cos(t) / (x + 1) ** 2
    #
    # plot(u, k, f, L, T, 20, 20)
    #
    # u = lambda x, t: x ** 2 - t ** 2
    # f = lambda x, t: 2 * k - 2 * t
    #
    # plot(u, k, f, L, T, 20, 20)
    #
    # k = 0.035
    #
    # plot(u, k, f, L, T, 20, 20)

    k = 0.01
    L = 6
    T = 6

    u = lambda x, t: sin(t) ** 3 * cos(x) ** 3
    f = lambda x, t: 3 * sin(t) ** 2 * (cos(t) * cos(x) ** 3 + k * sin(t) * (cos(x) ** 3 - 2 * sin(x) ** 2 * cos(x)))

    plot(u, k, f, L, T, 40, 40)

    k = 0.033
    L = 6
    T = 6

    plot(u, k, f, L, T, 100, 100)
