using System;

namespace Core.Models
{
    public class ViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }
        public Guid ReferenceId { get; set; }
    }

    public class ViewModelCustomId
    {   
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }
        public Guid ReferenceId { get; set; }
    }

    public class MiniViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}