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

        object obj = new object();

        public override void Update(host after_save)
        {
            lock (obj)
            {
                using (var db = new Entity.ServersAndHostsEntities2())
                {
                    var server = db.server.Find(after_save.id_server);
                    if (server == null) throw new Exception("Server not found");

                    var before_save = db.host.Find(after_save.id);
                    if (before_save == null) throw new Exception("Host not found");

                    // Разделите освобождение и занятие ресурсов на две логические операции
                    server.memory_free_kb += before_save.memory_kb_limit - after_save.memory_kb_limit;
                    server.cores_free += before_save.cpu_cores - after_save.cpu_cores;
                    server.ram_free_mb += before_save.ram_mb - after_save.ram_mb;

                    if (server.memory_free_kb < 0 || server.cores_free < 0 || server.ram_free_mb < 0)
                        throw new Exception("Not enough resources");

                    db.SaveChanges();
                }

                base.Update(after_save);
            }
        }

    }
}
