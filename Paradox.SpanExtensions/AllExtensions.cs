using System.Numerics;

namespace Paradox.SpanExtensions;

public static class AllExtensions
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AllGreaterThan<T>(this ReadOnlySpan<T> span, T value) where T : struct, INumber<T>
    {
        if (span.IsEmpty)
        {
            return false;
        }

        int i = 0;

        ref T reference = ref MemoryMarshal.GetReference(span);

        if (!Vector128.IsHardwareAccelerated || span.Length < Vector128<T>.Count)
        {
            goto Scalar;
        }
        else if (!Vector256.IsHardwareAccelerated || span.Length < Vector256<T>.Count)
        {
            Vector128<T> target = Vector128.Create(value);

            for (; i <= span.Length - Vector128<T>.Count; i += Vector128<T>.Count)
            {

            }
        }
        else
        {
            Vector256<T> target = Vector256.Create(value);
        }

    Scalar:
        for (; i < span.Length; i++)
        {
            if (span[i] < value)
            {
                return false;
            }
        }

        return true;
    }
}