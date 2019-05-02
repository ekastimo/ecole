using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using App.Areas.Crm.Services;
using App.Data;
using Xunit;
using Xunit.Abstractions;

namespace App.Test.Crm
{
    public  class PhoneNumberTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public PhoneNumberTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void PhoneNumberMustBeValid()
        {
            var pattern =
                "^\\+?(9[976][0-9]|8[987530][0-9]|6[987][0-9]|5[90][0-9]|42[0-9]|3[875][0-9]|2[98654321][0-9]|9[8543210]|8[6421]|6[6543210]|5[87654321]|4[987654310]|3[9643210]|2[70]|7|1|0)[0-9]{0,14}$";

            Assert.Matches(pattern, "0772120258");
            Assert.Matches(pattern, "256772120258");
            Assert.Matches(pattern, "+256772120258");
        }
    }
}
