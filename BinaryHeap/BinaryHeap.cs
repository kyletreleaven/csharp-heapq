/*
 * Copyright (c) 2015 Kyle Treleaven (ktreleav@gmail.com)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

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

		/// <summary>
		/// Swaps the elements of an IList at two indices, i and j.
		/// </summary>
		/// <param name="list">List.</param>
		/// <param name="i">The index.</param>
		/// <param name="j">J.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private static void Swap<T>(this IList<T> list,
		                            int i, int j)
		{
			T temp = list [i];
			list [i] = list [j];
			list [j] = temp;
		}

		/// <summary>
		/// Recursively fix the min heap rooted at node (index) k, which may violate the min heap invariant;
		/// however, assumes the invariant holds on both left and right branches
		/// </summary>
		/// <param name="heap">Heap.</param>
		/// <param name="k">K.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
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

		/// <summary>
		/// Turn the IList into a min heap in O(n) time.
		/// </summary>
		/// <param name="heap">Heap.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
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

		/// <summary>
		/// Obtain the min element of a heap.
		/// </summary>
		/// <returns>The peek.</returns>
		/// <param name="heap">Heap.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T HeapPeek<T>(this IList<T> heap)
		{
			return heap [0];
		}

		/// <summary>
		/// Remove and return the min value from a min heap, maintaining the min heap invariant.
		/// </summary>
		/// <returns>The pop.</returns>
		/// <param name="heap">Heap.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
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

		/// <summary>
		/// Push a new item into a heap (list), maintaining the min heap invariant.
		/// </summary>
		/// <param name="heap">Heap.</param>
		/// <param name="item">Item.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void HeapPush<T>(this IList<T> heap, T item)
			where T : IComparable<T>
		{
			int k = heap.Count;
			heap.Add (item);
			heap.FixDecreasedKey (k);
		}

		/// <summary>
		/// Compound operation: push item to the heap, then pop an item from the heap;
		/// faster than HeapPush() followed by HeapPop()
		/// </summary>
		/// <returns>The push pop.</returns>
		/// <param name="heap">Heap.</param>
		/// <param name="item">Item.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T HeapPushPop<T>(this IList<T> heap, T item)
			where T : IComparable<T>
		{
			T min = heap [0];

			// if incoming is smaller
			if (item.CompareTo (min) < 0) {
				// no work!
				return item;
			}

			// otherwise,
			heap [0] = item;
			heap.FixMinHeap (0);
			return min;
		}

		/// <summary>
		/// Compound operation: pop an item from the heap, then push an item to the heap;
		/// faster than HeapPop() followed by HeapPush()
		/// </summary>
		/// <returns>The replace.</returns>
		/// <param name="heap">Heap.</param>
		/// <param name="item">Item.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T HeapReplace<T>(this IList<T> heap, T item)
			where T : IComparable<T>
		{
			T min = heap [0];
			heap [0] = item;
			heap.FixMinHeap (0);
			return min;
		}

		// TODO: The three Python heapq general purpose functions based on heaps
		// https://docs.python.org/2/library/heapq.html

		public static IEnumerable<T> HeapMerge<T>(this IList<IEnumerable<T>> enumerables )
			where T : IComparable<T>
		{
			throw new NotImplementedException ();
		}

		public static IEnumerable<T> nSmallest<T>(this IEnumerable<T> source, int n )
			where T : IComparable<T>
		{
			throw new NotImplementedException ();
		}

		public static IEnumerable<T> nLargest<T>(this IEnumerable<T> source, int n )
			where T : IComparable<T>
		{
			throw new NotImplementedException ();
		}
	}
}
