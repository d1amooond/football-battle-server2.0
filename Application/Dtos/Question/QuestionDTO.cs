using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class QuestionDTO
    {
        public Guid Id { get; set; }
        public List<Description> Description { get; set; }
        public List<Answer> Answers { get; set; }
        public string Image { get; set; }
        public Answer CorrectAnswer { get; set; }
        public Guid CorrectAnswerId { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }

        public Guid? OwnerId { get; set; }
        public Statuses Status { get; set; }
        public Categories Category { get; set; }
    }

}
