using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServersAndHosts.Service
{
    public class ServiceResult<T>
    {
        public T Result { get; set; }
        public Exception Exception { get; set; }
    }
}
