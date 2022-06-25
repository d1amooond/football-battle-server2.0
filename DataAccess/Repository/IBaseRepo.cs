using Domain.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IBaseRepo
    {
        Task<T> Update<T>(BaseEntity entity, string collectionType = null);
        Task<T> GetById<T>(ObjectId id, string collectionType = null);
        Task<T> InsertInto<T>(T entity, string collectionType = null);
        Task DeleteById(ObjectId id, string collectionType = null);
    }
}
