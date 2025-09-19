using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.DAL.UOW
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        public Task ExecuteInTransaction(Func<Task> operation);
    }

}
