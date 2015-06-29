using NUnit.Framework;
using System;
using BinaryHeap;
using System.Collections.Generic;

namespace BinaryHeapTest
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void HeapPushTest ()
		{
			var heap = new List<int> ();

			heap.HeapPush (5);
			heap.HeapPush (6);
			heap.HeapPush (1);
			heap.HeapPush (0);

			Assert.IsTrue (heap.IsMinHeap ());

			// 0 1 5 6
			Assert.IsTrue (heap.HeapPeek () == 0);
			Assert.IsTrue (heap.IsMinHeap ());

			// 0 1 5 6
			Assert.IsTrue (heap.HeapPop () == 0);
			Assert.IsTrue (heap.IsMinHeap ());

			// 1 5 6
			Assert.IsTrue (heap.HeapPushPop (2) == 1);
			Assert.IsTrue (heap.IsMinHeap ());

			// 2 5 6
			Assert.IsTrue (heap.HeapReplace (0) == 2);
			Assert.IsTrue (heap.IsMinHeap ());

			// 0 5 6
			Assert.IsTrue (heap.HeapPushPop (-1) == -1);
			Assert.IsTrue (heap.IsMinHeap ());

			// -1 5 6
		}
	}
}

