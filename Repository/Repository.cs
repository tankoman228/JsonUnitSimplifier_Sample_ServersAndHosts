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
        private static PropertyInfo[] props;
        private static PropertyInfo prop_id;

        public Repository()
        {
            props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var t = typeof(T).Name;
            prop_id = props.Where(f => f.Name.Equals("id")).First();
        }

        public IEnumerable<T> GetAll(string include = null)
        {
            using (DbContext context = new Entity.ServersAndHostsEntities())
            {
                if (include == null) return context.Set<T>().ToList();
                else return context.Set<T>().Include(include).ToList();
            }
        }

        public T GetById(int id)
        {
            using (DbContext context = new Entity.ServersAndHostsEntities())
            {
                return context.Set<T>().Find(id);
            }
        }

        public int Add(T entity)
        {
            using (DbContext context = new Entity.ServersAndHostsEntities())
            {
                T value = context.Set<T>().Add(entity);
                context.SaveChanges();
                return (int)prop_id.GetValue(value);
            }
        }

        public void Update(T entity)
        {
            using (DbContext context = new Entity.ServersAndHostsEntities())
            {
                T obj = context.Set<T>().Find(entity);
                if (obj != null)
                {
                    foreach (var field in props)
                    {
                        field.SetValue(obj, field.GetValue(entity));
                    }
                }
                context.SaveChangesAsync();
            }
        }

        public void Delete(int id)
        {
            using (DbContext context = new Entity.ServersAndHostsEntities())
            {
                var entity = context.Set<T>().Find(id);
                if (entity != null)
                {
                    context.Set<T>().Remove(entity);
                    context.SaveChangesAsync();
                }
            }
        }
    }
}
