using CompanyManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagementSystem.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {

        Task<IEnumerable<Employee>> GetAllAsync(params Expression<Func<Employee, object>>[] includes);
        Task<Employee> GetByIdAsync(int id, params Expression<Func<Employee, object>>[] includes);
        

        
        

    }
}
