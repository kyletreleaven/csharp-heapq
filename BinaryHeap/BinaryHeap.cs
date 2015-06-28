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

		private static void FixMinHeap<T>(this IList<T> heap, int k)
			where T : IComparable<T>
		{
			int heapSize = heap.Count;

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
				heap.FixMinHeap (smallest);
			}
		}

		public static void Heapify<T>(this IList<T> heap)
			where T : IComparable<T>
		{
			for (int k=heap.Count-1; k >= 0; k--) {
				heap.FixMinHeap (k);
			}
		}

		private static bool CheckMinHeapNode<T>(this IList<T> heap, int k)
			where T : IComparable<T>
		{
			int left = Left(k), right = Right(k);
			if (left < heap.Count && heap [k].CompareTo (heap [left]) > 0)
				return false;
			if (right < heap.Count && heap[k].CompareTo(heap[right]) > 0)
				return false;

			return true;
		}

		public static bool IsMinHeap<T>(this IList<T> heap)
			where T : IComparable<T>
		{
			for (int k=0; k<heap.Count; k++) {
				if (!heap.CheckMinHeapNode (k))
					return false;
			}
			return true;
		}

		// TODO: need to invert polarity
		public static void HeapSort<T>(this IList<T> heap)
			where T : IComparable<T>
		{
			heap.Heapify ();

			var shrinkHeap = new ListView<T> (heap, 0, heap.Count);

			for (int k=heap.Count-1; k > 0; k--) {
				heap.Swap (k, 0);
				shrinkHeap.Count--;
				shrinkHeap.FixMinHeap(0);
			}
		}

		public static T HeapPeek<T>(this IList<T> heap)
		{
			return heap [0];
		}

		public static T HeapPop<T>(this IList<T> heap)
			where T : IComparable<T>
		{
			T min = heap [0];
			heap [0] = heap [heap.Count-1];
			heap.RemoveAt(heap.Count-1);
			heap.FixMinHeap (0);
			return min;
		}

		private static void FixDecreasedKey<T>(this IList<T> heap, int k)
			where T : IComparable<T>
		{
			int i = k;
			while (i > 0 && heap[Parent(i)].CompareTo( heap[i] ) > 0 ) {
				heap.Swap (i, Parent (i));
				i = Parent (i);
			}
		}

		public static void HeapPush<T>(this IList<T> heap, T item)
			where T : IComparable<T>
		{
			int k = heap.Count;
			heap.Add (item);
			heap.FixDecreasedKey (k);
		}
	}
}