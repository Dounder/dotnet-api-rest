using API.DTO.Common;
using AutoMapper;
using Core.Entities.Auth;
using Microsoft.AspNetCore.Identity;

namespace API.Mapper;

public class CommonProfile : Profile
{
    public CommonProfile()
    {
        CreateMap<IdentityUser, AppUser>();
        CreateMap<AppUser, IdentityUser>();
        CreateMap<AppUser, UserDto>();
        
        CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<decimal?, decimal>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<bool?, bool>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<double?, double>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<DateTime?, DateTime>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<Guid?, Guid>().ConvertUsing((src, dest) => src ?? dest);
    }
}