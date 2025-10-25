using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class GetUserRequestProfile : Profile
{
    public GetUserRequestProfile()
    {
        CreateMap<GetUserResult, GetUserResponse>();
    }
}
