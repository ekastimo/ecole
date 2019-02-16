using System.Collections.Generic;

namespace Core.Extensions
{
    public static class CollectionExtensions
    {
        public static string Stringify(this IEnumerable<string> collection)
        {
            return string.Join(",", collection);
        }
    }
}