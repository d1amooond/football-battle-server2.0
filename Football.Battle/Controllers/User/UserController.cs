using Application.Dtos;
using Application.General;
using Application.Requests;
using DataAccess.DBContext;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace Football.Battle.Server.Controllers.User
{
    public class UserController : ControllerBase
    {

        private readonly Context app;
        private readonly IFootballDBContext context;

        public UserController(IFootballDBContext context, IConfiguration configuration)
        {
            this.app = new Context(context, configuration);
        }

        [HttpPost("Users/Register")]
        public Task<Response<Guid>> RegisterUser([FromBody] RegisterUserRequest request)
        {
            return app.Services.User.RegisterUser(request);
        }

        [HttpPost("Users/Login")]
        public Task<Response<TokensDTO>> LoginUser([FromBody] LoginUserRequest request)
        {
            return app.Services.User.LoginUser(request);
        }

        [HttpPost("Users/Token")]
        public Task<Response<TokensDTO>> RefreshToken([FromBody] string token)
        {
            return app.Services.User.RefreshTokens(token);
        }

        [HttpGet("Users/Context"), Authorize]
        public Task<Response<UserDTO>> GetUserContext()
        {
            return app.Services.User.GetUserContext(Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""));
        }
    }
}
