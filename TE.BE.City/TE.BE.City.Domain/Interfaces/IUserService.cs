using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Domain.Interfaces
{
    public interface IUserService
    {
        Task<UserEntity> Authenticate(string username, string password);       
        Task<UserEntity> Post(UserEntity request);
        Task<UserEntity> Put(UserEntity request);
        Task<UserEntity> Delete(int id);
        Task<IEnumerable<UserEntity>> GetAll();
        Task<UserEntity> GetById(int id);
        Task<int> ValidateJWTToken(string token);
        Task<UserEntity> Recovery(string userName);
    }
}
