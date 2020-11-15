﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CSharpWars.DataAccess.Repositories.Interfaces;
using CSharpWars.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CSharpWars.DataAccess.Repositories
{
    public class Repository<TModel> : IRepository<TModel>
        where TModel : class, IHasId
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<TModel> _dbSet;

        public Repository(DbContext dbContext, DbSet<TModel> dbSet)
        {
            _dbContext = dbContext;
            _dbSet = dbSet;
        }

        public async Task<IList<TModel>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IList<TModel>> Find(Expression<Func<TModel, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IList<TModel>> Find<TProperty>(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, TProperty>> include)
        {
            return await _dbSet.AsNoTracking().Where(predicate).Include(include).ToListAsync();
        }

        public async Task<IList<TModel>> FindDescending<TKey>(
            Expression<Func<TModel, TKey>> keySelector, int count)
        {
            return await _dbSet
                .AsNoTracking()
                .OrderByDescending(keySelector)
                .Take(count)
                .ToListAsync();
        }

        public async Task<TModel> Single(Expression<Func<TModel, bool>> predicate)
        {
            return await _dbSet.SingleOrDefaultAsync(predicate);
        }

        public virtual async Task<TModel> Create(TModel toCreate)
        {
            toCreate.Id = Guid.NewGuid();
            await _dbSet.AddAsync(toCreate);
            await _dbContext.SaveChangesAsync();
            return toCreate;
        }

        public async Task Create(IList<TModel> toCreate)
        {
            foreach (var entity in toCreate)
            {
                entity.Id = Guid.NewGuid();
            }

            await _dbSet.AddRangeAsync(toCreate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(TModel toUpdate)
        {
            InternalUpdate(toUpdate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(IList<TModel> toUpdate)
        {
            foreach (var entity in toUpdate)
            {
                InternalUpdate(entity);
            }

            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task Delete(IList<TModel> toDelete)
        {
            _dbSet.RemoveRange(toDelete);
            await _dbContext.SaveChangesAsync();
        }

        private void InternalUpdate(TModel toUpdate)
        {
            var trackedEntity = _dbContext.ChangeTracker.Entries<TModel>().SingleOrDefault(x => x.Entity.Id == toUpdate.Id);
            if (trackedEntity != null)
            {
                trackedEntity.State = EntityState.Detached;
            }

            _dbContext.Entry(toUpdate).State = EntityState.Modified;
        }
    }
}