using BenchmarkDotNet.Attributes;
using System.Numerics;

#pragma warning disable CA1822 // Mark members as static

namespace Paradox.SpanExtensions.Benchmarks;

[MemoryDiagnoser]
public class FloorBenchmarks
{
    internal static readonly double[] _input = new double[]
    {
        1.4, 1.5, 1.3, 1.8, 1.23, 41421.1, 1.321312, 123123123.11, 123.123123213123, 123312123,111, 123.11,
        1,9, 1.1, 2.5, 5.8, 10.5, 11.5, 12.10789, 142412409234.4234234234234, 1231232131.2131312313, 41234124.11, 4321.111
    };

    [Benchmark]
    public void FloorInPlace()
    {
        Floor.InPlace<double>(_input);
    }

    [Benchmark]
    public void FLoorInPlaceLibrary()
    {
        _input.AsSpan().FloorInPlace();
    }
}

internal static class Floor
{
    public static void InPlace<T>(this Span<T> s) where T : struct, IFloatingPoint<T>
    {
        for (int i = 0; i < s.Length; i++)
        {
            s[i] = T.Floor(s[i]);
        }
    }
}