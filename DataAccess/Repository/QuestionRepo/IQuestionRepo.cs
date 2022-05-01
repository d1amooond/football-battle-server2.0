using Domain.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IQuestionRepo
    {
        Task<List<Question>> GetQuestions();

        Task<Question> CreateQuestion(Question question);

        // Question GetQuestionById();
    }
}
