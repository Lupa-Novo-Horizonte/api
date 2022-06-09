using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;

namespace TE.BE.City.Domain
{
    public class UserDomain : IUserDomain
    {
        private IConfiguration _config;

        public UserDomain(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> GenerateJWTToken(UserEntity userEntity)
        {
            return await Task.Run(() =>
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                Claim claimName = new Claim("fullName", userEntity.FirstName + userEntity.LastName);
                Claim claimRole = new Claim("role", userEntity.RoleId.ToString());
                Claim claimEmail = new Claim("email", userEntity.Username);
                IList<Claim> claims = new List<Claim>()
                {
                    claimName,
                    claimRole,
                    claimEmail
                };

                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            });
        }

        public async Task<int> ValidateJWTToken(string token)
        {
            return await Task.Run(() =>
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                return userId;
            });
        }

        public async Task<string> Encrypt(string password)
        {
            return await Task.Run(() =>
                {
                    MD5 md5 = MD5.Create();
                    byte[] inputBytes = Encoding.ASCII.GetBytes(password);
                    byte[] hash = md5.ComputeHash(inputBytes);

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hash.Length; i++)
                    {
                        sb.Append(hash[i].ToString("X2"));
                    }
                    return sb.ToString();
                });
        }

        public async Task<bool> IsValidPassword(string attemptPassword, string savedPassword)
        {
            if (await Encrypt(attemptPassword) == savedPassword)
                return true;
            else
                return false;
        }
    }
}
