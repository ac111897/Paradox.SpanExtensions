using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Paradox.SpanExtensions;

/// <summary>
/// Provides extension methods for strings and ReadOnlySpans of characters
/// </summary>
public static class StringExtensions
{
    internal const ushort MaxAsciiCharInclusive = '\x007f';

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAscii(this string value) => IsAscii((ReadOnlySpan<char>)value);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAscii(this ReadOnlySpan<char> str)
    {
        int i = 0;

        if (!Vector128.IsHardwareAccelerated || str.Length < Vector128<ushort>.Count)
        {
            goto Scalar;
        }
        else if (!Vector256.IsHardwareAccelerated || str.Length < Vector256<ushort>.Count)
        {
            Vector128<ushort> maxInclusive = Vector128.Create(MaxAsciiCharInclusive);
            ReadOnlySpan<ushort> values = MemoryMarshal.Cast<char, ushort>(str);

            for (; i <= str.Length - Vector128<ushort>.Count; i += Vector128<ushort>.Count)
            {
                var characters = Vector128.Create(values.Slice(i, Vector128<ushort>.Count));

                if (!Vector128.GreaterThanOrEqualAll(characters, maxInclusive))
                {
                    return false;
                }
            }
        }
        else
        {
            Vector256<ushort> maxInclusive = Vector256.Create(MaxAsciiCharInclusive);
            ReadOnlySpan<ushort> values = MemoryMarshal.Cast<char, ushort>(str);

            for (; i <= str.Length - Vector256<ushort>.Count; i += Vector256<ushort>.Count)
            {
                var characters = Vector256.Create(values.Slice(i, Vector256<ushort>.Count));

                if (!Vector256.GreaterThanOrEqualAll(characters, maxInclusive))
                {
                    return false;
                }
            }
        }

    Scalar:
        for (; i < str.Length; i++)
        {
            if (!((uint)str[i] <= MaxAsciiCharInclusive))
            {
                return false;
            }
        }

        return true;
    }

    public static bool CharsAreBetween(this string str, char minInclusive, char maxInclusive) => CharsAreBetween((ReadOnlySpan<char>)str, minInclusive, maxInclusive);

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

        return false;
    }
}
