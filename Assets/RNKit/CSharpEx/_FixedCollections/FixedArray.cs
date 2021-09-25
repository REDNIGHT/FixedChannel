using System;
using System.Runtime.CompilerServices;

namespace RN.CSharpEx.FixedCollections
{
	public partial struct FixedArray<T, TBuffer>
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
		public readonly int length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this.buffer.capacity;
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
			get => ref this[0];
		}
		public ref T last
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => ref this[this.length - 1];
		}
		
		
		//
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe Span<T> AsSpan() => new Span<T>(this.pointer, this.length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ReadOnlySpan<T> AsReadOnlySpan() => new ReadOnlySpan<T>(this.pointer, this.length);

		//
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Span<T>(in FixedArray<T, TBuffer> array) => Unsafe.AsRef(array).AsSpan();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ReadOnlySpan<T>(in FixedArray<T, TBuffer> array) => Unsafe.AsRef(array).AsReadOnlySpan();
	}
}
