using BenchmarkDotNet.Running;

namespace Paradox.SpanExtensions.Benchmarks;

public class Program
{
    public static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
}