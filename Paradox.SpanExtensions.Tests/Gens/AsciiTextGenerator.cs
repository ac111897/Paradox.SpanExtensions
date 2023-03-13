using System.Collections;
using System.Text;

namespace Paradox.SpanExtensions.Tests;

public class AsciiTextGenerator : IEnumerable<object[]>
{
    internal const string Ascii = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";

    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "rFbXjhze04d9WcnlaZpKM81o7P5gI6Q2wYu" };
        yield return new object[] { "xkNt9KwiZ36vMnJAPqRSz5p8B7fYDu1jH0glVUo" };
        yield return new object[] { "qN2z0suDVKxfRcIyUwvpiOo7bJhTjX9L6gG4Pl" };
        yield return new object[] { "FyV4kl37gKU6TzwuPNvaQX1IeSsJmHcOxRp0n8b" };
        yield return new object[] { "aHxPpErcb7OsIq3zmG1nDtvSYiR8TL92yUk0X5lZj" };
        yield return new object[] { "eN0CfLxb5TPRK39jJyqYdDmhzZ7plu1kw8gWtaoOv" };
        yield return new object[] { "2cWjoJhYwUp1QaGebf5RZ9XxlMn0zLVTtKukD3vI8q6y" };
        yield return new object[] { "xZmg9dFbOoL78EkJ6eCtW1QjlYwvA4T50G2KynuVqiSs" };
        yield return new object[] { "LaKfWw0iJms3qkEGtDvX9z8u1b2cV7RhyUMx5CjZNe6nB" };
        yield return new object[] { "rV7JWzjctCNGsFif1D89QPn0lKpMbXE6Ue5gYuykS4OAa" };
        yield return new object[] { "g0Qx4GbvW8J2Y1jR6EweoL9afzXkI5K3huTPmNyS7UsH" };
        yield return new object[] { "UdK6EmXvS8fJzG7cHNlpt4BQya2ZgWwO10YkiqxR9j3Vb" };
        yield return new object[] { "q5pC6bTZ80JtyPeWw3LhNgnOoSm9XKv1zDkV7Ex2RfjIsra" };
        yield return new object[] { "GcHbT6RM7rQfjgWVvxLNJZ1F83aIoSPkdzyOw25YtmeKs0u" };
        yield return new object[] { "7iLHneUpqa0sVfZb8Kj3tCJxoYzgX9yv1GmkW2EDMlRwO5Pc" };
        yield return new object[] { "2uzmRlX9oBGb8Wkr3qKdPpIJ5FcaOv1iLwTSU6Nfxtj0yVhE7" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}