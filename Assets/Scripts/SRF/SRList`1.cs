using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace SRF
{
	[Serializable]
	public class SRList<T> : IEnumerable<T>, IList<T>, ICollection<T>, IEnumerable, ISerializationCallbackReceiver
	{
		public SRList()
		{
		}

		public SRList(int capacity)
		{
			this.Buffer = new T[capacity];
		}

		public SRList(IEnumerable<T> source)
		{
			this.AddRange(source);
		}

		public T[] Buffer
		{
			get
			{
				return this._buffer;
			}
			private set
			{
				this._buffer = value;
			}
		}

		private EqualityComparer<T> EqualityComparer
		{
			get
			{
				if (this._equalityComparer == null)
				{
					this._equalityComparer = EqualityComparer<T>.Default;
				}
				return this._equalityComparer;
			}
		}

		public int Count
		{
			get
			{
				return this._count;
			}
			private set
			{
				this._count = value;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			if (this.Buffer != null)
			{
				for (int i = 0; i < this.Count; i++)
				{
					yield return this.Buffer[i];
				}
			}
			yield break;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public void Add(T item)
		{
			if (this.Buffer == null || this.Count == this.Buffer.Length)
			{
				this.Expand();
			}
			this.Buffer[this.Count++] = item;
		}

		public void Clear()
		{
			this.Count = 0;
		}

		public bool Contains(T item)
		{
			if (this.Buffer == null)
			{
				return false;
			}
			for (int i = 0; i < this.Count; i++)
			{
				if (this.EqualityComparer.Equals(this.Buffer[i], item))
				{
					return true;
				}
			}
			return false;
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			this.Trim();
			this.Buffer.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item)
		{
			if (this.Buffer == null)
			{
				return false;
			}
			int num = this.IndexOf(item);
			if (num < 0)
			{
				return false;
			}
			this.RemoveAt(num);
			return true;
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public int IndexOf(T item)
		{
			if (this.Buffer == null)
			{
				return -1;
			}
			for (int i = 0; i < this.Count; i++)
			{
				if (this.EqualityComparer.Equals(this.Buffer[i], item))
				{
					return i;
				}
			}
			return -1;
		}

		public void Insert(int index, T item)
		{
			if (this.Buffer == null || this.Count == this.Buffer.Length)
			{
				this.Expand();
			}
			if (index < this.Count)
			{
				for (int i = this.Count; i > index; i--)
				{
					this.Buffer[i] = this.Buffer[i - 1];
				}
				this.Buffer[index] = item;
				this.Count++;
			}
			else
			{
				this.Add(item);
			}
		}

		public void RemoveAt(int index)
		{
			if (this.Buffer != null && index < this.Count)
			{
				this.Count--;
				this.Buffer[index] = default(T);
				for (int i = index; i < this.Count; i++)
				{
					this.Buffer[i] = this.Buffer[i + 1];
				}
			}
		}

		public T this[int index]
		{
			get
			{
				if (this.Buffer == null)
				{
					throw new IndexOutOfRangeException();
				}
				return this.Buffer[index];
			}
			set
			{
				if (this.Buffer == null)
				{
					throw new IndexOutOfRangeException();
				}
				this.Buffer[index] = value;
			}
		}

		public void OnBeforeSerialize()
		{
			UnityEngine.Debug.Log("[OnBeforeSerialize] Count: {0}".Fmt(new object[]
			{
				this._count
			}));
			this.Clean();
		}

		public void OnAfterDeserialize()
		{
			UnityEngine.Debug.Log("[OnAfterDeserialize] Count: {0}".Fmt(new object[]
			{
				this._count
			}));
		}

		public void AddRange(IEnumerable<T> range)
		{
			foreach (T item in range)
			{
				this.Add(item);
			}
		}

		public void Clear(bool clean)
		{
			this.Clear();
			if (!clean)
			{
				return;
			}
			this.Clean();
		}

		public void Clean()
		{
			if (this.Buffer == null)
			{
				return;
			}
			for (int i = this.Count; i < this._buffer.Length; i++)
			{
				this._buffer[i] = default(T);
			}
		}

		public ReadOnlyCollection<T> AsReadOnly()
		{
			if (this._readOnlyWrapper == null)
			{
				this._readOnlyWrapper = new ReadOnlyCollection<T>(this);
			}
			return this._readOnlyWrapper;
		}

		private void Expand()
		{
			T[] array = (this.Buffer == null) ? new T[32] : new T[Mathf.Max(this.Buffer.Length << 1, 32)];
			if (this.Buffer != null && this.Count > 0)
			{
				this.Buffer.CopyTo(array, 0);
			}
			this.Buffer = array;
		}

		public void Trim()
		{
			if (this.Count > 0)
			{
				if (this.Count >= this.Buffer.Length)
				{
					return;
				}
				T[] array = new T[this.Count];
				for (int i = 0; i < this.Count; i++)
				{
					array[i] = this.Buffer[i];
				}
				this.Buffer = array;
			}
			else
			{
				this.Buffer = new T[0];
			}
		}

		public void Sort(Comparison<T> comparer)
		{
			bool flag = true;
			while (flag)
			{
				flag = false;
				for (int i = 1; i < this.Count; i++)
				{
					if (comparer(this.Buffer[i - 1], this.Buffer[i]) > 0)
					{
						T t = this.Buffer[i];
						this.Buffer[i] = this.Buffer[i - 1];
						this.Buffer[i - 1] = t;
						flag = true;
					}
				}
			}
		}

		[SerializeField]
		private T[] _buffer;

		[SerializeField]
		private int _count;

		private EqualityComparer<T> _equalityComparer;

		private ReadOnlyCollection<T> _readOnlyWrapper;
	}
}
