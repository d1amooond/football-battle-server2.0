using Domain.Enums;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Profile : BaseEntity
    {
        public ObjectId UserId { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public Countries Country { get; set; }
    }
}
