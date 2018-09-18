using System;

namespace Core.Models
{
    public class ViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public Guid ReferenceId { get; set; }
    }
}