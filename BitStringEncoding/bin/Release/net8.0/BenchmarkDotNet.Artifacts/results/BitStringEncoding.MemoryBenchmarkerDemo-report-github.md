```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22621.3447/22H2/2022Update/SunValley2)
11th Gen Intel Core i5-1135G7 2.40GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method       | Mean | Error |
|------------- |-----:|------:|
| AddByteOne   |   NA |    NA |
| AddByteTwo   |   NA |    NA |
| AddByteThree |   NA |    NA |

Benchmarks with issues:
  MemoryBenchmarkerDemo.AddByteOne: DefaultJob
  MemoryBenchmarkerDemo.AddByteTwo: DefaultJob
  MemoryBenchmarkerDemo.AddByteThree: DefaultJob
