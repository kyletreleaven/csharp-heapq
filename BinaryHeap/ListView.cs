using System;
using System.Collections.Generic;

namespace BinaryHeap
{
	public struct ListView<T> : IList<T>
	{
		private IList<T> _list;
		private int _start;

		public ListView(IList<T> list, int start, int count) : this()
		{
			_list = list;
			_start = start;
			this.Count = count;
		}

		#region IList implementation

		public int IndexOf (T item)
		{
			throw new NotImplementedException ();
		}

		public void Insert (int index, T item)
		{
			throw new NotImplementedException ();
		}

		public void RemoveAt (int index)
		{
			throw new NotImplementedException ();
		}

		public T this [int index] {
			get {
				return this._list [this._start + index];
			}
			set {
				this._list [this._start + index] = value;
			}
		}

		#endregion

		#region ICollection implementation

		public void Add (T item)
		{
			throw new NotImplementedException ();
		}

		public void Clear ()
		{
			throw new NotImplementedException ();
		}

		public bool Contains (T item)
		{
			throw new NotImplementedException ();
		}

		public void CopyTo (T[] array, int arrayIndex)
		{
			throw new NotImplementedException ();
		}

		public bool Remove (T item)
		{
			throw new NotImplementedException ();
		}

		public int Count { get; set; }

		public bool IsReadOnly {
			get {
				return true;
			}
		}

		#endregion

		#region IEnumerable implementation

		public IEnumerator<T> GetEnumerator ()
		{
			for (int k=0; k < this.Count; k++) {
				yield return this._list [this._start + k];
			}
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

