namespace Paradox.SpanExtensions;

public static class AsciiExtensions
{
    internal const ushort MaxAsciiValue = (ushort)sbyte.MaxValue;

    /// <summary>
    /// Checks if the string only contains ASCII characters ([U+0000...U+007f])
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAscii(this string value) => IsAscii((ReadOnlySpan<char>)value);

    /// <summary>
    /// Checks if the span contains only ASCII characters ([U+0000...U+007f])
    /// </summary>
    /// <param name="chars"></param>
    /// <returns></returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAscii(this Span<char> chars) => IsAscii((ReadOnlySpan<char>)chars);

    /// <summary>
    /// Checks if the readonly span contains only ASCII characters ([U+0000...U+007f])
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAscii(this ReadOnlySpan<char> str)
    {
        if (str.IsEmpty)
        {
            return false; // the string contains no characters at all so it can't be ASCII
        }

        int i = 0;

        // use refs instead of loading data from spans
        ref ushort reference = ref Unsafe.As<char, ushort>(ref MemoryMarshal.GetReference(str));

        if (!Vector128.IsHardwareAccelerated || str.Length < Vector128<ushort>.Count)
        {
            goto Scalar;
        }
        else if (!Vector128.IsHardwareAccelerated || str.Length < Vector256<ushort>.Count)
        {
            Vector128<ushort> max = Vector128.Create(MaxAsciiValue);

            for (; i <= str.Length - Vector128<ushort>.Count; i += Vector128<ushort>.Count)
            {
                Vector128<ushort> characters = Vector128.LoadUnsafe(ref Unsafe.Add(ref reference, i));

                if (Vector128.GreaterThanAny(characters, max))
                {
                    return false;
                }
            }
        }
        else
        {
            Vector256<ushort> max = Vector256.Create(MaxAsciiValue);

            for (; i <= str.Length - Vector256<ushort>.Count; i += Vector256<ushort>.Count)
            {
                Vector256<ushort> characters = Vector256.LoadUnsafe(ref Unsafe.Add(ref reference, i));

                if (Vector256.GreaterThanAny(characters, max))
                {
                    return false;
                }
            }
        }

    Scalar:
        for (; i < str.Length; i++)
        {
            if (Unsafe.Add(ref reference, i) > MaxAsciiValue)
            {
                return false;
            }
        }

        return true;
    }
}