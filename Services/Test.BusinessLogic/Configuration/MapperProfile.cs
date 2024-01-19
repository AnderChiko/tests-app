using AutoMapper;
using Test.BusinessLogic.Models;
using context = Test.Context;

namespace Test.Core.Configuration
{

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Tasks, context.Tasks>()
                 .ForMember(d => d.Id, act => act.MapFrom(src => src.Id))
                 .ForMember(d => d.Title, act => act.MapFrom(src => src.Title))
                 .ForMember(d => d.Description, act => act.MapFrom(src => src.Description))
                 .ForMember(d => d.Assignee, act => act.MapFrom(src => src.Assignee))
                 .ForMember(d => d.DueDate, act => act.MapFrom(src => src.DueDate))
                 .ReverseMap();

            CreateMap<User, context.User>()
                 .ForMember(d => d.Id, act => act.MapFrom(src => src.Id))
                 .ForMember(d => d.Username, act => act.MapFrom(src => src.Username))
                 .ForMember(d => d.Password, act => act.MapFrom(src => src.Password))
                 .ForMember(d => d.Email, act => act.MapFrom(src => src.Email))
                 .ReverseMap();
        }
    }
}
