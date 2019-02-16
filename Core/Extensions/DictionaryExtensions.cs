using System.Collections.Generic;

namespace Core.Extensions
{
    public static class DictionaryExtensions
    {

        public static V SafeGet<K, V>(this IDictionary<K, V> data, K key) 
        {
            if (data.TryGetValue(key, out var value))
            {
                return value;
            }
            return default(V);
        }
    }
}
