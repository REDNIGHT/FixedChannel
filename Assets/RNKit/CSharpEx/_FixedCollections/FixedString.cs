using System;
using System.Runtime.CompilerServices;

namespace RN.CSharpEx.FixedCollections
{
	public partial struct FixedString<TBuffer>
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear() => this.length = 0;


		//
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public new unsafe string ToString()
			=> new string(Unsafe.AsRef<char>(this.pointer), this.length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FromString(string str)
		{
			if (str.Length > this.capacity)
				throw new IndexOutOfRangeException("str.Length > this.capacity");

			str.AsSpan().CopyTo(this.AsSpan());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public FixedString(string str) : this()
			=> this.FromString(str);

		//
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals<RBuffer>(FixedString<RBuffer> other)
			where RBuffer : unmanaged, IFixedBuffer
			=> this.AsSpan() == other.AsSpan();


		//
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe Span<char> AsSpan() => new Span<char>(this.pointer, this.length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe ReadOnlySpan<char> AsReadOnlySpan() => new ReadOnlySpan<char>(this.pointer, this.length);


		//
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Span<char>(in FixedString<TBuffer> list) => Unsafe.AsRef(list).AsSpan();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ReadOnlySpan<char>(in FixedString<TBuffer> list) => Unsafe.AsRef(list).AsReadOnlySpan();
	}

}
