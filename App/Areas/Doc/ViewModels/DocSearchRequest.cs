using System;
using Core.Models;

namespace App.Areas.Doc.ViewModels
{
    public class DocSearchRequest : SearchBase
    {
        public Guid? Id { get; set; }
        public Guid? ContactId { get; set; }

    }
}
