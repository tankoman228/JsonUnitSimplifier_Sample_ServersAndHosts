using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServersAndHosts.Repository
{
    internal class RepositoryServer : RepositoryUniversal<Entity.server>
    {
        public override void Update(Entity.server entity_)
        {
            using (var context = new Entity.ServersAndHostsEntities())
            {
                var entity = context.server.Find(entity_.id);
                entity.address = entity_.address;
                entity.name_in_network = entity_.name_in_network;

                var prev_comps = context.server_component.Where(x => x.id_server == entity.id).ToList();
                foreach (var comp in prev_comps) { context.server_component.Remove(comp); }

                foreach (var comp in entity_.server_component) { 
                    context.server_component.Add(comp); 
                }

                context.SaveChanges();
            }
        }
    }
}
