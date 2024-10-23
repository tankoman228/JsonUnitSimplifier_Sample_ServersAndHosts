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
        private static FieldInfo[] fields;
        private static FieldInfo field_id;

        public Repository()
        {
            fields = typeof(T).GetFields();    
            field_id = fields.Where(f => f.Name == "id").First();
        }

        public async Task<IEnumerable<T>> GetAllAsync(string include = null)
        {
            using (DbContext context = new Entity.ServersAndHostsEntities())
            {
                if (include == null) return await context.Set<T>().ToListAsync();
                else return await context.Set<T>().Include(include).ToListAsync();
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            using (DbContext context = new Entity.ServersAndHostsEntities())
            {
                return await context.Set<T>().FindAsync(id);
            }
        }

        public async Task<int> AddAsync(T entity)
        {
            using (DbContext context = new Entity.ServersAndHostsEntities())
            {
                T value = context.Set<T>().Add(entity);
                await context.SaveChangesAsync();
                return (int)field_id.GetValue(value);
            }
        }

        public async Task UpdateAsync(T entity)
        {
            using (DbContext context = new Entity.ServersAndHostsEntities())
            {
                T obj = await context.Set<T>().FindAsync(entity);
                if (obj != null)
                {
                    foreach (var field in fields)
                    {
                        field.SetValue(obj, field.GetValue(entity));
                    }
                }
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (DbContext context = new Entity.ServersAndHostsEntities())
            {
                var entity = await context.Set<T>().FindAsync(id);
                if (entity != null)
                {
                    context.Set<T>().Remove(entity);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
