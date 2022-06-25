using DataAccess.DBContext;
using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class LanguageRepo : BaseRepo, ILanguageRepo
    {
        private readonly IFootballDBContext db;

        private static readonly string collectionType = "languages";

        public LanguageRepo(IFootballDBContext db) : base(db, collectionType)
        {
            this.db = db;
        }

        public Task<List<Language>> GetLanguages()
        {
            var languages = db.GetCollection<Language>(collectionType);
            return languages.Find(new BsonDocument()).ToListAsync();
        }

    }
}
