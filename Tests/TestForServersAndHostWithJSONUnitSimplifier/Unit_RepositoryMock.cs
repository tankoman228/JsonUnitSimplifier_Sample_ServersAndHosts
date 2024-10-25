using JsonUnitSimplifier;
using ServersAndHosts.Repository;
using ServersAndHosts.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;

namespace TestForServersAndHostWithJSONUnitSimplifier
{
    [TestClass]
    public class Unit_RepositoryMock
    {
        private const string PATH = "C:\\Users\\Admin\\source\\repos\\ServersAndHosts\\Tests\\TestForServersAndHostWithJSONUnitSimplifier\\JSON\\";

        [TestMethod]
        public void TestWithLib()
        {
            TestByJSON.TestLayeredService<server, RepositoryMock <server>> (
                File.ReadAllText(PATH + "ReposMock.json"),
                new RepositoryMock<server>(),
                (a, b) => a.Add(b),
                (repos, dataset) => {});
        }

        [TestMethod]
        public void TestWithoutOfLib()
        {
            RepositoryMock<server> repository = new RepositoryMock<server>();

            server[] servers = new server[]
            {
                new server
                {
                    address = "192.168.3.73",
                    name_in_network = "comp",
                    ram_total_mb = 1024,
                    cpu_frequency_mhz = 4096
                },
                new server
                {
                    address = "192.168.3.74",
                    name_in_network = "servak",
                    ram_total_mb = 2048,
                    cpu_frequency_mhz = 4096
                },
                new server
                {
                    address = "192.168.3.80",
                    name_in_network = "nout",
                    ram_total_mb = 3072,
                    cpu_frequency_mhz = 4096
                },
                new server
                {
                    address = "192.168.3.81",
                    name_in_network = "nout2",
                    ram_total_mb = 4096,
                    cpu_frequency_mhz = 4096
                },
            };

            foreach (var server in servers)
            {
                repository.Add(server);
            }

            Assert.IsTrue(repository.GetById(2) != null);

            Assert.AreEqual(0, servers[0].id);
            Assert.AreEqual(1, servers[1].id);
            Assert.AreEqual(2, servers[2].id);

            Assert.AreEqual(3, repository.Add(servers[1]));
            Assert.AreEqual(4, repository.Add(servers[2]));
            Assert.AreEqual(5, repository.Add(servers[3]));

            repository.Delete(5);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => repository.Delete(5));
        }
    }
}