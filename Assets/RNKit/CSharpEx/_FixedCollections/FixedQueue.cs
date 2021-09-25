using System;
using System.Runtime.CompilerServices;

namespace RN.CSharpEx.FixedCollections
{
	public partial struct FixedQueue<T, TBuffer>
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

		public readonly bool isFull => this.length  == this.capacity;
		public readonly bool isEmpty => this.length == 0;
		public readonly bool any => this.capacity   > this.length;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear() => this.length = 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Enqueue(in T t)
		{
			if (this.isFull) throw new IndexOutOfRangeException("this.isFull");

			Unsafe.AsRef<T>(this.pointer + this.length * Unsafe.SizeOf<T>()) = t;

			this.length += 1;
		}

		/// 返回还没写入的数据
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ReadOnlySpan<T> Enqueue(ReadOnlySpan<T> inBuffer)
		{
			if (this.isFull) throw new IndexOutOfRangeException("this.isFull");

			var emptyLength = this.capacity - this.length;
			var over = inBuffer.Length > emptyLength;

			var src = over ? inBuffer.Slice(0, emptyLength) : inBuffer;
			var dest = new Span<T>(this.pointer + this.length * Unsafe.SizeOf<T>(), this.capacity - this.length);

			src.CopyTo(dest);

			this.length += src.Length;

			return over ? inBuffer.Slice(emptyLength) : default;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe Span<T> Dequeue()
		{
			if (this.isEmpty) throw new IndexOutOfRangeException("this.isEmpty");

			var span = new Span<T>(this.pointer, this.length);

			this.length = 0;

			return span;
		}
	}
}
