using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServersAndHosts.Entity;
using ServersAndHosts.Repository;

namespace ServersAndHosts.Service
{
    public class ServerService : IServerService
    {
        private IRepository<Entity.server> repository;
        public ServerService(IRepository<Entity.server> repos)
        {
            repository = repos;
        }

        public bool MayHost(host host)
        {
            throw new NotImplementedException();
        }

        public List<Entity.server> GetAllServers()
        {
            return repository.GetAll(new string[] {
                "server_component", "server_component.component", "server_component.component.component_type"
            }).ToList();
        }

        public void Update(server s)
        {
            repository.Update(s);
        }

        public void Delete(server s)
        {
            repository.Delete(s.id);
        }

        public void Insert(server s)
        {
            repository.Add(s);
        }
    }
}
