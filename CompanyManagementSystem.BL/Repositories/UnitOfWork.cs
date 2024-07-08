using CompanyManagementSystem.BLL.Interfaces;
using CompanyManagementSystem.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagementSystem.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {


        private readonly CompanyManagementSystemDbContext _dbContext;

        private IEmployeeRepository _EmployeeRepository;

        private IDepartmentRepository _DepartmentRepository;

        public UnitOfWork(CompanyManagementSystemDbContext dbContext)   // Ask CLR to inject the DbContext
        {
            _dbContext = dbContext;
        }
        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                if (_EmployeeRepository == null)
                {
                    _EmployeeRepository = new EmployeeRepository(_dbContext);
                }
                return _EmployeeRepository;
            }
        }

        public IDepartmentRepository DepartmentRepository
        {
            get
            {
                if (_DepartmentRepository == null)
                {
                    _DepartmentRepository = new DepartmentRepository(_dbContext);
                }
                return _DepartmentRepository;
            }
        }


        public void Dispose()
        {
            _dbContext.Dispose();
        }
        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
