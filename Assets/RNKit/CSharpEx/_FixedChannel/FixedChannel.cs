using System;
using System.Runtime.CompilerServices;
using System.Threading;
using RN.CSharpEx.FixedCollections;

namespace RN.CSharpEx.FixedChannel
{
	/// <summary>
	/// 专门针对密集运算而优化的Channel
	/// 多路写入 单路读取
	/// 特点:
	///		内存连续,cpu命中率高
	///		自旋锁,解锁时间更敏捷,可以自行定制锁
	///		使用Span<T>, 执行效率更好, 还可以使用LinqFaster,NetFabric.Hyperlinq等高效的linq
	///		gc less
	/// 
	/// 因为是struct,注意不要出现被复制的情况
	///
	/// todo... 记录channel满员的状态,时间,次数...
	/// todo... channel满员时自动多开线程
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TBuffer"></typeparam>
	public partial struct FixedChannel<T, TBuffer>
		where T : unmanaged
		where TBuffer : unmanaged, IFixedBuffer
	{
		internal int length;
		internal TBuffer buffer;
		internal TBuffer tempBuffer;

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


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe void In(in T t)
		{
#if DEBUG
			if (this.length == this.capacity) throw new IndexOutOfRangeException("this.length == this.capacity");
#endif

			Unsafe.AsRef<T>(this.pointer + this.length * Unsafe.SizeOf<T>()) = t;

			//this.length += 1;
			Interlocked.Increment(ref this.length);
		}

		/// 返回还没写入的数据
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe ReadOnlySpan<T> In(ReadOnlySpan<T> inBuffer)
		{
#if DEBUG
			if (this.length == this.capacity) throw new IndexOutOfRangeException("this.length == this.capacity");
#endif
			var emptyLength = this.capacity - this.length;
			var over = inBuffer.Length > emptyLength;

			var src = over ? inBuffer.Slice(0, emptyLength) : inBuffer;
			var dest = new Span<T>(this.pointer + this.length * Unsafe.SizeOf<T>(), this.capacity - this.length);

			//this.length += src.Length;
			Interlocked.Add(ref this.length, src.Length);

			src.CopyTo(dest);


			return over ? inBuffer.Slice(emptyLength) : default;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe Span<T> Out()
		{
#if DEBUG
			if (this.length == 0) throw new IndexOutOfRangeException("this.length == 0");
#endif

			var src = new ReadOnlySpan<T>(this.pointer, this.length);
			var dest = new Span<T>(this.tempBuffer.pointer, this.length);

			src.CopyTo(dest);

			//this.length = 0;
			Interlocked.Exchange(ref this.length, 0);

			return dest;
		}
	}
}
