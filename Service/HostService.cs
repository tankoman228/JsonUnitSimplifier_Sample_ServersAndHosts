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
            var oldHost = repository.GetById(newHost.id);
            int CPU = oldHost.cpu_cores;
            int RAM = oldHost.ram_mb;
            int DISK = oldHost.memory_kb_limit;

            try
            {                
                repository.Update(newHost);
            }
            catch (Exception ex)
            {
                newHost.cpu_cores = CPU;
                newHost.ram_mb = RAM;
                newHost.memory_kb_limit = DISK;
                throw ex;
            }
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
