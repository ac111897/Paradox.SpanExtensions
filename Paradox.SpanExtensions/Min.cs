namespace Paradox.SpanExtensions;

public static class MinSpanExtensions
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Min<T>(this ReadOnlySpan<T> values)
        where T : struct
    {
        if (values.IsEmpty)
        {
            ThrowHelper.NoElements();
        }

        T min = default;

        // TODO

        return min;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? MinOrDefault<T>(this ReadOnlySpan<T> values)
        where T : struct
    {
        if (values.IsEmpty)
        {
            return default;
        }

        T min = default;

        return min;
    }
}