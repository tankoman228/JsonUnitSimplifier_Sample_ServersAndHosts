using ServersAndHosts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServersAndHosts.Entity;

namespace ServersAndHosts.Service
{
    public class ComponentService
    {
        IRepository<Entity.component> repository;
        public ComponentService(IRepository<Entity.component> repos)
        {
            repository = repos;
            /*
            repository.AddAsync(new Entity.component
            {
                component_type = new Entity.component_type
                {
                    typename = "TestCompType"
                },
                cores = 2,
                memory = 2,
                mhz = 3,
                name = "TestComp"
            }               
            );*/
        }      

        public async Task<List<string>> GetComponents()
        {
            List<string> components = new List<string>();

            var comps = await repository.GetAllAsync("component_type");
            foreach (var comp in comps)
            {
                components.Add($"{comp.component_type.typename}: {comp.name}");
            }
            return components;
        }

        public async void AddComponent(Entity.component component)
        {
            await repository.AddAsync(component);
        }

        public void RemoveComponent(string component)
        {
            throw new NotImplementedException();
        }

        public void GetComponentsOfServer(server s)
        {
            throw new NotImplementedException();
        }
    }
}
