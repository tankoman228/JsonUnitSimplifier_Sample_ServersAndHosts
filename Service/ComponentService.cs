using ServersAndHosts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServersAndHosts.Entity;

namespace ServersAndHosts.Service
{
    public class ComponentService : IComponentService
    {
        IRepository<Entity.component> repository;
        public ComponentService(IRepository<Entity.component> repos)
        {
            repository = repos;
        }      

        public List<string> GetComponents()
        {
            List<string> components = new List<string>();

            var comps = repository.GetAll("component_type");
            foreach (var comp in comps)
            {
                components.Add(ComponentToString(comp));
            }
            return components;
        }

        public void AddComponent(Entity.component component)
        {
            repository.Add(component);
        }

        public void RemoveComponent(string component)
        {
            var comps = repository.GetAll("component_type");
            foreach (var comp in comps)
            {
                if (ComponentToString(comp) == component)
                {
                    repository.Delete(comp.id);
                    return;
                }
            }
            throw new Exception("Not found");            
        }

        public void GetComponentsOfServer(server s)
        {
            throw new NotImplementedException();
        }

        private string ComponentToString(component comp)
        {
            return $"{comp.component_type.typename}: {comp.name}";
        }
    }
}
