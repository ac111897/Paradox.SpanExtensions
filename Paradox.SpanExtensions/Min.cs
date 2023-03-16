using System.Numerics;

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
        where T : struct, INumber<T>
    {
        if (values.IsEmpty)
        {
            return default;
        }

        int i = 0;
        T min = values[i];

        if (!Vector128.IsHardwareAccelerated || values.Length < Vector128<T>.Count)
        {
            goto Scalar;
        }
        else if (!Vector128.IsHardwareAccelerated || values.Length < Vector128<T>.Count)
        {
            for (; i <= values.Length - Vector128<T>.Count; i += Vector128<T>.Count)
            {
                
            }
        }
        else
        {

        }

    Scalar:
        for (; i < values.Length; i++)
        {
            if (values[i] < min)
            {
                min = values[i];
            }
        }

        return min;
    }
}