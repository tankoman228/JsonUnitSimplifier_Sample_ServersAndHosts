using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServersAndHosts.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string include = null);
        T GetById(int id);
        int Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
