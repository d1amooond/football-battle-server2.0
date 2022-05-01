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
        private readonly string key;
        public UserService(Context app)
        {
            this.app = app;
            this.key = app.Configuration.GetSection("JwtKey").ToString();
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

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
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

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, request.Username),
                };

                var accessToken = this.GenerateAccessToken(claims);
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345")),
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
        async public Task<Response<TokensDTO>> RefreshTokens(string token)
        {
            var response = new Response<TokensDTO>();
            try
            {
                var principal = GetPrincipalFromExpiredToken(token);

                var refresh = await this.app.Repository.User.GetRefreshTokenByUsername(principal.Identity.Name);
                
                if (refresh == null)
                {
                    return response.Failure("Cannot find refresh token for access");
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, refresh.Username),
                };

                var accessToken = this.GenerateAccessToken(claims);
                var refreshToken = this.GenerateRefreshToken();

                var refreshTokenEntity = new RefreshToken
                {
                    Username = principal.Identity.Name,
                    Token = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(1)
                };

                await this.app.Repository.User.CreateRefreshToken(refreshTokenEntity);


                // revoke token
                refresh.Revoked = DateTime.UtcNow;
                await this.app.Repository.User.RevokeRefreshToken(refresh);

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

        async public Task<Response<UserDTO>> RegisterUser(RegisterUserRequest request)
        {
            var response = new Response<UserDTO>();

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

                if (string.IsNullOrWhiteSpace(request.Country))
                {
                    return response.Failure("Country is required");
                }

                var user = new User
                {
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Username = request.Username,
                };

                var createdUser = await this.app.Repository.User.CreateUser(user);
                return response.Success(createdUser.ToDTO());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return response;
         }
    }
}
