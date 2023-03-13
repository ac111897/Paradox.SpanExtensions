using System.Diagnostics.CodeAnalysis;

namespace Paradox.SpanExtensions;

internal static class ThrowHelper
{
    [DoesNotReturn]
    public static void NoElements()
    {
        throw new InvalidOperationException("Span contains no elements");
    }
}