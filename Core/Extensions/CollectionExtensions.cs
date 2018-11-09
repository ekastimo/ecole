using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

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