namespace UltimateDotNetSkeleton.Services.EmployeeService
{
    using UltimateDotNetSkeleton.Logger;
    using UltimateDotNetSkeleton.Repository.Manager;

    internal sealed class EmployeeService : IEmployeeService
	{
		private readonly IRepositoryManager _repository;
		private readonly ILoggerManager _logger;

		public EmployeeService(IRepositoryManager repository, ILoggerManager logger)
		{
			_repository = repository;
			_logger = logger;
		}
	}
}
