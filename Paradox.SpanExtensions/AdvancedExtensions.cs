using System.Diagnostics;
using System.Numerics;

namespace Paradox.SpanExtensions;

public static class AdvancedExtensions
{
    public static T[] Xor<T>(this Span<T> values, ReadOnlySpan<T> other)  where T : struct, IBitwiseOperators<T, T, T> => Xor((ReadOnlySpan<T>)values, other);
    public static T[] Xor<T>(this ReadOnlySpan<T> values, ReadOnlySpan<T> other)
        where T : struct, IBitwiseOperators<T, T, T>
    {
        if (values.IsEmpty)
        {
            return Array.Empty<T>();
        }

        T[] buffer = new T[values.Length];

        XorInPlace((Span<T>)buffer, other);

        return buffer;
    }

    /// <summary>
    /// Provides an inplace version of <see cref="Xor{T}(Span{T}, ReadOnlySpan{T})"/> where the xor performs on the memory of the first buffer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values"></param>
    /// <param name="s"></param>
    public static void XorInPlace<T>(Span<T> values, ReadOnlySpan<T> s)
        where T : struct, IBitwiseOperators<T, T, T>
    {
        if (values.IsEmpty) return;

        int i = 0;

        ref T buffer = ref MemoryMarshal.GetReference(values);
        ref T rightSource = ref MemoryMarshal.GetReference(s);

        if (!Vector128.IsHardwareAccelerated || values.Length < Vector128<T>.Count)
        {
            goto Scalar;
        }
        else if (!Vector256.IsHardwareAccelerated || values.Length < Vector256<T>.Count)
        {
            for (; i <= values.Length - Vector128<T>.Count; i += Vector128<T>.Count)
            {
                ref T dest = ref Unsafe.Add(ref buffer, i);

                Vector128<T> left = Vector128.LoadUnsafe(ref dest);
                Vector128<T> right = Vector128.LoadUnsafe(ref Unsafe.Add(ref rightSource, i));

                (left ^ right).StoreUnsafe(ref dest);
            }

        }
        else
        {
            for (; i <= values.Length - Vector256<T>.Count; i += Vector256<T>.Count)
            {
                ref T dest = ref Unsafe.Add(ref buffer, i);

                Vector256<T> left = Vector256.LoadUnsafe(ref dest);
                Vector256<T> right = Vector256.LoadUnsafe(ref Unsafe.Add(ref rightSource, i));

                (left ^ right).StoreUnsafe(ref dest);
            }
        }

    Scalar:
        for (; i < values.Length; i++)
        {
            Unsafe.Add(ref buffer, i) ^= Unsafe.Add(ref rightSource, i);
        }

        return;
    }


    /// <summary>
    /// Floors a Span's elements and returns a new array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="s"></param>
    /// <returns></returns>
    public static T[] Floor<T>(this ReadOnlySpan<T> s)
        where T : struct, IFloatingPoint<T>
    {
        T[] ret = new T[s.Length];

        FloorInPlace((Span<T>)ret);

        return ret;
    }

    public static void FloorInPlace<T>(this Span<T> s)
        where T : struct, IFloatingPoint<T>
    {
        if (typeof(T) == typeof(double) || typeof(T) == typeof(float))
        {
            AcceleratedFloor(s);
        }
        else
        {
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = T.Floor(s[i]);
            }
        }
    }

    internal static void AcceleratedFloor<T>(Span<T> s) where T : struct, IFloatingPoint<T>
    {
        Debug.Assert(typeof(T) == typeof(float) || typeof(T) == typeof(double));

        if (s.IsEmpty)
        {
            return;
        }

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
                ref T current = ref Unsafe.Add(ref reference, i);

                Vector128<T> vector = Vector128.LoadUnsafe(ref current);

                if (typeof(T) == typeof(double))
                {
                    Vector128.Floor(vector.As<T, double>()).As<double, T>().StoreUnsafe(ref current);
                }
                else if (typeof(T) == typeof(float))
                {
                    Vector128.Floor(vector.As<T, float>()).As<float, T>().StoreUnsafe(ref current);
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
                ref T current = ref Unsafe.Add(ref reference, i);

                Vector256<T> vector = Vector256.LoadUnsafe(ref current);

                if (typeof(T) == typeof(double))
                {
                    Vector256.Floor(vector.As<T, double>()).As<double, T>().StoreUnsafe(ref current);
                }
                else if (typeof(T) == typeof(float))
                {
                    Vector256.Floor(vector.As<T, float>()).As<float, T>().StoreUnsafe(ref current);
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

            current = T.Floor(current);
        }
    }
}