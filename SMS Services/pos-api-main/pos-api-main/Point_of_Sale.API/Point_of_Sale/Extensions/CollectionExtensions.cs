using System.Collections.Immutable;

namespace Point_of_Sale.Extensions
{
    public static class CollectionExtensions
    {
        public static string Join(this ImmutableArray<string> sentences, string separator)
        {
            return string.Join(separator, sentences);
        }

        public static string JoinNewLine(this ImmutableArray<string> sentences)
        {
            return string.Join(Environment.NewLine, sentences);
        }
    }
}
