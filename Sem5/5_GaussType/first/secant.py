eps = 0.00000001
n = 1000


def separate_roots(a, b, func):
    segments = []
    step = (b - a) / n
    x1 = a
    x2 = x1 + step
    y1 = func(x1)
    while x2 <= b:
        y2 = func(x2)
        if y1 * y2 <= 0:
            segments.append((x1, x2))
        x1 = x2
        x2 += step
        y1 = y2

    return segments


def secant(a, b, func):
    while True:
        temp = a
        a -= func(a) / (func(a) - func(b)) * (a - b)
        b = temp
        if abs(a - b) < eps:
            break
    return a


def count(a, b, func):
    segments = separate_roots(a, b, func)
    roots = []
    for item in segments:
        root = secant(item[0], item[1], func)
        roots.append(root)

    return roots
