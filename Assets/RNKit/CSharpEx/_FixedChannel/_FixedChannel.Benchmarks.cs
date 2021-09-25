#if BENCHMARKS
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using RN.CSharpEx.FixedCollections;

namespace RN.CSharpEx.FixedChannel.Benchmarks
{
	[MemoryDiagnoser, RankColumn]
	[SimpleJob(RuntimeMoniker.Mono)]
	//[SimpleJob(RuntimeMoniker.Net48)]
	//[SimpleJob(RuntimeMoniker.NetCoreApp21)]
	public class FixedChannel_Vs_SystemChannel
	{
		private const int End = -1;


		private static void _2_FixedChannel_SpinLock(int inSleep, int outSleep, int count)
		{
			FixedChannel<int, Int32C16Buffer> channelA = default;
			FixedChannel<int, Int32C16Buffer> channelB = default;

			SpinLock _lockA = default;
			var writerA = channelA.Writer(_lockA);
			var readerA = channelA.Reader(_lockA);

			SpinLock _lockB = default;
			var writerB = channelB.Writer(_lockB);
			var readerB = channelB.Reader(_lockB);

			//channelA
			Task.Run(() =>
			{
				for (int c = 0; c < count; c++)
				{
					if (inSleep >= 0) Thread.Sleep(inSleep);

					writerA.Wait().In(c);
				}

				writerA.Wait().In(End);
			});


			//channelB
			Task.Run(() =>
			{
				var run = true;
				while (run)
				{
					var outs = readerA.Wait().Out();

					foreach (var v in outs)
					{
						if (v == End)
						{
							run = false;
							break;
						}
						
						if (outSleep >= 0) Thread.Sleep(outSleep);
					}

					ReadOnlySpan<int> insRO = outs;
					do
						insRO = writerB.Wait().In(insRO);
					while (insRO.Length > 0);
				}
			});


			{
				var run = true;
				while (run)
				{
					var outs = readerB.Wait().Out();

					foreach (var v in outs)
					{
						if (v == End)
						{
							run = false;
							break;
						}
					}
				}
			}
		}

		private static void _2_FixedChannel_Lock(int inSleep, int outSleep, int count)
		{
			object _lockLA = new object();
			object _lockLB = new object();
			FixedChannel<int, Int32C16Buffer> channelLA = default;
			FixedChannel<int, Int32C16Buffer> channelLB = default;

			var writerA = channelLA.Writer(_lockLA);
			var readerA = channelLA.Reader(_lockLA);

			var writerB = channelLB.Writer(_lockLB);
			var readerB = channelLB.Reader(_lockLB);

			//channelA
			Task.Run(() =>
			{
				for (int c = 0; c < count; c++)
				{
					if (inSleep >= 0) Thread.Sleep(inSleep);

					writerA.Wait().In(c);
				}

				writerA.Wait().In(End);
			});


			//channelB
			Task.Run(() =>
			{
				var run = true;
				while (run)
				{
					var outs = readerA.Wait().Out();

					foreach (var v in outs)
					{
						if (v == End)
						{
							run = false;
							break;
						}
						
						if (outSleep >= 0) Thread.Sleep(outSleep);
					}


					ReadOnlySpan<int> insRO = outs;
					do
						insRO = writerB.Wait().In(insRO);
					while (insRO.Length > 0);
				}
			});


			{
				var run = true;
				while (run)
				{
					var outs = readerB.Wait().Out();

					foreach (var v in outs)
					{
						if (v == End)
						{
							run = false;
							break;
						}
					}
				}
			}
		}


		private static void _2_SystemChannel(int inSleep, int outSleep, int count, bool CreateUnbounded)
		{
			Channel<int> channelA = null;
			Channel<int> channelB = null;

			if (CreateUnbounded)
			{
				channelA = Channel.CreateUnbounded<int>();
				channelB = Channel.CreateUnbounded<int>();
			}
			else
			{
				channelA = Channel.CreateBounded<int>(Int32C16Buffer.Capacity);
				channelB = Channel.CreateBounded<int>(Int32C16Buffer.Capacity);
			}


			var writerA = channelA.Writer;
			var readerA = channelA.Reader;

			var writerB = channelB.Writer;
			var readerB = channelB.Reader;

			{
				//channelA
				Task.Run(async () =>
				{
					for (int c = 0; c < count; c++)
					{
						if (inSleep >= 0) Thread.Sleep(inSleep);

						//if (await writerA.WaitToWriteAsync())
						await writerA.WriteAsync(c);
					}

					//if (await writerA.WaitToWriteAsync())
					await writerA.WriteAsync(End);
				});


				//channelB
				Task.Run(async () =>
				{
					var run = true;
					while (run)
					{
						if (await readerA.WaitToReadAsync())
						{
							var v = await readerA.ReadAsync();
							if (v == End)
							{
								run = false;

								//if (await writerB.WaitToWriteAsync())
								await writerB.WriteAsync(v);

								break;
							}

							if (outSleep >= 0) Thread.Sleep(outSleep);

							//if (await writerB.WaitToWriteAsync())
							await writerB.WriteAsync(v);
						}
					}
				});
			}

			Task.Run(async () =>
				 {
					 var run = true;
					 while (run)
					 {
						 //if (await readerB.WaitToReadAsync())
						 {
							 var v = await readerB.ReadAsync();
							 if (v == End)
							 {
								 run = false;
								 break;
							 }
						 }
					 }
				 })
				.Wait();
		}






		//
		private const int Count = 256;
		[Params(0, 1)]
		public int inSleep { get; set; }
		[Params(0, 1)]
		public int outSleep { get; set; }

		[Benchmark]
		public void _2_FixedChannel_SpinLock() => _2_FixedChannel_SpinLock(this.inSleep, this.outSleep, Count);
		[Benchmark]
		public void _2_FixedChannel_Lock() => _2_FixedChannel_Lock(this.inSleep, this.outSleep, Count);
		[Benchmark]
		public void _2_SystemChannel_Unbounded() => _2_SystemChannel(this.inSleep, this.outSleep, Count, true);
		[Benchmark]
		public void _2_SystemChannel_Bounded() => _2_SystemChannel(this.inSleep, this.outSleep, Count, false);
	}

	public class Program
	{
		public static void Main(string[] args)
		{
			var summary = BenchmarkRunner.Run<FixedChannel_Vs_SystemChannel>();
		}
	}
}
#endif
