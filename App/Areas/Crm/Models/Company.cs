using App.Areas.Crm.Enums;

namespace App.Areas.Crm.Models
{
    public class Company
    {
        public string Name { get; set; }
        public CompanyCategory Category { get; set; }
    }
}