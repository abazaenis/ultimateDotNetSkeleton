namespace UltimateDotNetSkeleton.Mapper
{
	using AutoMapper;

	using UltimateDotNetSkeleton.Dtos.Company;
	using UltimateDotNetSkeleton.Dtos.Employee;
	using UltimateDotNetSkeleton.Models;

	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Company, CompanyDto>()
				.ForMember(
					company => company.FullAddress,
					opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));

			CreateMap<Employee, EmployeeDto>();

			CreateMap<CompanyForCreationDto, Company>();

			CreateMap<EmployeeForCreationDto, Employee>();
		}
	}
}
