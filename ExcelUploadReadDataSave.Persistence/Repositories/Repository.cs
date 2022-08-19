using ExcelUploadReadDataSave.Application.Repositories;
using ExcelUploadReadDataSave.Domain.Entities.Common;
using ExcelUploadReadDataSave.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExcelUploadReadDataSave.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
        }
        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll()
            => Table;
        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method)
            => Table.Where(method);
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method)
            => await Table.FirstOrDefaultAsync(method);
        public async Task<T> GetById(int id)
            => await Table.FindAsync(id);
        public async Task<bool> AddAsync(T model)
        {
            EntityEntry<T> entity = await Table.AddAsync(model);
            return entity.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<T> datas)
        {
            await Table.AddRangeAsync(datas);
            return true;
        }
        public bool Remove(T model)
        {
            EntityEntry<T> entity = Table.Remove(model);
            return entity.State == EntityState.Deleted;
        }
        public async Task<bool> RemoveAsync(int id)
        {
            T entity = await Table.FirstOrDefaultAsync(x => x.Id == id);
            return Remove(entity);
        }
        public bool RemoveRange(List<T> datas)
        {
            Table.RemoveRange(datas);
            return true;
        }
        public bool Update(T model)
        {
            EntityEntry<T> entity = Table.Update(model);
            return entity.State == EntityState.Modified;
        }
        public async Task<int> SaveAsync()
            => await _context.SaveChangesAsync();
    }
}
