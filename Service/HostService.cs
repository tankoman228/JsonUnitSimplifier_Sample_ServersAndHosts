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
            return repository.GetAll(new string[] { "server" }).ToList();
        }

        public void Update(host newHost)
        {
            repository.Update(newHost);
        }

        public void Delete(host s)
        {
            repository.Delete(s.id);
        }

        public void Insert(host s)
        {
            repository.Add(s);
        }
    }
}
