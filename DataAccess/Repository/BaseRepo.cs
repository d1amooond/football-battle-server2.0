using DataAccess.DBContext;
using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils.Extensions;

namespace DataAccess.Repository
{
    public class BaseRepo : IBaseRepo
    {
        private readonly IFootballDBContext db;
        private readonly string collectionType;

        public BaseRepo(IFootballDBContext db, string collectionType = null)
        {
            this.db = db;
            this.collectionType = collectionType;
        }

        public async Task<T> GetById<T>(ObjectId id, string collectionType = null)
        {
            var collection = this.db.GetCollection<T>(collectionType ?? this.collectionType);
            var entities = await collection.FindAsync<T>(Builders<T>.Filter.Eq("_id", id));
            return entities.FirstOrDefault();
        }

        public async Task<T> Update<T>(BaseEntity entity, string collectionType = null)
        {
            var collection = this.db.GetCollection<T>(collectionType ?? this.collectionType);
            await collection.UpdateOneAsync(Builders<T>.Filter.Eq("_id", entity.Id), new ObjectUpdateDefinition<T>(entity));

            var entities = await collection.FindAsync<T>(Builders<T>.Filter.Eq("_id", entity.Id));
            return entities.FirstOrDefault();
        }

        public async Task<T> InsertInto<T>(T entity, string collectionType = null)
        {
            var collection = this.db.GetCollection<T>(collectionType ?? this.collectionType);
            await collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task DeleteById(ObjectId id, string collectionType = null)
        {
            var collection = this.db.GetCollection<BaseEntity>(collectionType ?? this.collectionType);
            await collection.DeleteOneAsync<BaseEntity>(entity => entity.Id == id);
        }
    }
}
