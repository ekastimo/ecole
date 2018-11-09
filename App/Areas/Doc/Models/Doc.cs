using System;
using Core.Models;

namespace App.Areas.Doc.Models
{
    public class Doc : ModelBase
    {
        public string Description { get; set; }
        public string OriginalFileName { get; set; }
        public string FileName { get; set; }
        public double SizeInMbs { get; set; }
        public string ContentType { get; set; }
        public bool IsPrimary { get; set; }
        public Guid CreatedBy { get; set; }
    }
}