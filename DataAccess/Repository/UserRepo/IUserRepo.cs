using Domain.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IUserRepo
    {
        Task CreateRefreshToken(RefreshToken token);
        Task RevokeRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenByUsername(string username);
        Task<List<User>> GetUsers();

        Task<User> GetUserById(Guid id);

        Task<User> GetUserByUsername(string username);

        Task<User> CreateUser(User user);

        //Task<User> UpdateUser(User user);
    }
}
