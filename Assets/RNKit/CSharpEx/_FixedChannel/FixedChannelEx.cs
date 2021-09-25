using System;
using System.Runtime.CompilerServices;
using System.Threading;
using RN.CSharpEx.FixedCollections;

namespace RN.CSharpEx.FixedChannel
{
	public static partial class FixedChannelEx
	{
		//
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FixedChannelWriter<T, TBuffer> Writer<T, TBuffer>(this ref FixedChannel<T, TBuffer> channel, in SpinLock l)
			where T : unmanaged
			where TBuffer : unmanaged, IFixedBuffer
			=> new FixedChannelWriter<T, TBuffer>(ref channel, l);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FixedChannelReader<T, TBuffer> Reader<T, TBuffer>(this ref FixedChannel<T, TBuffer> channel, in SpinLock l)
			where T : unmanaged
			where TBuffer : unmanaged, IFixedBuffer
			=> new FixedChannelReader<T, TBuffer>(ref channel, l);

		//
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FixedChannelWriter2<T, TBuffer> Writer<T, TBuffer>(this ref FixedChannel<T, TBuffer> channel, object mutex)
			where T : unmanaged
			where TBuffer : unmanaged, IFixedBuffer
			=> new FixedChannelWriter2<T, TBuffer>(ref channel, mutex);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FixedChannelReader2<T, TBuffer> Reader<T, TBuffer>(this ref FixedChannel<T, TBuffer> channel, object mutex)
			where T : unmanaged
			where TBuffer : unmanaged, IFixedBuffer
			=> new FixedChannelReader2<T, TBuffer>(ref channel, mutex);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void Wait(ESleep sleep)
		{
			if (sleep == ESleep.Yield)
				Thread.Yield();
			else
				Thread.Sleep((int)sleep);

			//SpinWait.SpinUntil(() => this.length < this.capacity);
			//if (this.length == this.capacity) this.spinWait.SpinOnce();
		}
	}


	public enum ESleep
	{
		Yield = -1,
		_0 = 0,
		_1,
		_2,
	}
	public readonly unsafe partial struct FixedChannelWriter<T, TBuffer>
		where T : unmanaged
		where TBuffer : unmanaged, IFixedBuffer
	{
		internal readonly FixedChannel<T, TBuffer>* channel;
		internal readonly SpinLock* spinlock;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal FixedChannelWriter(ref FixedChannel<T, TBuffer> c, in SpinLock l)
		{
			this.channel = (FixedChannel<T, TBuffer>*)Unsafe.AsPointer(ref c);
			this.spinlock = (SpinLock*)Unsafe.AsPointer(ref Unsafe.AsRef(l));
		}

		public int capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this.channel->capacity;
		}
		public int length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this.channel->length;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public FixedChannelWriter<T, TBuffer> Wait(ESleep sleep = ESleep.Yield)
		{
#if DEBUG
			if (this.length > this.capacity) throw new IndexOutOfRangeException("this.length > this.capacity");
#endif
			while (this.length == this.capacity) FixedChannelEx.Wait(sleep);

			return this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void In(in T t)
		{
			bool lockTaken = false;

			this.spinlock->Enter(ref lockTaken);

			this.channel->In(t);

			if (lockTaken) this.spinlock->Exit(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan<T> In(ReadOnlySpan<T> buffer)
		{
			bool lockTaken = false;

			this.spinlock->Enter(ref lockTaken);

			var o = this.channel->In(buffer);

			if (lockTaken) this.spinlock->Exit(false);

			return o;
		}
	}

	public readonly unsafe partial struct FixedChannelReader<T, TBuffer>
		where T : unmanaged
		where TBuffer : unmanaged, IFixedBuffer
	{
		internal readonly FixedChannel<T, TBuffer>* channel;
		internal readonly SpinLock* spinlock;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal FixedChannelReader(ref FixedChannel<T, TBuffer> c, in SpinLock l)
		{
			this.channel = (FixedChannel<T, TBuffer>*)Unsafe.AsPointer(ref c);
			this.spinlock = (SpinLock*)Unsafe.AsPointer(ref Unsafe.AsRef(l));
		}

		public int capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this.channel->capacity;
		}
		public int length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this.channel->length;
		}
		public bool isFull
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
#if DEBUG
				if (this.length > this.capacity) throw new IndexOutOfRangeException("this.length > this.capacity");
#endif
				return this.length == this.capacity;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public FixedChannelReader<T, TBuffer> Wait(ESleep sleep = ESleep._0)
		{
			//一般等待数据的时间都是比较长的 所以这里用Thread.Sleep(1)
			while (this.length == 0) FixedChannelEx.Wait(sleep);
			return this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> Out()
		{
			bool lockTaken = false;

			this.spinlock->Enter(ref lockTaken);

			var o = this.channel->Out();

			if (lockTaken) this.spinlock->Exit(false);

			return o;
		}
	}

	public readonly unsafe struct FixedChannelWriter2<T, TBuffer>
		where T : unmanaged
		where TBuffer : unmanaged, IFixedBuffer
	{
		internal readonly FixedChannel<T, TBuffer>* channel;
		internal readonly object mutex;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal FixedChannelWriter2(ref FixedChannel<T, TBuffer> c, object m)
		{
			this.channel = (FixedChannel<T, TBuffer>*)Unsafe.AsPointer(ref c);
			this.mutex = m;
		}

		public int capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this.channel->capacity;
		}
		public int length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this.channel->length;
		}
		public bool isFull
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
#if DEBUG
				if (this.length > this.capacity) throw new IndexOutOfRangeException("this.length > this.capacity");
#endif
				return this.length == this.capacity;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public FixedChannelWriter2<T, TBuffer> Wait(ESleep sleep = ESleep._0)
		{
#if DEBUG
			if (this.length > this.capacity) throw new IndexOutOfRangeException("this.length > this.capacity");
#endif
			while (this.length == this.capacity) FixedChannelEx.Wait(sleep);
			return this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void In(in T t)
		{
			Monitor.Enter(this.mutex);

			this.channel->In(t);

			Monitor.Exit(this.mutex);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan<T> In(ReadOnlySpan<T> buffer)
		{
			Monitor.Enter(this.mutex);

			var o = this.channel->In(buffer);

			Monitor.Exit(this.mutex);

			return o;
		}
	}

	public readonly unsafe struct FixedChannelReader2<T, TBuffer>
		where T : unmanaged
		where TBuffer : unmanaged, IFixedBuffer
	{
		internal readonly FixedChannel<T, TBuffer>* channel;
		internal readonly object mutex;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal FixedChannelReader2(ref FixedChannel<T, TBuffer> c, object m)
		{
			this.channel = (FixedChannel<T, TBuffer>*)Unsafe.AsPointer(ref c);
			this.mutex = m;
		}

		public int capacity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this.channel->capacity;
		}
		public int length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this.channel->length;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public FixedChannelReader2<T, TBuffer> Wait(ESleep sleep = ESleep._1)
		{
			//一般等待数据的时间都是比较长的 所以这里用Thread.Sleep(1)
			while (this.length == 0) FixedChannelEx.Wait(sleep);
			return this;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> Out()
		{
			Monitor.Enter(this.mutex);

			var o = this.channel->Out();

			Monitor.Exit(this.mutex);

			return o;
		}
	}
}
