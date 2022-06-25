using DataAccess.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class CategoryRepo : BaseRepo, ICategoryRepo
    {
        private readonly IFootballDBContext db;

        private static readonly string collectionType = "categories";

        public CategoryRepo(IFootballDBContext db) : base(db, collectionType)
        {
            this.db = db;
        }
    }
}
