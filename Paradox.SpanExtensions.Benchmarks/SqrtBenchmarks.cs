using BenchmarkDotNet.Attributes;
using System.Numerics;

#pragma warning disable CA1822 // Mark members as static

namespace Paradox.SpanExtensions.Benchmarks;

[MemoryDiagnoser]
public class SqrtBenchmarks
{
    internal static readonly double[] _data = new double[]
    {
        4, 16, 25, 36, 49, 64, 81, 100, 121, 144, 4, 16, 25, 36, 49, 64, 81, 100, 121, 144,
        4, 16, 25, 36, 49, 64, 81, 100, 121, 144,  4, 16, 25, 36, 49, 64, 81, 100, 121, 144
    };

    [Benchmark]

    public void SqrtInPlace()
    {
        Sqrt.InPlace<double>(_data);
    }

    [Benchmark]
    public void SqrtInPlaceLibrary()
    {
        _data.AsSpan().SqrtInPlace();
    }
}

internal static class Sqrt
{
    public static void InPlace<T>(Span<T> s) where T : struct, IRootFunctions<T>
    {
        for (int i = 0; i < s.Length; i++)
        {
            s[i] = T.Sqrt(s[i]);
        }
    }
}