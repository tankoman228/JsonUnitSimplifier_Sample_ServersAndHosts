using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServersAndHosts.Entity;
using ServersAndHosts.Repository;

namespace ServersAndHosts.Service
{
    public interface IServerService
    {
        bool MayHost(host host);

        List<Entity.server> GetAllServers();

        void Update(server s);

        void Delete(server s);

        void Insert(server s);
    }
}
