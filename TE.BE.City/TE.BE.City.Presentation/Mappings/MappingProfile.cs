using AutoMapper;
using System.Collections.Generic;
using TE.BE.City.Domain.Entity;
using TE.BE.City.Presentation.Model.Response;

namespace TE.BE.City.Presentation.Mappings
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Mapping only from IN/OUT. Entity -> Model
        /// </summary>
        public MappingProfile()
        {
            CreateMap<UserEntity, UserResponse>();
            CreateMap<LightEntity, LightResponse>();
            CreateMap<SewerEntity, SewerResponse>();
            CreateMap<UserEntity, AuthenticateResponse>();
            CreateMap<TrashEntity, TrashResponse>();
            CreateMap<CollectEntity, CollectResponse>();
            CreateMap<AsphaltEntity, AsphaltResponse>();
            CreateMap<StatusEntity, StatusResponseModel>();
            CreateMap<WaterEntity, WaterResponse>();
            CreateMap<PublicServiceEntity, PublicServiceResponse>();
        }
    }
}
