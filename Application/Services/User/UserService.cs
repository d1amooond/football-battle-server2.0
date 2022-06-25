using Application.Dtos;
using Application.General;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Extensions;
using Application.Requests;
using Utils.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Net.Http.Headers;

namespace Application.Services.Question
{
    public class UserService
    {
        private readonly Context app;
        private readonly string secret;
        public UserService(Context app)
        {
            this.app = app;
            this.secret = app.Configuration.GetSection("JWTSettings:Secret").Value;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public string GenerateAccessToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        async public Task<Response<TokensDTO>> LoginUser(LoginUserRequest request)
        {
            var response = new Response<TokensDTO>();
            try
            {
                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    return response.Failure("Username is required");
                }

                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return response.Failure("Password is required");
                }

                var user = await this.app.Repository.User.GetUserByUsername(request.Username);
                if (user == default)
                {
                    return response.Failure("Cannot find user with this username");
                }

                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                {
                    return response.Failure("Password is incorrect");
                }

                var accessToken = this.GenerateAccessToken(request.Username);
                var refreshToken = this.GenerateRefreshToken();

                var refreshTokenEntity = new RefreshToken
                {
                    Username = request.Username,
                    Token = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(1)
                };

                await this.app.Repository.User.CreateRefreshToken(refreshTokenEntity);

                var tokens = new TokensDTO
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60)
                };

                return response.Success(tokens);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return response;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.secret)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
        async public Task<Response<TokensDTO>> RefreshTokens(string refreshToken)
        {
            var response = new Response<TokensDTO>();
            try
            {
                var token = await this.app.Repository.User.GetRefreshToken(refreshToken);

                if (token == default)
                {
                    return response.Failure("Incorrect refresh token");
                }

                var newAccessToken = this.GenerateAccessToken(token.Username);
                var newRefreshToken = this.GenerateRefreshToken();

                var refreshTokenEntity = new RefreshToken
                {
                    Username = token.Username,
                    Token = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(1)
                };

                await this.app.Repository.User.CreateRefreshToken(refreshTokenEntity);


                // revoke token
                token.Revoked = DateTime.UtcNow;
                await this.app.Repository.User.RevokeRefreshToken(token);

                var tokens = new TokensDTO
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60)
                };

                return response.Success(tokens);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return response;
        } 

        public async Task<Response<UserDTO>> GetUserContext(string token)
        {
            var response = new Response<UserDTO>();
            try
            {
                var principal = GetPrincipalFromExpiredToken(token);
                
                if (principal == null)
                {
                    response.Failure("There are no principal with this token");
                }

                var user = await this.app.Repository.User.GetUserByUsername(principal.Identity.Name);

                if (user == null)
                {
                    return response.Failure("There are not user with such username");
                }

                var role = await this.app.Repository.User.GetRoleByUserId(user.Id);

                if (role == null)
                {
                    return response .Failure("Cannot find role for user");
                }

                var profile = await this.app.Repository.User.GetProfileByUserId(user.Id);

                if (profile == null)
                {
                    return response.Failure("Cannot find profile for user");
                }

                var userDTO = user.ToDTO();
                userDTO.Profile = profile.ToDTO();
                userDTO.Role = role.ToDTO();

                return response.Success(userDTO);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return response;
        }

        async public Task<Response<Guid>> RegisterUser(RegisterUserRequest request)
        {
            var response = new Response<Guid>();

            try
            {
                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    return response.Failure("Name is required");
                }

                if (request.Password.Length < 8)
                {
                    return response.Failure("Password should have at least 8 symbols");
                }

                var user = new User
                {
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Username = request.Username,
                };

                var createdUser = await this.app.Repository.User.CreateUser(user);

                // create role
                var role = new Role
                {
                    UserId = createdUser.Id,
                    Type = Roles.User,
                };

                var createdRole = await this.app.Repository.User.InsertInto<Role>(role, "roles");

                // create profile
                var profile = new Profile
                {
                    UserId = user.Id,
                };

                await this.app.Repository.User.InsertInto<Profile>(profile, "profiles");

                return response.Success(createdUser.Id.AsGuid());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return response;
         }
    }
}
