using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Requests
{
    public class DraftQuestionRequest
    {
        public string Description { get; set; }
        public List<string> Answers { get; set; }
        public string Image { get; set; }
        public string CorrectAnswer { get; set; }
        public Guid? OwnerId { get; set; }
    }
}
