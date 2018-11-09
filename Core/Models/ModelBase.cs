using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Core.Models
{
    public class ModelBase
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
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
