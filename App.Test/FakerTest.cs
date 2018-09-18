using System;
using App.Areas.Crm.Models;
using App.Data;
using Newtonsoft.Json;
using Xunit;

namespace App.Test
{
    public class FakerTest
    {
        [Fact]
        public void Test1()
        {
            var data= Faker.FakeContacts();
            var json=JsonConvert.SerializeObject(data);
            Console.WriteLine(json);
        }
    }
}
