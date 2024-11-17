namespace UltimateDotNetSkeleton.Services.Implementations
{
	using System.Collections.Generic;

	using AutoMapper;

	using UltimateDotNetSkeleton.Dtos.Company;
	using UltimateDotNetSkeleton.Models;
	using UltimateDotNetSkeleton.Repositories.Contracts;
	using UltimateDotNetSkeleton.Services.Contracts;
	using UltimateDotNetSkeleton.Utilities.Logger;

	internal sealed class CompanyService : ICompanyService
	{
		private readonly IRepositoryManager _repository;
		private readonly ILoggerManager _logger;
		private readonly IMapper _mapper;

		public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
		{
			_repository = repository;
			_logger = logger;
			_mapper = mapper;
		}

		public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
		{
			try
			{
				var companies = _repository.Company.GetAllCompanies(trackChanges);

				var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

				return companiesDto;
			}
			catch (Exception ex)
			{
				_logger.LogError($"Something went wrong in the {nameof(GetAllCompanies)} service method {ex}");
				throw;
			}
		}
	}
}
