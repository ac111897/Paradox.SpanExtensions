using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Paradox.SpanExtensions;

public static class SpanSumExtensions
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Sum<T>(this ReadOnlySpan<T> values) where T : struct, INumber<T>
    {
        int i = 0;

        T sum = T.Zero;

        if (!Vector128.IsHardwareAccelerated || values.Length < Vector128<T>.Count)
        {
            goto Scalar;
        }
        else if (!Vector256.IsHardwareAccelerated || values.Length < Vector256<T>.Count)
        {
            for (; i <= values.Length - Vector128<T>.Count; i += Vector128<T>.Count)
            {
                sum += Vector128.Sum(Vector128.Create(values.Slice(i, Vector128<T>.Count)));
            }
        }
        else
        {
            for (; i <= values.Length - Vector256<T>.Count; i += Vector256<T>.Count)
            {
                sum += Vector256.Sum(Vector256.Create(values.Slice(i, Vector256<T>.Count)));

            }
        }

        Scalar:
        for (; i < values.Length; i++)
        {
            sum += values[i];
        }

        return sum;
    }
}