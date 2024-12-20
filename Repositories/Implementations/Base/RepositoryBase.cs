﻿namespace UltimateDotNetSkeleton.Repositories.Implementations.Base
{
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore;
    using UltimateDotNetSkeleton.Repositories.Contracts;
    using UltimateDotNetSkeleton.Repository.Context;

    public abstract class RepositoryBase<T> : IRepositoryBase<T>
        where T : class
    {
        private readonly RepositoryContext repositoryContext;

        protected RepositoryBase(RepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll(bool trackChanges) => !trackChanges ? repositoryContext.Set<T>().AsNoTracking() : repositoryContext.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ?
            repositoryContext.Set<T>()
                .Where(expression)
                .AsNoTracking() :
            repositoryContext.Set<T>()
                .Where(expression);

        public void Create(T entity) => repositoryContext.Set<T>().Add(entity);

        public void Update(T entity) => repositoryContext.Set<T>().Update(entity);

        public void Delete(T entity) => repositoryContext.Set<T>().Remove(entity);
    }
}
