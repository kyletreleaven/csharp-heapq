using System;
using System.Collections.Generic;

namespace BinaryHeap
{
	public static class BinaryHeapExt
	{
		private static int Parent(int k)
		{
			return (k-1) >> 1;
		}

		private static int Left(int k)
		{
			return (k << 1) + 1;
		}

		private static int Right(int k)
		{
			return (k << 1) + 2;
		}

		private static void Swap<T>(this IList<T> list,
		                            int i, int j)
		{
			T temp = list [i];
			list [i] = list [j];
			list [j] = temp;
		}

		public static void MinHeapify<T>(this IList<T> heap, int k)
			where T : IComparable<T>
		{
			var heapSize = heap.Count;

			int left, right, smallest;
			left = Left (k);
			right = Right (k);

			smallest = 
				left < heapSize
				&& heap [left].CompareTo( heap[k] ) < 0
					? left
					: k;

			smallest =
				right < heapSize
					&& heap [right].CompareTo( heap[smallest] ) < 0
					? right
					: smallest;

			if (smallest != k) {
				heap.Swap (k, smallest);
				heap.MinHeapify (smallest);
			}
		}

		public static void MakeMinHeap<T>(this IList<T> heap)
			where T : IComparable<T>
		{
			for (int k=heap.Count-1; k >= 0; k--) {
				heap.MinHeapify (k);
			}
		}

		// buggy, but good progress tonight
		public static void HeapSort<T>(this IList<T> heap)
			where T : IComparable<T>
		{
			heap.MakeMinHeap ();

			var shrinkHeap = new ListView<T> (heap, 0, heap.Count);

			for (int k=heap.Count-1; k > 0; k--) {
				heap.Swap (k, 0);
				shrinkHeap.Count--;
				shrinkHeap.MinHeapify (0);
			}
		}
	}
}