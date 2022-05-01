using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.DBContext
{
    public interface IFootballDBContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
