namespace UltimateDotNetSkeleton.Services.CompanyService
{
	using UltimateDotNetSkeleton.Logger;
	using UltimateDotNetSkeleton.Repository.Manager;

	internal sealed class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public CompanyService(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }
    }
}
