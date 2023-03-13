namespace Paradox.SpanExtensions.Tests;

public class AsciiTests
{
    [Theory]
    [ClassData(typeof(AsciiTextGenerator))]
    public void IsAscii(string input)
    {
        Assert.True(AsciiExtensions.IsAscii(input));
    }
}