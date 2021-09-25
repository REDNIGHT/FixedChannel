using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using RN.CSharpEx.FixedCollections;

namespace RN.CSharpEx.FixedChannel.Example
{
	internal class Example
	{
		protected static FixedChannel<char, Int16C4Buffer> channel;
#if true
		private static SpinLock _lock = default;
		private static FixedChannelWriter<char, Int16C4Buffer> writer => channel.Writer(_lock);
		private static FixedChannelReader<char, Int16C4Buffer> reader => channel.Reader(_lock);
#else
		private static readonly object _lock = new object();
		private static FixedChannelWriter2<char, C4Buffer> writer => channel.Writer(_lock);
		private static FixedChannelReader2<char, C4Buffer> reader => channel.Reader(_lock);
#endif

		public static void Main(string[] args)
		{
			var End = char.MinValue;
			Task.WaitAll(Task.Run(() =>
						 {
							 ReadOnlySpan<char> insRO = "hello world!".AsSpan();
							 do
								 insRO = writer.Wait().In(insRO);
							 while (insRO.Length > 0);

							 writer.Wait().In(End);
						 }),
						 Task.Run(() =>
						 {
							 while (true)
							 {
								 foreach (var v in reader.Wait().Out())
								 {
									 if (v == End)
										 return;

									 Console.WriteLine(v);
								 }
							 }
						 }));

		}
	}
}
