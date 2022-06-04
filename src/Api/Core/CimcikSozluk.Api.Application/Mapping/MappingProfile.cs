using AutoMapper;
using CimcikSozluk.Api.Domain.Models;
using CimcikSozluk.Common.Models.Queries;
using CimcikSozluk.Common.Models.RequestModels;

namespace CimcikSozluk.Api.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, LoginUserViewModel>().ReverseMap();
        CreateMap<CreateUserCommand, User>();
        CreateMap<UpdateUserCommand, User>();
        CreateMap<CreateEntryCommand, Entry>().ReverseMap();
        CreateMap<CreateEntryCommentCommand, EntryComment>().ReverseMap();
    }
}