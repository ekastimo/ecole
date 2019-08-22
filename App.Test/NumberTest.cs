using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace App.Test
{
    public class NumberTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public NumberTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, "^\\+?(9[976][0-9]|8[987530][0-9]|6[987][0-9]|5[90][0-9]|42[0-9]|3[875][0-9]|2[98654321][0-9]|9[8543210]|8[6421]|6[6543210]|5[87654321]|4[987654310]|3[9643210]|2[70]|7|1|0)[0-9]{0,14}$");
        }

    }

}