#if XUNIT
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using RN.CSharpEx.FixedCollections;
using Xunit;
using Xunit.Abstractions;
using Shouldly;

namespace RN.CSharpEx.FixedChannel.Tests
{
	public class FixedChannelTests
	{
		private static ITestOutputHelper Console;
		public FixedChannelTests(ITestOutputHelper console)
		{
			Console = console;
		}

		const int End = -1;

		private static void _Test((int b, int e) inCount, (int b, int e) inSleep, (int b, int e) outSleep, int count)
		{
			FixedChannel<int, Int32C16Buffer> channel = default;
#if true
			SpinLock _lock = default;
			FixedChannelWriter<int, Int32C16Buffer> writer = channel.Writer(_lock);
			FixedChannelReader<int, Int32C16Buffer> reader = channel.Reader(_lock);
#else
			object _lock = new object();
			FixedChannelWriter2<int, Int32C16Buffer> writer = channel.Writer(_lock);
			FixedChannelReader2<int, Int32C16Buffer> reader = channel.Reader(_lock);
#endif
			count.ShouldBeGreaterThan(inCount.e);

			Task.Run(() =>
			{
				Random r = new Random();
				if (inCount.b == 0 && inCount.e == 0)
				{
					for (int c = 0; c < count; c++)
					{
						if (inSleep.b >= 0 && inSleep.e > 0)
							Thread.Sleep(r.Next(inSleep.b, inSleep.e));

						{
							writer.Wait().In(c);
						}
					}
				}
				else
				{
					Span<int> span = stackalloc int[Int32C16Buffer.Capacity];
					var c = 0;
					while (c < count)
					{
						var rcA = r.Next(inCount.b, inCount.e);
						var rc = rcA;
						if (c + rc > count)
							rc = count - c;

						var ins = span.Slice(0, rc);

						foreach (ref var v in ins)
						{
							v = c;
							if (c >= count)
								break;
							++c;
						}

						Console?.WriteLine($"ins.Length={ins.Length}  ({string.Join(",", ins.ToArray())})");

						ReadOnlySpan<int> insRO = ins;
						do
							insRO = writer.Wait().In(insRO);
						while (insRO.Length > 0);
					}
				}

				writer.Wait().In(End);
			});

			{
				var run = true;
				var i = 0;
				Random r = new Random();
				while (run)
				{
					var outs = reader.Wait().Out();
					Console?.WriteLine($"								"
									 + $"outs.Length={outs.Length}  "
									 + $"({string.Join(",", outs.ToArray())})");
					foreach (var v in outs)
					{
						if (v == End)
						{
							run = false;
							break;
						}

						v.ShouldBe(i);
						++i;
					}

					if (outSleep.b >= 0 && outSleep.e > 0)
						Thread.Sleep(r.Next(inSleep.b, inSleep.e));
				}
			}
		}

		private static void _Test2Channel((int b, int e) inSleep, (int b, int e) outSleep, int Count)
		{
			FixedChannel<int, Int32C16Buffer> channelA = default;
			FixedChannel<int, Int32C16Buffer> channelB = default;
#if true
			SpinLock _lockA = default;
			var writerA = channelA.Writer(_lockA);
			var readerA = channelA.Reader(_lockA);

			SpinLock _lockB = default;
			var writerB = channelB.Writer(_lockB);
			var readerB = channelB.Reader(_lockB);
#else
			var _lockA = new object();
			var writerA = channelA.Writer(_lockA);
			var readerA = channelA.Reader(_lockA);

			var _lockB = new object();
			var writerB = channelB.Writer(_lockB);
			var readerB = channelB.Reader(_lockB);
#endif
			//channelA
			Task.Run(() =>
			{
				Random r = new Random();

				for (int c = 0; c < Count; c++)
				{
					if (inSleep.b >= 0 && inSleep.e > 0)
						Thread.Sleep(r.Next(inSleep.b, inSleep.e));

					{
						writerA.Wait().In(c);
					}
				}

				writerA.Wait().In(End);
			});

			//channelB
			Task.Run(() =>
			{
				var run = true;
				var i = 0;
				Random r = new Random();
				while (run)
				{
					var outs = readerA.Wait().Out();
					Console?.WriteLine($"								"
									 + $"outs.Length={outs.Length}  "
									 + $"({string.Join(",", outs.ToArray())})");


					foreach (var v in outs)
					{
						if (v == End)
						{
							run = false;
							break;
						}

						v.ShouldBe(i);
						++i;
					}


					ReadOnlySpan<int> insRO = outs;
					do
						insRO = writerB.Wait().In(insRO);
					while (insRO.Length > 0);

					if (outSleep.b >= 0 && outSleep.e > 0)
						Thread.Sleep(r.Next(inSleep.b, inSleep.e));
				}
			});



			{
				var run = true;
				var i = 0;
				Random r = new Random();
				while (run)
				{
					var outs = readerB.Wait().Out();
					Console?.WriteLine($"								"
									 + $"outs.Length={outs.Length}  "
									 + $"({string.Join(",", outs.ToArray())})");

					foreach (var v in outs)
					{
						if (v == End)
						{
							run = false;
							break;
						}

						v.ShouldBe(i);
						++i;
					}

					if (outSleep.b >= 0 && outSleep.e > 0)
						Thread.Sleep(r.Next(inSleep.b, inSleep.e));
				}
			}
		}


		[Theory]
		[InlineData(0, 0, 0, 0, 0, 0, 1024)]
		[InlineData(0, 0, 0, 1, 0, 1, 1024)]
		[InlineData(0, 0, 0, 2, 0, 1, 1024)]
		[InlineData(0, 0, 0, 3, 0, 1, 1024)]
		[InlineData(0, 0, 0, 1, 0, 2, 1024)]
		[InlineData(0, 0, 0, 2, 0, 2, 1024)]
		[InlineData(0, 0, 0, 3, 0, 2, 1024)]
		public void Test_A(int icb, int ice, int ib, int ie, int ob, int oe, int c)
			=> _Test((icb, ice), (ib, ie), (ob, oe), c);

		[Theory]
		[InlineData(1, Int32C16Buffer.Capacity, 0, 0, 0, 0, 1024)]
		[InlineData(1, Int32C16Buffer.Capacity, 0, 1, 0, 1, 1024)]
		[InlineData(1, Int32C16Buffer.Capacity, 0, 2, 0, 1, 1024)]
		[InlineData(1, Int32C16Buffer.Capacity, 0, 3, 0, 1, 1024)]
		[InlineData(1, Int32C16Buffer.Capacity, 0, 1, 0, 2, 1024)]
		[InlineData(1, Int32C16Buffer.Capacity, 0, 2, 0, 2, 1024)]
		[InlineData(1, Int32C16Buffer.Capacity, 0, 3, 0, 2, 1024)]
		public void Test_B(int icb, int ice, int ib, int ie, int ob, int oe, int c)
			=> _Test((icb, ice), (ib, ie), (ob, oe), c);

		[Theory]
		[InlineData(0, 0, 0, 0, 1024)]
		[InlineData(0, 1, 0, 1, 1024)]
		[InlineData(0, 2, 0, 1, 1024)]
		[InlineData(0, 3, 0, 1, 1024)]
		[InlineData(0, 1, 0, 2, 1024)]
		[InlineData(0, 2, 0, 2, 1024)]
		[InlineData(0, 3, 0, 2, 1024)]
		public void Test2Channel(int ib, int ie, int ob, int oe, int c)
			=> _Test2Channel((ib, ie), (ob, oe), c);

	}
}
#endif
