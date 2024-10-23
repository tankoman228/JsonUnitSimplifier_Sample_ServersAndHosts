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
            throw new NotImplementedException();
        }

        public void Update(server s)
        {
            throw new NotImplementedException();
        }

        public void Delete(server s)
        {
            throw new NotImplementedException();
        }

        public void Insert(server s)
        {
            throw new NotImplementedException();
        }
    }
}
