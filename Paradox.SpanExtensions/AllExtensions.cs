using System.Numerics;

namespace Paradox.SpanExtensions;

public static partial class AllExtensions
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool All<T>(this Span<T> span, Func<T, bool> predicate) => All((ReadOnlySpan<T>)span, predicate); 

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool All<T>(this ReadOnlySpan<T> span, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        ref T start = ref MemoryMarshal.GetReference(span);

        for (int i = 0; i < span.Length; i++)
        {
            if (!predicate(Unsafe.Add(ref start, i)))
            {
                return false;
            }
        }

        return true;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static bool All<T>(this Span<T> span, delegate*<T, bool> func) => All((ReadOnlySpan<T>)span, func);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static bool All<T>(this ReadOnlySpan<T> span, delegate*<T, bool> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        if (span.IsEmpty) return false;

        ref T start = ref MemoryMarshal.GetReference(span);

        for (int i = 0; i < span.Length; i++)
        {
            if (!func(Unsafe.Add(ref start, i)))
            {
                return false;
            }
        }

        return true;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AllGreaterThan<T>(this Span<T> span, T value) where T : struct, INumber<T>
        => AllGreaterThan((ReadOnlySpan<T>)span, value);

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
                Vector128<T> vector = Vector128.LoadUnsafe(ref Unsafe.Add(ref reference, i));

                if (Vector128.LessThanAny(vector, target))
                {
                    return false;
                }
            }
        }
        else
        {
            Vector256<T> target = Vector256.Create(value);

            for (; i <= span.Length - Vector256<T>.Count; i += Vector256<T>.Count)
            {
                Vector256<T> vector = Vector256.LoadUnsafe(ref Unsafe.Add(ref reference, i));

                if (Vector256.LessThanAny(vector, target))
                {
                    return false;
                }
            }
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