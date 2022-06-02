using AutoMapper;
using CimcikSozluk.Api.Domain.Models;
using CimcikSozluk.Common.Models.Queries;

namespace CimcikSozluk.Api.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, LoginUserViewModel>().ReverseMap();
    }
}