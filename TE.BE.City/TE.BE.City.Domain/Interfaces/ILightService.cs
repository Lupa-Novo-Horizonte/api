using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TE.BE.City.Domain.Entity;

namespace TE.BE.City.Domain.Interfaces
{
    public interface ILightService
    {
        Task<LightEntity> Post(LightEntity request);
        Task<LightEntity> Put(LightEntity request);
        Task<LightEntity> Delete(int id);
        Task<LightEntity> Get();
    }
}
