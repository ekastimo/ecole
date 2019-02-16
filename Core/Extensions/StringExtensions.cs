using System.Linq;

namespace Core.Extensions
{
    public static class StringExtensions
    {

        public static string ToPascalCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            var converted = str.Split(" ").Select(ToPascal);
            return string.Join(' ', converted);
        }

        private static string ToPascal(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            var a = str.ToCharArray();
            a[0] = char.ToLower(a[0]);

            return new string(a);
        }
    }
}
