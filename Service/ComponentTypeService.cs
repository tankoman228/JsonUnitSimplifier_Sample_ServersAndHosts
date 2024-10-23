using ServersAndHosts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServersAndHosts.Service
{
    public class ComponentTypeService
    {
        private IRepository<Entity.component_type> repository;
        public ComponentTypeService(IRepository<Entity.component_type> repos)
        {
            repository = repos;
        }

        public List<Entity.component_type> GetComponentTypes()
        {
            var res = repository.GetAll();
            return res.ToList();
        }

        /// <summary>
        /// Если нужжно, создат, после чего вернёт идентификатор нового объекта
        /// </summary>
        public int IdOrAddComponentTypeIfNotExists(string name)
        {
            var res = repository.GetAll();
            var t = res.Where(x => x.typename == name).FirstOrDefault();

            if (t != null) return t.id;

            return repository.Add(new Entity.component_type { typename = name });
        }
    }
}
