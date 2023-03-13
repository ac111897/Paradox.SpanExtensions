using BenchmarkDotNet.Attributes;

namespace Paradox.SpanExtensions.Benchmarks;

[MemoryDiagnoser]
public class Ascii
{
    [Params("abcdefghijklmnopqrstuvwxyz", 
        "abcdefghijklmnopqrstuvwxyz1234567890012345678901234567890234567890")]

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string Input { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [Benchmark(Baseline = true)]
    public bool Vectorized()
    {
        return AsciiExtensions.IsAscii(Input);
    }

    [Benchmark]
    public bool Scalar()
    {
        return StringImpl.ScalarAscii(Input);
    }

    [Benchmark]
    public bool Vectorized_Ref()
    {
        return StringImpl.IsAscii(Input);
    }
}