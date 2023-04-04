using System.Numerics;

namespace Paradox.SpanExtensions;

public static class SqrtExtensions
{
    public static T[] Sqrt<T>(this Span<T> s) where T : struct, IRootFunctions<T> => Sqrt((ReadOnlySpan<T>)s);
    public static T[] Sqrt<T>(this ReadOnlySpan<T> s) where T : struct, IRootFunctions<T>
    {
        T[] buffer = new T[s.Length];

        SqrtInPlace((Span<T>)buffer);

        return buffer;
    }

    /// <summary>
    /// Replaces every element with the corresponding square roots of the original elements
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="s"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SqrtInPlace<T>(this Span<T> s) where T : struct, IRootFunctions<T>
    {
        if (s.IsEmpty) return;

        int i = 0;
        ref T reference = ref MemoryMarshal.GetReference(s);

        if (!Vector128.IsHardwareAccelerated || s.Length < Vector128<T>.Count || Vector128<T>.IsSupported) // this guards against types that arent supported
        {
            goto Scalar;
        }
        else if (!Vector256.IsHardwareAccelerated || s.Length < Vector256<T>.Count)
        {
            for (; i <= s.Length - Vector128<T>.Count; i += Vector128<T>.Count)
            {
                ref T ptr = ref Unsafe.Add(ref reference, i);

                Vector128.Sqrt(Vector128.LoadUnsafe(ref ptr)).StoreUnsafe(ref ptr);
            }
        }
        else
        {
            for (; i <= s.Length - Vector256<T>.Count; i += Vector256<T>.Count)
            {
                ref T ptr = ref Unsafe.Add(ref reference, i);

                Vector256.Sqrt(Vector256.LoadUnsafe(ref ptr)).StoreUnsafe(ref ptr);
            }
        }

    Scalar:
        for (; i < s.Length; i++)
        {
            ref T value = ref Unsafe.Add(ref reference, i);

            value = T.Sqrt(value);
        }
    }
}