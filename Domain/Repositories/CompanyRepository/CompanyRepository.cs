namespace UltimateDotNetSkeleton.Domain.Repositories.CompanyRepository
{
    using System.Collections.Generic;
    using UltimateDotNetSkeleton.Domain.Models;
    using UltimateDotNetSkeleton.Domain.Repositories.Base;
    using UltimateDotNetSkeleton.Domain.Repositories.Context;

    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public Company? GetCompany(Guid companyId, bool trackChanges) =>
            FindByCondition(company => company.Id.Equals(companyId), trackChanges)
            .SingleOrDefault();

        public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
            [.. FindAll(trackChanges).OrderBy(company => company.Name)];

        public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges) =>
            [.. FindByCondition(x => ids.Contains(x.Id), trackChanges)];

        public void CreateCompany(Company company) =>
            Create(company);

        public void DeleteCompany(Company company) => Delete(company);
    }
}
