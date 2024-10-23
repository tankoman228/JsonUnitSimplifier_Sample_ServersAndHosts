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

        public async Task<List<Entity.component_type>> GetComponentTypes()
        {
            var res = await repository.GetAllAsync();
            return res.ToList();
        }

        /// <summary>
        /// Если нужжно, создат, после чего вернёт идентификатор нового объекта
        /// </summary>
        public async Task<int> IdOrAddComponentTypeIfNotExists(string name)
        {
            var res = await repository.GetAllAsync();
            var t = res.Where(x => x.typename == name).FirstOrDefault();

            if (t != null) return t.id;

            return await repository.AddAsync(new Entity.component_type { typename = name });
        }
    }
}
