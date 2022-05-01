using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class Question : BaseEntity
    {
        public List<Answer> Answers { get; set; }
        public string Image { get; set; }

        public List<Description> Description { get; set; }

        public Answer CorrectAnswer { get; set; }
        public ObjectId CorrectAnswerId { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
        public int Impressions { get; set; } = 0;

        public int Answered { get; set; } = 0;

        public ObjectId OwnerId { get; set; }
        public Categories Category { get; set; }
        public Statuses Status { get; set; }

    }
}
