﻿namespace UltimateDotNetSkeleton.Application.Mapper
{
    using AutoMapper;
    using UltimateDotNetSkeleton.Application.DataTransferObjects.Company;
    using UltimateDotNetSkeleton.Application.DataTransferObjects.Employee;
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

            CreateMap<EmployeeForUpdateDto, Employee>();
        }
    }
}
