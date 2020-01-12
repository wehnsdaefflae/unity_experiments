class Brane():
	def __init__(location: int, subbranes: Optional[Tuple[Brane, ...]] = None):
		self.location = location
		if subbranes is None:
			self.dim = 0

		else:
			no_subbranes = len(subbranes)
			assert 1 < no_subbranes
			assert no_subbranes % 2 == 0
			self.dim = no_subbranes // 2
			assert all(x.dim == self.dim - 1 for x in subbranes)

		self.subbranes = subbranes


	def middle() -> Sequence[int]:
		if brane.dim == 0:
			return [self.location]

		elif brane.dim == 1:
			return [sum(x.location for x in brane.subbranes) / len(brane.subbranes)]

		else:
			for each_brane in brane.subbranes:
				i = interpolate(each_brane)
				coordinate.append(i)


