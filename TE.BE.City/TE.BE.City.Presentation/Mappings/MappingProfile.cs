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
            CreateMap<UserResponse, UserEntity>();

            CreateMap<LightEntity, LightResponse>();
            CreateMap<LightResponse, LightEntity>();

            CreateMap<SewerEntity, SewerResponse>();
            CreateMap<SewerResponse, SewerEntity>();

            CreateMap<UserEntity, AuthenticateResponse>();
            CreateMap<AuthenticateResponse, UserEntity>();

            CreateMap<TrashEntity, TrashResponse>();
            CreateMap<TrashResponse, TrashEntity>();

            CreateMap<CollectEntity, CollectResponse>();
            CreateMap<CollectResponse, CollectEntity>();

            CreateMap<AsphaltEntity, AsphaltResponse>();
            CreateMap<AsphaltResponse, AsphaltEntity>();

            CreateMap<StatusEntity, StatusResponseModel>();
            CreateMap<StatusResponseModel, StatusEntity>();

            CreateMap<WaterEntity, WaterResponse>();
            CreateMap<WaterResponse, WaterEntity>();

            CreateMap<PublicServiceEntity, PublicServiceResponse>();
            CreateMap<PublicServiceResponse, PublicServiceEntity>();
        }
    }
}
