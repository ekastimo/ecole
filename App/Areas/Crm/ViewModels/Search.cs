using System;
using Core.Models;

namespace App.Areas.Crm.ViewModels
{
    public class SearchRequest
    {
        public Guid? Id { get; set; }
        public Guid? ContactId { get; set; }
    }

    public class ContactSearchRequest : SearchBase
    {
        public Guid? Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ChurchLocation { get; set; }
        public string CellGroup { get; set; }
        public string[] Tags { get; set; }
    }
}