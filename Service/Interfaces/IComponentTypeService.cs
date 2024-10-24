using ServersAndHosts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServersAndHosts.Service
{
    public interface IComponentTypeService
    {
        List<Entity.component_type> GetComponentTypes();

        /// <summary>
        /// Если нужжно, создат, после чего вернёт идентификатор нового объекта
        /// </summary>
       int IdOrAddComponentTypeIfNotExists(string name);
    }
}
