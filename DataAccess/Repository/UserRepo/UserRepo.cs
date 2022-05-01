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
    public class UserRepo : IUserRepo
    {
        private readonly IFootballDBContext db;

        private readonly string collectionType = "users";

        public UserRepo(IFootballDBContext db)
        {
            this.db = db;
        }

        public Task<List<User>> GetUsers()
        {
            var userCollection = db.GetCollection<User>(collectionType);
            return userCollection.Find(new BsonDocument()).ToListAsync();
        }

        public Task RevokeRefreshToken(RefreshToken refreshToken)
        {
            var tokenCollection = db.GetCollection<RefreshToken>("refresh_tokens");
            return tokenCollection.ReplaceOneAsync<RefreshToken>(t => t.Id == refreshToken.Id, refreshToken);
        }

        public Task CreateRefreshToken(RefreshToken token)
        {
            var tokenCollection = db.GetCollection<RefreshToken>("refresh_tokens");
            return tokenCollection.InsertOneAsync(token);
        }

        public async Task<RefreshToken> GetRefreshTokenByUsername(string username)
        {
            var tokenCollection = db.GetCollection<RefreshToken>("refresh_tokens");
            var refreshToken = await tokenCollection.FindAsync(t => t.IsActive && t.Username == username);
            return refreshToken.FirstOrDefault();
        }

        public async Task<User> GetUserById(Guid id)
        {
            var userCollection = db.GetCollection<User>(collectionType);
            var users = await userCollection.FindAsync((u) => u.Id == id.AsObjectId());
            return users.FirstOrDefault();
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var userCollection = db.GetCollection<User>(collectionType);
            var users = await userCollection.FindAsync((u) => u.Username == username);
            return users.FirstOrDefault();
        }
        public async Task<User> CreateUser(User user)
        {
            var userCollection = db.GetCollection<User>(collectionType);
            await userCollection.InsertOneAsync(user);
            return user;
        }
    }
}
