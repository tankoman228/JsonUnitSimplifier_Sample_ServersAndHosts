using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServersAndHosts.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        private static FieldInfo[] fields;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
            fields = typeof(T).GetFields();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync(); // Сохранение изменений
        }

        public async Task UpdateAsync(T entity)
        {
            T obj = await _dbSet.FindAsync(entity);
            if (obj != null)
            {
                foreach (var field in fields)
                {
                    field.SetValue(obj, field.GetValue(entity));
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
