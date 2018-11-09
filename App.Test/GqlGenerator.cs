using System;
using System.Linq;
using Xunit;

namespace App.Test
{
    public class GqlGenerator
    {

        [Fact]
        public void Genrate()
        {
            var enumType = typeof(Areas.Crm.Enums.AddressCategory);
            var data = enumType.GetEnumValues().Cast<object>().ToDictionary(k => k.ToString(), v => (int)v);
   
            foreach (var value in data)
            {
                Console.WriteLine(value);
            }
        }
    }
}
