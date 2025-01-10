namespace UltimateDotNetSkeleton.Domain.Repositories.CompanyRepository
{
    using UltimateDotNetSkeleton.Domain.Models;

    public interface ICompanyRepository
    {
        Task<Company?> GetCompanyAsync(Guid companyId, bool trackChanges);

        Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges);

        Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

        void CreateCompany(Company company);

        void DeleteCompany(Company company);
    }
}
