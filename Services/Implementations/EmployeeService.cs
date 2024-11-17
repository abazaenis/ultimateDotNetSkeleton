namespace UltimateDotNetSkeleton.Services.Implementations
{
	using AutoMapper;

	using UltimateDotNetSkeleton.Repositories.Contracts;
	using UltimateDotNetSkeleton.Services.Contracts;
	using UltimateDotNetSkeleton.Utilities.Logger;

	internal sealed class EmployeeService : IEmployeeService
	{
		private readonly IRepositoryManager _repository;
		private readonly ILoggerManager _logger;
		private readonly IMapper _mapper;

		public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
		{
			_repository = repository;
			_logger = logger;
			_mapper = mapper;
		}
	}
}
