namespace UltimateDotNetSkeleton.Domain.Repositories.CompanyRepository
{
    using UltimateDotNetSkeleton.Domain.Models;

    public interface ICompanyRepository
    {
        Company? GetCompany(Guid companyId, bool trackChanges);

        IEnumerable<Company> GetAllCompanies(bool trackChanges);

        IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges);

        void CreateCompany(Company company);

        void DeleteCompany(Company company);
    }
}
