using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Answer
    {
        [BsonId]
        public ObjectId? Id { get; set; }
        public List<AnswerVariant> Variants { get; set; }
    }
}
