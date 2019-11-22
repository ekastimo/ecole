using System;
using Core.Models;

namespace App.Areas.Tags
{
    public class Tag: ModelBase
    {
        public TagCategory Category { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public Guid CreatedBy { get; set; }
    }

    public class TagSearchRequest : SearchBase
    {
        public TagCategory[] Categories { get; set; }
    }
}
