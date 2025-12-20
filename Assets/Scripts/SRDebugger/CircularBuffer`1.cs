using System;
using System.Collections;
using System.Collections.Generic;

namespace SRDebugger
{
	public class CircularBuffer<T> : IEnumerable<T>, IReadOnlyList<T>, IEnumerable
	{
		public CircularBuffer(int capacity) : this(capacity, new T[0])
		{
		}

		public CircularBuffer(int capacity, T[] items)
		{
			if (capacity < 1)
			{
				throw new ArgumentException("Circular buffer cannot have negative or zero capacity.", "capacity");
			}
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			if (items.Length > capacity)
			{
				throw new ArgumentException("Too many items to fit circular buffer", "items");
			}
			this._buffer = new T[capacity];
			Array.Copy(items, this._buffer, items.Length);
			this._count = items.Length;
			this._start = 0;
			this._end = ((this._count != capacity) ? this._count : 0);
		}

		public int Capacity
		{
			get
			{
				return this._buffer.Length;
			}
		}

		public bool IsFull
		{
			get
			{
				return this.Count == this.Capacity;
			}
		}

		public bool IsEmpty
		{
			get
			{
				return this.Count == 0;
			}
		}

		public int Count
		{
			get
			{
				return this._count;
			}
		}

		public T this[int index]
		{
			get
			{
				if (this.IsEmpty)
				{
					throw new IndexOutOfRangeException(string.Format("Cannot access index {0}. Buffer is empty", index));
				}
				if (index >= this._count)
				{
					throw new IndexOutOfRangeException(string.Format("Cannot access index {0}. Buffer size is {1}", index, this._count));
				}
				int num = this.InternalIndex(index);
				return this._buffer[num];
			}
			set
			{
				if (this.IsEmpty)
				{
					throw new IndexOutOfRangeException(string.Format("Cannot access index {0}. Buffer is empty", index));
				}
				if (index >= this._count)
				{
					throw new IndexOutOfRangeException(string.Format("Cannot access index {0}. Buffer size is {1}", index, this._count));
				}
				int num = this.InternalIndex(index);
				this._buffer[num] = value;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			ArraySegment<T>[] segments = new ArraySegment<T>[]
			{
				this.ArrayOne(),
				this.ArrayTwo()
			};
			foreach (ArraySegment<T> segment in segments)
			{
				for (int i = 0; i < segment.Count; i++)
				{
					yield return segment.Array[segment.Offset + i];
				}
			}
			yield break;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public T Front()
		{
			this.ThrowIfEmpty("Cannot access an empty buffer.");
			return this._buffer[this._start];
		}

		public T Back()
		{
			this.ThrowIfEmpty("Cannot access an empty buffer.");
			return this._buffer[((this._end == 0) ? this._count : this._end) - 1];
		}

		public void PushBack(T item)
		{
			if (this.IsFull)
			{
				this._buffer[this._end] = item;
				this.Increment(ref this._end);
				this._start = this._end;
			}
			else
			{
				this._buffer[this._end] = item;
				this.Increment(ref this._end);
				this._count++;
			}
		}

		public void PushFront(T item)
		{
			if (this.IsFull)
			{
				this.Decrement(ref this._start);
				this._end = this._start;
				this._buffer[this._start] = item;
			}
			else
			{
				this.Decrement(ref this._start);
				this._buffer[this._start] = item;
				this._count++;
			}
		}

		public void PopBack()
		{
			this.ThrowIfEmpty("Cannot take elements from an empty buffer.");
			this.Decrement(ref this._end);
			this._buffer[this._end] = default(T);
			this._count--;
		}

		public void PopFront()
		{
			this.ThrowIfEmpty("Cannot take elements from an empty buffer.");
			this._buffer[this._start] = default(T);
			this.Increment(ref this._start);
			this._count--;
		}

		public T[] ToArray()
		{
			T[] array = new T[this.Count];
			int num = 0;
			ArraySegment<T>[] array2 = new ArraySegment<T>[]
			{
				this.ArrayOne(),
				this.ArrayTwo()
			};
			foreach (ArraySegment<T> arraySegment in array2)
			{
				Array.Copy(arraySegment.Array, arraySegment.Offset, array, num, arraySegment.Count);
				num += arraySegment.Count;
			}
			return array;
		}

		private void ThrowIfEmpty(string message = "Cannot access an empty buffer.")
		{
			if (this.IsEmpty)
			{
				throw new InvalidOperationException(message);
			}
		}

		private void Increment(ref int index)
		{
			if (++index == this.Capacity)
			{
				index = 0;
			}
		}

		private void Decrement(ref int index)
		{
			if (index == 0)
			{
				index = this.Capacity;
			}
			index--;
		}

		private int InternalIndex(int index)
		{
			return this._start + ((index >= this.Capacity - this._start) ? (index - this.Capacity) : index);
		}

		private ArraySegment<T> ArrayOne()
		{
			if (this._start < this._end)
			{
				return new ArraySegment<T>(this._buffer, this._start, this._end - this._start);
			}
			return new ArraySegment<T>(this._buffer, this._start, this._buffer.Length - this._start);
		}

		private ArraySegment<T> ArrayTwo()
		{
			if (this._start < this._end)
			{
				return new ArraySegment<T>(this._buffer, this._end, 0);
			}
			return new ArraySegment<T>(this._buffer, 0, this._end);
		}

		private readonly T[] _buffer;

		private int _end;

		private int _count;

		private int _start;
	}
}
