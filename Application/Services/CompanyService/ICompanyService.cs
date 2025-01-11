namespace UltimateDotNetSkeleton.Application.Services.CompanyService
{
	using UltimateDotNetSkeleton.Application.DataTransferObjects.Company;

	public interface ICompanyService
	{
		Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges);

		Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges);

		Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

		Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company, bool trackChanges);

		Task<(IEnumerable<CompanyDto> Companies, string Ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection);

		Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges);

		Task DeleteCompanyAsync(Guid companyId, bool trackChanges);
	}
}
