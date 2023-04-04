using System.Diagnostics;
using System.Numerics;

namespace Paradox.SpanExtensions;

public static class CeilingExtensions
{
    public static T[] Ceiling<T>(this Span<T> s) where T : struct, IFloatingPoint<T> => Ceiling((ReadOnlySpan<T>)s);
    public static T[] Ceiling<T>(this ReadOnlySpan<T> s) where T : struct, IFloatingPoint<T>
    {
        if (s.IsEmpty)
        {
            return Array.Empty<T>();
        }

        T[] ret = new T[s.Length];

        CeilingInPlace((Span<T>)ret);

        return ret;
    }

    public static void CeilingInPlace<T>(this Span<T> s)
        where T : struct, IFloatingPoint<T>
    {
        if (s.IsEmpty)
        {
            return;
        }

        if (typeof(T) == typeof(double) || typeof(T) == typeof(float)) // if the input is a primitive floating point process it in the fast path
        {
            AcceleratedFloor(s);
        }
        else
        {
            for (int i = 0; i < s.Length; i++)
            {
                ref T reference = ref s[i];

                reference = T.Ceiling(reference);
            }
        }
    }

    internal static void AcceleratedFloor<T>(this Span<T> s)
        where T : struct, IFloatingPoint<T>
    {
        Debug.Assert(typeof(T) == typeof(double) || typeof(T) == typeof(float));

        int i = 0;
        ref T reference = ref MemoryMarshal.GetReference(s);

        if (!Vector128.IsHardwareAccelerated || s.Length < Vector128<T>.Count)
        {
            goto Scalar;
        }
        else if (!Vector256.IsHardwareAccelerated || s.Length < Vector256<T>.Count)
        {
            for (; i <= s.Length - Vector128<T>.Count; i += Vector128<T>.Count)
            {
                Vector128<T> vector = Vector128.LoadUnsafe(ref Unsafe.Add(ref reference, i));

                if (typeof(T) == typeof(double))
                {

                }
                else if (typeof(T) == typeof(float))
                {

                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }
        else
        {
            for (; i <= s.Length - Vector256<T>.Count; i += Vector256<T>.Count)
            {
                if (typeof(T) == typeof(double))
                {

                }
                else if (typeof(T) == typeof(float))
                {

                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

    Scalar:
        for (; i < s.Length; i++)
        {
            ref T current = ref Unsafe.Add(ref reference, i);

            current = T.Ceiling(current);
        }
    }
}