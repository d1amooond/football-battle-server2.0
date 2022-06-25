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
    public class UserRepo : BaseRepo, IUserRepo
    {
        private readonly IFootballDBContext db;

        private static readonly string collectionType = "users";

        public UserRepo(IFootballDBContext db) : base(db, collectionType)
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

            var refreshToken = await tokenCollection.FindAsync<RefreshToken>(t => t.Username == username && t.Revoked == null && t.ExpiresAt > DateTime.UtcNow);
            return refreshToken.FirstOrDefault();
        }

        public async Task<RefreshToken> GetRefreshToken(string token)
        {
            var tokenCollection = db.GetCollection<RefreshToken>("refresh_tokens");

            var refreshToken = await tokenCollection.FindAsync<RefreshToken>(t => t.Token == token && t.Revoked == null && t.ExpiresAt > DateTime.UtcNow);
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

        public async Task<Role> GetRoleByUserId(ObjectId userId)
        {
            var roleCollection = db.GetCollection<Role>("roles");
            var roles = await roleCollection.FindAsync((r) => r.UserId == userId);
            return roles.FirstOrDefault();
        }

        public async Task<Profile> GetProfileByUserId(ObjectId userId)
        {
            var profileCollection = db.GetCollection<Profile>("profiles");
            var profiles = await profileCollection.FindAsync(p => p.UserId == userId);
            return profiles.FirstOrDefault();
        }
    }
}
