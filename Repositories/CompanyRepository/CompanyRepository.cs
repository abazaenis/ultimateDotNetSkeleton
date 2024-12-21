namespace UltimateDotNetSkeleton.Repositories.CompanyRepository
{
    using System.Collections.Generic;
    using UltimateDotNetSkeleton.Models;
    using UltimateDotNetSkeleton.Repositories.Base;
    using UltimateDotNetSkeleton.Repositories.Context;

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
    }
}
