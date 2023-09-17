using AutoMapper;
using BookStore.API.Data;
using BookStore.API.Models.Users;

namespace BookStore.API.Configurations;

public class UsersMapper : Profile
{
	public UsersMapper()
	{
		CreateMap<UserDto, ApiUser>().ReverseMap();
	}
}

