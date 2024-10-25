using ServersAndHosts.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServersAndHosts.Repository
{
    internal class RepositoryHost : RepositoryUniversal<Entity.host>
    {
        public override int Add(host entity)
        {
            using (var db = new Entity.ServersAndHostsEntities2())
            {
                var server = db.server.Find(entity.id_server);

                server.memory_free_kb -= entity.memory_kb_limit;
                server.cores_free -= entity.cpu_cores;
                server.ram_free_mb -= entity.ram_mb;

                if (server.memory_free_kb < 0 || server.cores_free < 0 || server.ram_free_mb < 0)
                    throw new Exception("Not enough resources");

                db.host.Add(entity);
                db.SaveChanges();
            }
            return entity.id;
        }

        public override void Delete(int id)
        {
            using (var db = new Entity.ServersAndHostsEntities2())
            {
                var host = db.host.FirstOrDefault(x => x.id == id);
                if (host == null) throw new Exception("Host not found");

                var server = db.server.FirstOrDefault(x => x.id == host.id_server);
                if (server == null) throw new Exception("Server not found");

                server.memory_free_kb += host.memory_kb_limit;
                server.cores_free += host.cpu_cores;
                server.ram_free_mb += host.ram_mb;

                db.SaveChanges(); // Сохраняем изменения перед удалением
                base.Delete(id);
            }
        }

        static object obj = new object();

        public override void Update(host updatedHost)
        {
            lock (obj)
            {
                using (var db = new Entity.ServersAndHostsEntities2())
                {
                    var existingHost = db.host.FirstOrDefault(x => x.id == updatedHost.id);
                    if (existingHost == null) throw new Exception("Host not found");

                    var serverOfExisting = db.server.Include("host").FirstOrDefault(x => x.id == existingHost.id_server);
                    if (serverOfExisting == null) throw new Exception("Server not found");

                    var serverOfUpdated = db.server.Include("host").FirstOrDefault(x => x.id == updatedHost.id_server);

                    try
                    {
                        TryRecalculateServerResources(serverOfUpdated, updatedHost, db);
                        if (serverOfExisting.id != serverOfUpdated.id)
                        {
                            TryRecalculateServerResources(serverOfExisting, new host
                            {
                                cpu_cores = -updatedHost.cpu_cores,
                                ram_mb = -updatedHost.ram_mb,
                                memory_kb_limit = -updatedHost.memory_kb_limit
                            }, db);
                        }

                        base.Update(updatedHost); 
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Update failed: " + ex.Message);
                    }
                }
            }
        }

        private void TryRecalculateServerResources(server server, host newHost, ServersAndHostsEntities2 db)
        {
            var RAM = server.ram_total_mb;
            var DISK = server.memory_total_kb;
            var CPU = server.cores_total;


            foreach (var host in server.host)
            {
                if (host.id == newHost.id) continue;

                RAM -= host.ram_mb;
                DISK -= host.memory_kb_limit;
                CPU -= host.cpu_cores;
            }

            RAM -= newHost.ram_mb;
            DISK -= newHost.memory_kb_limit;
            CPU -= newHost.cpu_cores;

            if (RAM < 0 || DISK < 0 || CPU < 0)
                throw new Exception("No enough resources on this server");

            server.cores_free = CPU;
            server.memory_free_kb = DISK;
            server.ram_free_mb = RAM;

            db.SaveChanges();
        }
    }
}
