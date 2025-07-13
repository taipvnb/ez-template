using System;

namespace com.ez.engine.foundation
{
	public class BinaryHeap<T> where T : IBinaryHeapElement
	{
		public int Count => _lastChildIndex;

		private T[] _array;
		private int _lastChildIndex;
		private BinaryHeapSortMode _sortMode;
		private const float GrowthFactor = 1.6f;

		public BinaryHeap(int minSize, BinaryHeapSortMode sortMode)
		{
			_sortMode = sortMode;
			_array = new T[minSize];
			_lastChildIndex = 0;
		}

		public BinaryHeap(T[] dataArray, BinaryHeapSortMode sortMode)
		{
			_sortMode = sortMode;
			var minSize = 16;
			if (dataArray != null)
			{
				minSize = dataArray.Length + 1;
			}

			_array = new T[minSize];
			_lastChildIndex = 0;
			Insert(dataArray, BinaryHeapBuildMode.N);
		}

		public void Clear()
		{
			_array = new T[16];
			_lastChildIndex = 0;
		}

		public void Insert(T[] dataArray, BinaryHeapBuildMode buildMode)
		{
			if (dataArray == null)
			{
				throw new NullReferenceException("BinaryHeap Not Support Insert Null Object");
			}

			var totalLength = _lastChildIndex + dataArray.Length + 1;
			if (_array.Length < totalLength)
			{
				ResizeArray(totalLength);
			}

			if (buildMode == BinaryHeapBuildMode.NLog)
			{
				for (var i = 0; i < dataArray.Length; ++i)
				{
					Insert(dataArray[i]);
				}
			}
			else
			{
				for (var i = 0; i < dataArray.Length; ++i)
				{
					_array[++_lastChildIndex] = dataArray[i];
				}

				SortAsCurrentMode();
			}
		}

		public void Insert(T element)
		{
			if (element == null)
			{
				throw new NullReferenceException("BinaryHeap Not Support Insert Null Object");
			}

			var index = ++_lastChildIndex;

			if (index == _array.Length)
			{
				ResizeArray();
			}

			_array[index] = element;

			PercolateUp(index);
		}

		public T Pop()
		{
			if (_lastChildIndex < 1)
			{
				return default;
			}

			var result = _array[1];
			_array[1] = _array[_lastChildIndex];
			_array[_lastChildIndex] = default;
			_lastChildIndex--;
			if (_lastChildIndex > 0)
			{
				PercolateDown(1);
			}

			return result;
		}

		public T Top()
		{
			if (_lastChildIndex < 1)
			{
				return default;
			}

			return _array[1];
		}

		public void Sort(BinaryHeapSortMode sortMode)
		{
			if (_sortMode == sortMode)
			{
				return;
			}

			_sortMode = sortMode;
			SortAsCurrentMode();
		}

		public void RebuildAtIndex(int index)
		{
			if (index > _lastChildIndex || index < 1)
			{
				return;
			}

			var element = _array[index];
			if (element == null)
			{
				return;
			}

			var parentIndex = index >> 1;
			if (parentIndex > 0)
			{
				if (_sortMode == BinaryHeapSortMode.Min)
				{
					if (element.SortScore < _array[parentIndex].SortScore)
					{
						PercolateUp(index);
					}
					else
					{
						PercolateDown(index);
					}
				}
				else
				{
					if (element.SortScore > _array[parentIndex].SortScore)
					{
						PercolateUp(index);
					}
					else
					{
						PercolateDown(index);
					}
				}
			}
			else
			{
				PercolateDown(index);
			}
		}

		private void SortAsCurrentMode()
		{
			var startChild = _lastChildIndex >> 1;
			for (var i = startChild; i > 0; --i)
			{
				PercolateDown(i);
			}
		}

		public void RemoveAt(int index)
		{
			if (index > _lastChildIndex || index < 1)
			{
				return;
			}

			if (index == _lastChildIndex)
			{
				_array[index] = default;
				_lastChildIndex--;
				return;
			}

			_array[index] = _array[_lastChildIndex];
			_array[_lastChildIndex] = default;
			_lastChildIndex--;
			if (_array[index] != null)
			{
				_array[index].HeapIndex = index;
				RebuildAtIndex(index);
			}
		}

		public T GetElement(int index)
		{
			if (index > _lastChildIndex)
			{
				return default;
			}

			return _array[index];
		}

		public bool HasValue()
		{
			return _lastChildIndex > 0;
		}

		private void ResizeArray(int newSize = -1)
		{
			if (newSize < 0)
			{
				newSize = Math.Max(_array.Length + 4, (int)Math.Round(_array.Length * GrowthFactor));
			}

			if (newSize > 1 << 20)
			{
				throw new Exception(
					"Binary Heap Size too large (2^20). A heap size this large is probably the cause of pathfinding running in an infinite loop. " +
					"\nRemove this check (in BinaryHeap.cs) if you are sure that it is not caused by a bug");
			}

			var tmp = new T[newSize];
			for (var i = 0; i < _array.Length; i++)
			{
				tmp[i] = _array[i];
			}

			_array = tmp;
		}

		private void PercolateUp(int index)
		{
			var element = _array[index];
			if (element == null)
			{
				return;
			}

			var sortScore = element.SortScore;

			var parentIndex = index >> 1;

			if (_sortMode == BinaryHeapSortMode.Min)
			{
				while (parentIndex >= 1 && sortScore < _array[parentIndex].SortScore)
				{
					_array[index] = _array[parentIndex];
					_array[index].HeapIndex = index;
					index = parentIndex;
					parentIndex = index >> 1;
				}
			}
			else
			{
				while (parentIndex >= 1 && sortScore > _array[parentIndex].SortScore)
				{
					_array[index] = _array[parentIndex];
					_array[index].HeapIndex = index;
					index = parentIndex;
					parentIndex = index >> 1;
				}
			}

			_array[index] = element;
			_array[index].HeapIndex = index;
		}

		private void PercolateDown(int index)
		{
			var element = _array[index];
			if (element == null)
			{
				return;
			}

			var childIndex = index << 1;

			if (_sortMode == BinaryHeapSortMode.Min)
			{
				while (childIndex <= _lastChildIndex)
				{
					if (childIndex != _lastChildIndex)
					{
						if (_array[childIndex + 1].SortScore < _array[childIndex].SortScore)
						{
							childIndex = childIndex + 1;
						}
					}

					if (_array[childIndex].SortScore < element.SortScore)
					{
						_array[index] = _array[childIndex];
						_array[index].HeapIndex = index;
					}
					else
					{
						break;
					}

					index = childIndex;
					childIndex = index << 1;
				}
			}
			else
			{
				while (childIndex <= _lastChildIndex)
				{
					if (childIndex != _lastChildIndex)
					{
						if (_array[childIndex + 1].SortScore > _array[childIndex].SortScore)
						{
							childIndex = childIndex + 1;
						}
					}

					if (_array[childIndex].SortScore > element.SortScore)
					{
						_array[index] = _array[childIndex];
						_array[index].HeapIndex = index;
					}
					else
					{
						break;
					}

					index = childIndex;
					childIndex = index << 1;
				}
			}

			_array[index] = element;
			_array[index].HeapIndex = index;
		}
	}
}
