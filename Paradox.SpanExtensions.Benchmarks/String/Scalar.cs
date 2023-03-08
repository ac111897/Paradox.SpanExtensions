namespace Paradox.SpanExtensions.Benchmarks;

public static class Scalar
{
    public static bool IsAscii(string input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (!char.IsAscii(input[i]))
            {
                return false;
            }
        }

        return true;
    }
}