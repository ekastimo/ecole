using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class ModelBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }

        public bool IsDeleted { get; set; }

        public ModelBase()
        {
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }
    }
}
