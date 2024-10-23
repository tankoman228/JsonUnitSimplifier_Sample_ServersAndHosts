using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServersAndHosts.Entity;
using ServersAndHosts.Repository;

namespace ServersAndHosts.Service
{
    public class HostService : IHostService
    {
        private IRepository<Entity.host> repository;
        public HostService(IRepository<Entity.host> repos)
        {
            repository = repos;
        }

        public List<host> GetAllHosts()
        {
            throw new NotImplementedException();
        }

        public List<host> SaveHosts(List<host> newHosts)
        {
            throw new NotImplementedException();
        }

        public void Delete(host s)
        {
            throw new NotImplementedException();
        }

        public void Insert(host s)
        {
            throw new NotImplementedException();
        }
    }
}
