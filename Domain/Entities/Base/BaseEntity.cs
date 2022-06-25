using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BaseEntity
    {
        public ObjectId Id { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }
    }
}
