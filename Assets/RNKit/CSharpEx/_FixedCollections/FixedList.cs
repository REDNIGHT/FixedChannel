using System;
using System.Runtime.CompilerServices;

namespace RN.CSharpEx.FixedCollections
{
	public partial struct FixedList<T, TBuffer>
		where T : unmanaged
		where TBuffer : unmanaged, IFixedBuffer
	{
		internal TBuffer buffer;

		internal unsafe byte* pointer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this.buffer.pointer;
		}
		public readonly int capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this.buffer.capacity;
		}

		public int length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private set;
		}

		public readonly bool isFull => this.length == this.capacity;

		public readonly bool hasEmpty => this.capacity > this.length;

		public readonly int index
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this.length - 1;
		}


		public ref T this[int i]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{

				if (i < 0)
					throw new IndexOutOfRangeException("i < 0");

				if (i >= this.length)
					throw new IndexOutOfRangeException($"i > count  i={i}  count={this.length}");

				unsafe
				{
					return ref Unsafe.AsRef<T>(this.pointer + Unsafe.SizeOf<T>() * i);
				}
			}
		}

		public ref T first
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (this.length == 0) throw new IndexOutOfRangeException($"this.count == 0");
				return ref this[0];
			}
		}
		public ref T last
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (this.length == 0) throw new IndexOutOfRangeException($"this.count == 0");
				return ref this[this.index];
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(in T value)
		{
			if (this.length + 1 > this.capacity)
				throw new IndexOutOfRangeException($"count + 1 > length  count={this.length}  capacity={this.capacity}");

			++this.length;
			this[this.index] = value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Remove()
		{
			if (this.length <= 0)
				throw new IndexOutOfRangeException("count <= 0");

			var t = this[this.index];
			--this.length;
			return t;
		}


		//todo...
		public void Insert(int index, T item) => throw new NotImplementedException();
		public void InsertRange(int index, ReadOnlySpan<T> collection) => throw new NotImplementedException();
		public bool Remove(T item) => throw new NotImplementedException();
		public void RemoveAt(int index) => throw new NotImplementedException();
		public void RemoveRange(int index, int count) => throw new NotImplementedException();


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear() => this.length = 0;
		
		
		
		//
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe Span<T> AsSpan() => new Span<T>(this.pointer, this.length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ReadOnlySpan<T> AsReadOnlySpan() => new ReadOnlySpan<T>(this.pointer, this.length);


		//
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Span<T>(in FixedList<T, TBuffer> list) => Unsafe.AsRef(list).AsSpan();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ReadOnlySpan<T>(in FixedList<T, TBuffer> list) => Unsafe.AsRef(list).AsReadOnlySpan();
	}
	
}
