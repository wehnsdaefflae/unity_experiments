from typing import Tuple, Dict


def noise(matrix: Dict[Tuple[int, ...], int], point_origin: Tuple[int, ...], size: int):
	dim = len(point_origin)
	value_a = matrix[point_origin]

	for i in range(dim):
		point_b = list(point_origin)
		point_mid = list(point_origin)

		for j in range(i + 1):
			point_b[j] = size
			point_mid[j] = (point_origin[j] + size) // 2

		value_b = matrix[tuple(point_b)]
		value_mid = (value_a + value_b) // 2
		matrix[tuple(point_mid)] = value_mid
