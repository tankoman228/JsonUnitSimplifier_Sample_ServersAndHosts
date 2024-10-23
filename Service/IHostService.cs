﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServersAndHosts.Entity;
using ServersAndHosts.Repository;

namespace ServersAndHosts.Service
{
    public interface IHostService
    {
        List<host> GetAllHosts();

        List<host> SaveHosts(List<host> newHosts);

        void Delete(host s);

        void Insert(host s);
    }
}
