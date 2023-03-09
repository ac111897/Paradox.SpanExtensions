using System.Runtime.Intrinsics;

namespace Paradox.SpanExtensions;

public static class MinSpanExtensions
{
    public static T? Min<T>(ReadOnlySpan<T> values)
        where T : struct
    {
        if (values.IsEmpty)
        {
            return default;
        }

        T min = default;

        // TODO

        return min;
    }
}