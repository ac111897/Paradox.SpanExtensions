namespace Paradox.SpanExtensions;

public static class CommonSpanExtensions
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any<T>(this ReadOnlySpan<T> values) => values.Length > 0;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any<T>(this Span<T> values) => values.Length > 0;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T First<T>(this Span<T> values) => First((ReadOnlySpan<T>)values);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T First<T>(this ReadOnlySpan<T> values)
    {
        if (values.IsEmpty)
        {
            ThrowHelper.NoElements();
        }

        return MemoryMarshal.GetReference(values);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Last<T>(this ReadOnlySpan<T> values)
    {
        if (values.IsEmpty)
        {
            ThrowHelper.NoElements();
        }

        return Unsafe.Add(ref MemoryMarshal.GetReference(values), values.Length - 1);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? LastOrDefault<T>(this ReadOnlySpan<T> values)
    {
        return values.IsEmpty ? default : Unsafe.Add(ref MemoryMarshal.GetReference(values), values.Length - 1);
    }
}