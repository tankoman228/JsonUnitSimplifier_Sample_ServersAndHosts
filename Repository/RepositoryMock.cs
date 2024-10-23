using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServersAndHosts.Repository
{
#pragma warning disable CS1998

    internal class RepositoryMock<T> : IRepository<T> where T : class
    {
        private static List<T> objs = new List<T>();
        private static FieldInfo[] fields;
        private static FieldInfo field_id;

        public RepositoryMock()
        {            
            fields = typeof(T).GetFields();
            foreach (var field in fields)
            {
                if (field.Name.ToLower() == "id")
                {
                    field_id = field;
                    break;
                }
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(string v)
        {
            return objs;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return objs[id];
        }

        public async Task<int> AddAsync(T entity)
        {
            field_id.SetValue(entity, objs.Count);
            objs.Add(entity);
            return objs.Count - 1;
        }

        public async Task UpdateAsync(T entity)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                if (field_id.GetValue(objs[i]) == field_id.GetValue(entity))
                {
                    objs[i] = entity; return;
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            objs.RemoveAt(id);
        }
    }
}
#pragma warning restore CS1998