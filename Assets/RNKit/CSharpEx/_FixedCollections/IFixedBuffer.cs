using System;
using System.Runtime.CompilerServices;
namespace RN.CSharpEx.FixedCollections
{
	public partial interface IFixedBuffer
	{
		int capacity { get; }
		unsafe byte* pointer { get; }
	}

#region ByteCXBuffer
	public struct ByteC4Buffer : IFixedBuffer
	{
		public const int Capacity = 4;
		private unsafe fixed byte _bytes[Capacity * sizeof(byte)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct ByteC8Buffer : IFixedBuffer
	{
		public const int Capacity = 8;
		private unsafe fixed byte _bytes[Capacity * sizeof(byte)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct ByteC16Buffer : IFixedBuffer
	{
		public const int Capacity = 16;
		private unsafe fixed byte _bytes[Capacity * sizeof(byte)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct ByteC32Buffer : IFixedBuffer
	{
		public const int Capacity = 32;
		private unsafe fixed byte _bytes[Capacity * sizeof(byte)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct ByteC64Buffer : IFixedBuffer
	{
		public const int Capacity = 64;
		private unsafe fixed byte _bytes[Capacity * sizeof(byte)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct ByteC128Buffer : IFixedBuffer
	{
		public const int Capacity = 128;
		private unsafe fixed byte _bytes[Capacity * sizeof(byte)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct ByteC256Buffer : IFixedBuffer
	{
		public const int Capacity = 256;
		private unsafe fixed byte _bytes[Capacity * sizeof(byte)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct ByteC512Buffer : IFixedBuffer
	{
		public const int Capacity = 512;
		private unsafe fixed byte _bytes[Capacity * sizeof(byte)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct ByteC1024Buffer : IFixedBuffer
	{
		public const int Capacity = 1024;
		private unsafe fixed byte _bytes[Capacity * sizeof(byte)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct ByteC2048Buffer : IFixedBuffer
	{
		public const int Capacity = 2048;
		private unsafe fixed byte _bytes[Capacity * sizeof(byte)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
#endregion
	
#region Int16CXBuffer
	public struct Int16C4Buffer : IFixedBuffer
	{
		public const int Capacity = 4;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int16)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int16C8Buffer : IFixedBuffer
	{
		public const int Capacity = 8;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int16)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int16C16Buffer : IFixedBuffer
	{
		public const int Capacity = 16;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int16)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int16C32Buffer : IFixedBuffer
	{
		public const int Capacity = 32;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int16)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int16C64Buffer : IFixedBuffer
	{
		public const int Capacity = 64;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int16)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int16C128Buffer : IFixedBuffer
	{
		public const int Capacity = 128;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int16)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int16C256Buffer : IFixedBuffer
	{
		public const int Capacity = 256;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int16)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int16C512Buffer : IFixedBuffer
	{
		public const int Capacity = 512;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int16)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int16C1024Buffer : IFixedBuffer
	{
		public const int Capacity = 1024;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int16)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
#endregion

#region Int32CXBuffer
	public struct Int32C4Buffer : IFixedBuffer
	{
		public const int Capacity = 4;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int32)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int32C8Buffer : IFixedBuffer
	{
		public const int Capacity = 8;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int32)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int32C16Buffer : IFixedBuffer
	{
		public const int Capacity = 16;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int32)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int32C32Buffer : IFixedBuffer
	{
		public const int Capacity = 32;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int32)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int32C64Buffer : IFixedBuffer
	{
		public const int Capacity = 64;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int32)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int32C128Buffer : IFixedBuffer
	{
		public const int Capacity = 128;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int32)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int32C256Buffer : IFixedBuffer
	{
		public const int Capacity = 256;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int32)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int32C512Buffer : IFixedBuffer
	{
		public const int Capacity = 512;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int32)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int32C1024Buffer : IFixedBuffer
	{
		public const int Capacity = 1024;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int32)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
#endregion
	
#region Int64CXBuffer
	public struct Int64C4Buffer : IFixedBuffer
	{
		public const int Capacity = 4;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int64)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int64C8Buffer : IFixedBuffer
	{
		public const int Capacity = 8;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int64)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int64C16Buffer : IFixedBuffer
	{
		public const int Capacity = 16;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int64)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int64C32Buffer : IFixedBuffer
	{
		public const int Capacity = 32;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int64)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int64C64Buffer : IFixedBuffer
	{
		public const int Capacity = 64;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int64)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int64C128Buffer : IFixedBuffer
	{
		public const int Capacity = 128;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int64)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int64C256Buffer : IFixedBuffer
	{
		public const int Capacity = 256;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int64)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
	public struct Int64C512Buffer : IFixedBuffer
	{
		public const int Capacity = 512;
		private unsafe fixed byte _bytes[Capacity * sizeof(Int64)];

		public unsafe byte* pointer => (byte*)Unsafe.AsPointer(ref this);
		public int capacity => Capacity;
	}
#endregion
}
