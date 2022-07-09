using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Domain.Interfaces;
using TE.BE.City.Infra.CrossCutting;

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
                Claim claimUserId = new Claim("userId", userEntity.Id.ToString());
                IList<Claim> claims = new List<Claim>()
                {
                    claimName,
                    claimRole,
                    claimEmail,
                    claimUserId
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

        public async Task<bool> SendMail(UserEntity userEntity)
        {
            string url = _config["SmtpSettings:Url"] + userEntity.Token;

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_config["SmtpSettings:SenderEmail"]));
            message.To.Add(MailboxAddress.Parse(userEntity.Username));
            message.Subject = "Projeto Lupa NH - Recuperação de Senha";
            message.Body = new TextPart("plain")
            {
                Text = "Olá. Você requisitou uma redefinição de senha. Para prosseguir clique no link abaixo. Caso não tenha requisitado, por favor ignore este e-mail." +
                "\nAcesse pelo link: " + url +
                "\n\n" +
                "Projeto Luta NH"
            };

            var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(_config["SmtpSettings:Server"], int.Parse(_config["SmtpSettings:Port"]), true);
                await client.AuthenticateAsync(new NetworkCredential(_config["SmtpSettings:SenderEmail"], _config["SmtpSettings:Password"]));
                //await client.SendAsync(message);
                await client.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                client.Dispose();
            }

            return true;
        }
    }
}
