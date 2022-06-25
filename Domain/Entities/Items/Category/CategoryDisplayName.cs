using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CategoryDisplayName
    {
        public ObjectId LanguageId { get; set; }
        public string LanguageShortCode { get; set; }
        public string Name { get; set; }
    }
}
