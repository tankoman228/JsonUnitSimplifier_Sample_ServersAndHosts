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
                "server_component", "host", "server_component.component", "server_component.component.component_type"
            }).ToList();
        }

        public void Update(server s, List<server_component> components)
        {
            s.address = s.address;
            s.name_in_network = s.name_in_network;

            UpdateResources(s, components);
            s.server_component.Clear();
            foreach (var c in components)
            {
                s.server_component.Add(new server_component
                {
                    id_component = c.id_component,
                    id_server = s.id,
                });
            }
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

        public void Create(string address, string name, List<server_component> components)
        {
            var server = new Entity.server
            {
                address = address,
                name_in_network = name,
                server_component = new List<server_component>()
            };
            foreach (var c in components)
            {
                server.server_component.Add(new server_component { id_component = c.id_component });
            }
            repository.Add(server);

            // Auto detecting and saving basic server info
            UpdateResources(server, components);
        }

        public List<string> GetAbout(server s)
        {
            var aboutList = new List<string>();
            foreach (var x in s.server_component)
            {
                try
                {
                    string about = $"{x.component.component_type.typename}:\t\t{x.component.name}\t";
                    if (x.component.memory != null)
                    {
                        about += $"size of: {x.component.memory}\t";
                    }
                    if (x.component.cores != null)
                    {
                        about += $"{x.component.cores} cores\t";
                    }
                    if (x.component.mhz != null)
                    {
                        about += $"Freq: {x.component.mhz} MHz";
                    }
                    aboutList.Add(about);
                }
                catch (Exception ex) { Console.Error.WriteLine(ex.StackTrace); }
            }
            return aboutList;
        }

        /// <summary>
        /// Update server's fields about ram, cpu, disk space. update components list
        /// </summary>
        private void UpdateResources(server server, List<server_component> components)
        {
            int ram = 0, cores = 0, disk = 0;
            foreach (var c in components)
            {
                try
                {
                    if (c.component.component_type.typename.ToLower().Equals("ram"))
                    {
                        ram += (int)c.component.memory;
                    }
                    else if (c.component.component_type.typename.ToLower().Equals("cpu"))
                    {
                        cores += (int)c.component.cores;
                    }
                    else if (c.component.component_type.typename.ToLower().Contains("hard"))
                    {
                        disk += (int)c.component.memory;
                    }
                    else if (c.component.component_type.typename.ToLower().Contains("ssd"))
                    {
                        disk += (int)c.component.memory;
                    }
                }
                catch (Exception ex) { }
            }

            server.ram_free_mb = ram;
            server.ram_total_mb = ram;
            server.cores_free = cores;
            server.cores_total = cores;
            server.memory_free_kb = disk * 1024;
            server.memory_total_kb = disk * 1024;

            foreach (var host in server.host)
            {
                server.ram_free_mb -= host.ram_mb;
                server.memory_free_kb -= host.memory_kb_took;
                server.ram_free_mb -= host.ram_mb;
            }          
        }
    }
}
