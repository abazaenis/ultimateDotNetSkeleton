namespace UltimateDotNetSkeleton.Application.Mappings
{
	using AutoMapper;

	using UltimateDotNetSkeleton.Application.DTOs.Authentication;
	using UltimateDotNetSkeleton.Application.DTOs.Company;
	using UltimateDotNetSkeleton.Application.DTOs.Employee;
	using UltimateDotNetSkeleton.Domain.Enums;
	using UltimateDotNetSkeleton.Domain.Models;
	using UltimateDotNetSkeleton.Infrastructure.Services.DateTimeHelper;

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
			CreateMap<BaseUserForRegistrationDto, User>();

			CreateMap<UserForRegistrationDto, User>()
				.IncludeBase<BaseUserForRegistrationDto, User>()
				.ForMember(dest => dest.RegistrationType, opt => opt.MapFrom(_ => RegistrationType.Native))
				.ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(_ => DateTimeHelper.GetUnspecifiedUtcNow()));

			CreateMap<AppleUserForRegistrationDto, User>()
				.IncludeBase<BaseUserForRegistrationDto, User>()
				.ForMember(dest => dest.ThirdPartyId, opt => opt.MapFrom(src => src.AppleId))
				.ForMember(dest => dest.RegistrationType, opt => opt.MapFrom(_ => RegistrationType.Apple));

			CreateMap<GoogleUserForRegistrationDto, User>()
				.IncludeBase<BaseUserForRegistrationDto, User>()
				.ForMember(dest => dest.ThirdPartyId, opt => opt.MapFrom(src => src.GoogleId))
				.ForMember(dest => dest.RegistrationType, opt => opt.MapFrom(_ => RegistrationType.Google));
		}
	}
}
