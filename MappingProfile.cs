namespace UltimateDotNetSkeleton
{
	using AutoMapper;

	using UltimateDotNetSkeleton.Dtos.Company;
	using UltimateDotNetSkeleton.Models;

	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Company, CompanyDto>()
				.ForMember(
					company => company.FullAddress,
					opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
		}
	}
}
