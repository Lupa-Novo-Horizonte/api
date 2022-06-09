using AutoMapper;
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

                 //.ForMember(destination => destination.Status, map =>
                 //{
                 //    map.PreCondition(src => (src.Status != null));
                 //    map.MapFrom(source => new StatusResponseModel
                 //    {
                 //        Id = source.Status.Id,
                 //        Name = source.Status.Name,
                 //        CreatedAt = source.Status.CreatedAt.Date,
                 //        Closed = source.Status.Closed
                 //    });
                 //})
                 //.ForMember(destination => destination.User, map =>
                 //{
                 //    map.PreCondition(src => (src.User != null));
                 //    map.MapFrom(source => new UserResponse
                 //    {
                 //        Id = source.Status.Id,
                 //        FirstName = source.User.FirstName,
                 //        LastName = source.User.LastName,
                 //        RoleId = source.User.RoleId.ToString(),
                 //        Username = source.User.Username,
                 //        Active = source.User.Active,
                 //        Block = source.User.Block,
                 //        CreatedAt = source.User.CreatedAt.Date
                 //    });
                 //}); ;
        }
    }
}
