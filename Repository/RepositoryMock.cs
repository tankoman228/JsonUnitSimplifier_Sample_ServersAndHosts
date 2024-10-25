using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServersAndHosts.Repository
{

    public class RepositoryMock<T> : IRepository<T> where T : class
    {
        private static List<T> objs = new List<T>();
        private static PropertyInfo[] props;
        private static PropertyInfo prop_id;

        public RepositoryMock()
        {            
            props = typeof(T).GetProperties();
            foreach (var field in props)
            {
                if (field.Name.ToLower() == "id")
                {
                    prop_id = field;
                    break;
                }
            }
        }

        public IEnumerable<T> GetAll(string[] includes = null)
        {
            return objs;
        }

        public T GetById(int id)
        {
            return objs[id];   
        }  

        public int Add(T entity)
        {
            prop_id.SetValue(entity, objs.Count); 
            objs.Add(entity);
            Console.WriteLine((int)prop_id.GetValue(entity));
            return (int)prop_id.GetValue(entity);
        }

        public void Update(T entity)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                if (prop_id.GetValue(objs[i]) == prop_id.GetValue(entity))
                {
                    objs[i] = entity; return;
                }
            }
        }

        public void Delete(int id)
        {
            objs.RemoveAt(id);
        }
    }
}