using Domain.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IUserRepo : IBaseRepo
    {
        Task CreateRefreshToken(RefreshToken token);
        Task RevokeRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenByUsername(string username);
        Task<RefreshToken> GetRefreshToken(string token);
        Task<List<User>> GetUsers();

        Task<User> GetUserById(Guid id);

        Task<User> GetUserByUsername(string username);

        Task<User> CreateUser(User user);
        Task<Role> GetRoleByUserId(ObjectId userId);

        Task<Profile> GetProfileByUserId(ObjectId userId);

        //Task<User> UpdateUser(User user);
    }
}
