using DataAccess.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly IFootballDBContext db;

        public RepositoryManager(IFootballDBContext db)
        {
            this.db = db;
        }
        public IQuestionRepo Question => question ??= new QuestionRepo(db);
        private IQuestionRepo question;

        public IUserRepo User => user ??= new UserRepo(db);
        private IUserRepo user;

        public ILanguageRepo Language => language ??= new LanguageRepo(db);
        private ILanguageRepo language;
    }
}
