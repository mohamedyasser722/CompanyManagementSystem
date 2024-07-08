using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagementSystem.BLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // singnatures of all repositories
        IEmployeeRepository EmployeeRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        public Task<int> CommitAsync();

    }
}
