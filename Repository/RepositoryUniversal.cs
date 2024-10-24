using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServersAndHosts.Repository
{
    public class RepositoryUniversal<T> : IRepository<T> where T : class
    {
        private static PropertyInfo[] props;
        private static PropertyInfo prop_id;

        public RepositoryUniversal()
        {
            props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var t = typeof(T).Name;
            prop_id = props.Where(f => f.Name.Equals("id")).First();
        }

        public virtual IEnumerable<T> GetAll(string[] include = null)
        {
            using (DbContext context = new Entity.ServersAndHostsEntities())
            {
                if (include == null) return context.Set<T>().ToList();
                else
                {
                    DbQuery<T> set = context.Set<T>();
                    foreach (string item in include)
                    {
                        set = set.Include(item);
                    }
                    return set.ToList();
                }
            }
        }

        public virtual T GetById(int id)
        {
            using (DbContext context = new Entity.ServersAndHostsEntities())
            {
                return context.Set<T>().Find(id);
            }
        }

        public virtual int Add(T entity)
        {
            using (DbContext context = new Entity.ServersAndHostsEntities())
            {
                T value = context.Set<T>().Add(entity);
                context.SaveChanges();
                return (int)prop_id.GetValue(value);
            }
        }



        public virtual void Update(T entity)
        {          
            using (DbContext context = new Entity.ServersAndHostsEntities())
            {
                var set = context.Set<T>();
                set.Attach(entity);

                context.Entry(entity).State = EntityState.Modified; // Устанавливаем состояние как измененное
                context.SaveChanges();
            }
        }

        public virtual void Delete(int id)
        {
            using (DbContext context = new Entity.ServersAndHostsEntities())
            {
                var entity = context.Set<T>().Find(id);
                if (entity != null)
                {
                    context.Set<T>().Remove(entity);
                    context.SaveChanges();
                }
            }
        }
    }
}
