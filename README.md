# FixedChannel

	专门针对密集运算而优化的Channel
	多路写入 单路读取
	特点:
		内存连续,cpu命中率高
		自旋锁,解锁时间更敏捷,可以自行定制锁
		使用Span<T>, 执行效率更好, 还可以使用LinqFaster,NetFabric.Hyperlinq等高效的linq
		gc less
    
    
    
// * Summary *

BenchmarkDotNet=v0.13.1, OS=macOS Big Sur 11.6 (20G165) [Darwin 20.6.0]
Intel Core i9-9880H CPU 2.30GHz, 1 CPU, 16 logical and 8 physical cores
  [Host] : Mono 6.12.0.125 (2020-02/8c552e98bd6 Mon), X64 
  Mono   : Mono 6.12.0.125 (2020-02/8c552e98bd6 Mon), X64 

Job=Mono  Runtime=Mono  

|                     Method | inSleep | outSleep |         Mean |       Error |      StdDev | Rank |  Gen 0 | Allocated |
|--------------------------- |-------- |--------- |-------------:|------------:|------------:|-----:|-------:|----------:|
|   _2_FixedChannel_SpinLock |       0 |        0 |     194.5 us |     1.44 us |     1.35 us |    1 |      - |         - |
|       _2_FixedChannel_Lock |       0 |        0 |  22,001.3 us |    84.85 us |    79.36 us |    4 |      - |         - |
| _2_SystemChannel_Unbounded |       0 |        0 |     577.0 us |     2.18 us |     1.82 us |    2 | 2.9297 |         - |
|   _2_SystemChannel_Bounded |       0 |        0 |   1,175.7 us |    14.68 us |    13.73 us |    3 | 5.8594 |         - |

|   _2_FixedChannel_SpinLock |       0 |        1 | 288,271.9 us |   308.64 us |   288.70 us |    5 |      - |         - |
|       _2_FixedChannel_Lock |       0 |        1 | 290,593.0 us | 2,089.37 us | 1,954.40 us |    5 |      - |         - |
| _2_SystemChannel_Unbounded |       0 |        1 | 304,186.5 us |   820.11 us |   767.13 us |    6 |      - |         - |
|   _2_SystemChannel_Bounded |       0 |        1 | 304,744.6 us | 1,537.57 us | 1,438.24 us |    6 |      - |         - |

|   _2_FixedChannel_SpinLock |       1 |        0 | 288,223.9 us |   525.16 us |   491.24 us |    5 |      - |         - |
|       _2_FixedChannel_Lock |       1 |        0 | 301,892.3 us |   775.19 us |   725.11 us |    6 |      - |         - |
| _2_SystemChannel_Unbounded |       1 |        0 | 303,623.2 us |   518.50 us |   485.01 us |    6 |      - |         - |
|   _2_SystemChannel_Bounded |       1 |        0 | 303,165.8 us | 1,937.80 us | 1,812.62 us |    6 |      - |         - |

|   _2_FixedChannel_SpinLock |       1 |        1 | 290,501.4 us |   232.50 us |   217.48 us |    5 |      - |         - |
|       _2_FixedChannel_Lock |       1 |        1 | 303,851.1 us |   994.79 us |   930.53 us |    6 |      - |         - |
| _2_SystemChannel_Unbounded |       1 |        1 | 302,817.6 us |   574.29 us |   537.20 us |    6 |      - |         - |
|   _2_SystemChannel_Bounded |       1 |        1 | 302,535.3 us |   779.03 us |   728.71 us |    6 |      - |         - |
