﻿namespace UltimateDotNetSkeleton.Repositories.Implementations.Company
{
	using System.Collections.Generic;

	using UltimateDotNetSkeleton.Models;
	using UltimateDotNetSkeleton.Repositories.Contracts;
	using UltimateDotNetSkeleton.Repositories.Implementations.Base;
	using UltimateDotNetSkeleton.Repository.Context;

	public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
	{
		public CompanyRepository(RepositoryContext repositoryContext)
			: base(repositoryContext)
		{
		}

		public IEnumerable<Company> GetAllCompanies(bool trackChanges)
		{
			return FindAll(trackChanges).OrderBy(company => company.Name).ToList();
		}
	}
}