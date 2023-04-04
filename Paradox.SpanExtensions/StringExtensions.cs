namespace Paradox.SpanExtensions;

/// <summary>
/// Provides extension methods for strings and ReadOnlySpans of characters
/// </summary>
public static class StringExtensions
{
    [Pure]
    public static bool IsAsciiLowerLetters(this string str) => IsAsciiLowerLetters((ReadOnlySpan<char>)str);

    [Pure]
    public static bool IsAsciiLowerLetters(this Span<char> chars) => IsAsciiLowerLetters((ReadOnlySpan<char>)chars);

    [Pure]
    public static bool IsAsciiLowerLetters(this ReadOnlySpan<char> str) => CharsAreBetween(str, 'a', 'z');

    [Pure]
    public static bool IsAsciiDigits(this string str) => IsAsciiDigits((ReadOnlySpan<char>)str);

    [Pure]
    public static bool IsAsciiDigits(this Span<char> chars) => IsAsciiDigits((ReadOnlySpan<char>)chars);

    [Pure]
    public static bool IsAsciiDigits(this ReadOnlySpan<char> str) => CharsAreBetween(str, '0', '9');

    [Pure]
    public static bool CharsAreBetween(this string str, char minInclusive, char maxInclusive) => CharsAreBetween((ReadOnlySpan<char>)str, minInclusive, maxInclusive);

    [Pure]
    public static bool CharsAreBetween(this ReadOnlySpan<char> chars, char minInclusive, char maxInclusive)
    {
        if (chars.IsEmpty) return false;

        int i = 0;

        if (!Vector128.IsHardwareAccelerated || chars.Length < Vector128<ushort>.Count)
        {
            goto Scalar;
        }
        else if (!Vector256.IsHardwareAccelerated || chars.Length < Vector256<ushort>.Count)
        {
            ReadOnlySpan<ushort> values = MemoryMarshal.Cast<char, ushort>(chars);

            var min = Vector128.Create((ushort)minInclusive);
            var max = Vector128.Create((ushort)maxInclusive);

            for (; i <= chars.Length - Vector128<ushort>.Count; i += Vector128<ushort>.Count)
            {
                var characters = Vector128.Create(values.Slice(i, Vector128<ushort>.Count));

                if (!(Vector128.GreaterThanOrEqualAll(characters, min) && Vector128.LessThanOrEqualAll(characters, max)))
                {
                    return false;
                }
            }
        }
        else
#if NET8_OR_GREATER
        if (!Vector512.IsHardwareAccelerated || chars.Length < Vector512<ushort>.Count)
#endif
        {
            ReadOnlySpan<ushort> values = MemoryMarshal.Cast<char, ushort>(chars);

            var min = Vector256.Create((ushort)minInclusive);
            var max = Vector256.Create((ushort)maxInclusive);

            for (; i <= chars.Length - Vector256<ushort>.Count; i += Vector256<ushort>.Count)
            {
                var characters = Vector256.Create(values.Slice(i, Vector256<ushort>.Count));

                if (!(Vector256.GreaterThanOrEqualAll(characters, min) && Vector256.LessThanOrEqualAll(characters, max)))
                {
                    return false;
                }
            }
        }
#if NET8_OR_GREATER
        else 
        {
            ReadOnlySpan<ushort> values = MemoryMarshal.Cast<char, ushort>(chars);

            var min = Vector512.Create((ushort)minInclusive);
            var max = Vector512.Create((ushort)maxInclusive);

            for (; i <= chars.Length - Vector512<ushort>.Count; i += Vector512<ushort>.Count)
            {
                var characters = Vector512.Create(values.Slice(i, Vector512<ushort>.Count));

                if (!(Vector512.GreaterThanOrEqualAll(characters, min) && Vector512.LessThanOrEqualAll(characters, max)))
                {
                    return false;
                }
            }
        }
#endif

    Scalar:
        for (; i < chars.Length; i++)
        {
            char character = chars[i];

            if (!(character >= minInclusive && character <= maxInclusive))
            {
                return false;
            }
        }

        return true;
    }
}
