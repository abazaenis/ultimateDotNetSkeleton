namespace UltimateDotNetSkeleton.Domain.Repositories.CompanyRepository
{
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;
    using UltimateDotNetSkeleton.Domain.Context;
    using UltimateDotNetSkeleton.Domain.Models;
    using UltimateDotNetSkeleton.Domain.Repositories.Base;

    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
	{
		public CompanyRepository(RepositoryContext repositoryContext)
			: base(repositoryContext)
		{
		}

		public async Task<Company?> GetCompanyAsync(Guid companyId, bool trackChanges) =>
			await FindByCondition(company => company.Id.Equals(companyId), trackChanges).SingleOrDefaultAsync();

		public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges) =>
			await FindAll(trackChanges).OrderBy(company => company.Name).ToListAsync();

		public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
			await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();

		public void CreateCompany(Company company) =>
			Create(company);

		public void DeleteCompany(Company company) => Delete(company);
	}
}
