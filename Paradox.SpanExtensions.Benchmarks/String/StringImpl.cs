using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Paradox.SpanExtensions.Benchmarks;

public static class StringImpl
{
    internal const ushort MaxAsciiValue = (ushort)sbyte.MaxValue;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ScalarAscii(this ReadOnlySpan<char> str)
    {
        if (str.IsEmpty) return false;

        for (int i = 0; i < str.Length; i++)
        {
            if (!char.IsAscii(str[i])) return false;
        }

        return true;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAscii(this ReadOnlySpan<char> str)
    {
        if (str.IsEmpty)
        {
            return false; // the string contains no characters at all so it can't be ASCII
        }
            
        int i = 0;

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
                //Vector128<ushort> characters = Vector128.Create(values.Slice(i, Vector128<ushort>.Count));

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