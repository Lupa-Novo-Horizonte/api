using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Domain.Interfaces
{
    public interface IUserDomain
    {
        Task<string> GenerateJWTToken(UserEntity userEntity);
        Task<string> Encrypt(string password);
        Task<bool> IsValidPassword(string attemptPassword, string savedPassword);
        Task<int> ValidateJWTToken(string token);
    }
}
