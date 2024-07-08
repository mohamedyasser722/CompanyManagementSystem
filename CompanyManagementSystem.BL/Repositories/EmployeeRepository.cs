using CompanyManagementSystem.BLL.Interfaces;
using CompanyManagementSystem.DAL.Context;
using CompanyManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagementSystem.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly CompanyManagementSystemDbContext _context;
        public EmployeeRepository(CompanyManagementSystemDbContext context) :  base(context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<Employee>> GetAllAsync(params Expression<Func<Employee, object>>[] includes)
        {
            IQueryable<Employee> query = _context.Set<Employee>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }
 

        public async Task<Employee> GetByIdAsync(int id, params Expression<Func<Employee, object>>[] includes)
        {
            IQueryable<Employee> query = _context.Set<Employee>();
            if(includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        

      
    }
}
