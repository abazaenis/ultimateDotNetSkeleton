namespace UltimateDotNetSkeleton.Application.Mappings
{
    using AutoMapper;
    using UltimateDotNetSkeleton.Application.DTOs.Company;
    using UltimateDotNetSkeleton.Application.DTOs.Employee;
    using UltimateDotNetSkeleton.Application.DTOs.User;
    using UltimateDotNetSkeleton.Domain.Models;

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

            CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();

            CreateMap<CompanyForUpdateDto, Company>();

            CreateMap<UserForRegistrationDto, User>();
		}
    }
}
