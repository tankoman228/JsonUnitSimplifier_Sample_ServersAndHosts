using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServersAndHosts.Other
{
    /// <summary>
    /// Для более удобной работы с асинхронными задачами, чтобы не терять исключения
    /// </summary>
    public class AsyncManager
    {
        private string where;
        private Action<string> error;
        public string Where { get { return where; } }

        public AsyncManager(string where, Action<string> error) {
            this.where = where;
            this.error = error;
        }

        /// <summary>
        /// Оборачивает в try catch и выполняет задачу в отдельном потоке
        /// </summary>
        public void TryAsyncOrReturnError(Action action)
        {
            Task.Run(() =>
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    error($"Error in {where}: {error}\n{ex.Message}\n{ex.InnerException?.Message}");
                    //throw ex;
                }
            });
        }
    }
}
