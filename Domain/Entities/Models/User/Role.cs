using Domain.Enums;
using MongoDB.Bson;

namespace Domain.Entities
{
    public class Role : BaseEntity
    {
        public ObjectId UserId;
        public Roles Type { get; set; }
    }
}
