using DataAccess.DBContext;
using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class QuestionRepo : IQuestionRepo
    {
        private readonly IFootballDBContext db;

        public QuestionRepo(IFootballDBContext db)
        {
            this.db = db;
        }

        public Task<List<Question>> GetQuestions()
        {
            string collectionType = "questions";
            var questions = db.GetCollection<Question>(collectionType);
            return questions.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Question> CreateQuestion(Question question)
        { 
            var questions = db.GetCollection<Question>("questions");
            await questions.InsertOneAsync(question);
            return question;
        }
    }
}
