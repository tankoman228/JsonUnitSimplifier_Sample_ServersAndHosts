using ServersAndHosts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServersAndHosts.Entity;

namespace ServersAndHosts.Service
{
    public interface IComponentService
    {
        List<string> GetComponents();

        void AddComponent(Entity.component component);

        void RemoveComponent(string component);

        void GetComponentsOfServer(server s);
    }
}
