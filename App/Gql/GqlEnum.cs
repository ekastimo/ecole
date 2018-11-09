using System.Linq;
using GraphQL.Types;

namespace App.Gql
{
    public class GqlEnum<T> : EnumerationGraphType
    {
        public GqlEnum()
        {
            var enumType = typeof(T);
            var data = enumType.GetEnumValues()
                .Cast<object>()
                .ToDictionary(k => k.ToString(), v => (int)v);
            Name = enumType.Name;
            foreach (var value in data)
            {
                AddValue(value.Key, null, value.Value);
            }
        }
    }
}
