﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using GylleneDroppen.Api.Data;
using GylleneDroppen.Api.Repositories.Interfaces;

namespace GylleneDroppen.Api.Repositories
{
    public class Repository<T> :IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<T> DbSet;

        protected Repository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            DbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }
    }
}
