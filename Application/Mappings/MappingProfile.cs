namespace UltimateDotNetSkeleton.Application.Mappings
{
	using AutoMapper;

	using UltimateDotNetSkeleton.Application.DTOs.Authentication;
	using UltimateDotNetSkeleton.Application.DTOs.Company;
	using UltimateDotNetSkeleton.Application.DTOs.Employee;
	using UltimateDotNetSkeleton.Domain.Models;

	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
			CreateEmployeeMappings();
			CreateCompanyMappings();
			CreateUserMappings();
		}

        public void CreateEmployeeMappings()
        {
			CreateMap<Employee, EmployeeDto>();

			CreateMap<EmployeeForCreationDto, Employee>();

			CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();
		}

        public void CreateCompanyMappings()
        {
			CreateMap<Company, CompanyDto>()
				.ForMember(
					company => company.FullAddress,
					opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));

			CreateMap<CompanyForCreationDto, Company>();

			CreateMap<CompanyForUpdateDto, Company>();
		}

        public void CreateUserMappings()
		{
            CreateMap<UserForRegistrationDto, User>();
		}
	}
}
