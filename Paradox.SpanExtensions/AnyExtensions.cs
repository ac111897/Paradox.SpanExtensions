namespace Paradox.SpanExtensions;

public static class AnyExtensions
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static bool Any<T>(this Span<T> span, delegate*<T, bool> predicate) => Any((ReadOnlySpan<T>)span, predicate);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static bool Any<T>(this ReadOnlySpan<T> span, delegate*<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (span.IsEmpty) return false;

        ref T start = ref MemoryMarshal.GetReference(span);

        for (int i = 0; i < span.Length; i++)
        {
            if (predicate(Unsafe.Add(ref start, i)))
            {
                return true;
            }
        }

        return false;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any<T>(this Span<T> s, Func<T, bool> predicate) => Any((ReadOnlySpan<T>)s, predicate);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any<T>(this ReadOnlySpan<T> span, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (span.IsEmpty) return false;

        ref T start = ref MemoryMarshal.GetReference(span);

        for (int i = 0; i < span.Length; i++)
        {
            if (predicate(Unsafe.Add(ref start, i)))
            {
                return true;
            }
        }

        return false;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any<T>(this ReadOnlySpan<T> values) => values.Length > 0;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any<T>(this Span<T> values) => values.Length > 0;
}