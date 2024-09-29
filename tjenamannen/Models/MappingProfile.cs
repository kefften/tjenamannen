using AutoMapper;
using tjenamannen.Controllers;

namespace tjenamannen.Models;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<UserRegistrationModel, ApplicationUser>()
			.ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
	}
}