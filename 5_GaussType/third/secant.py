eps = 0.000000000001
n = 1000


def separate_roots(a, b, func, num):
    segments = []
    step = (b - a) / n
    x1 = a
    x2 = x1 + step
    y1 = func(x1, num)
    while x2 <= b:
        y2 = func(x2, num)
        if y1 * y2 <= 0:
            segments.append((x1, x2))
        x1 = x2
        x2 += step
        y1 = y2

    return segments


def secant(a, b, func, num):
    while True:
        temp = a
        a -= func(a, num) / (func(a, num) - func(b, num)) * (a - b)
        b = temp
        if abs(a - b) < eps:
            break
    return a


# Finds roots of func, returns list of roots
def count(a, b, func, num):
    segments = separate_roots(a, b, func, num)
    roots = []
    for item in segments:
        root = secant(item[0], item[1], func, num)
        roots.append(root)

    return roots
