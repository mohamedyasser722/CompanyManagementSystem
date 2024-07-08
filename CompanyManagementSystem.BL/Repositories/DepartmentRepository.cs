using CompanyManagementSystem.BLL.Interfaces;
using CompanyManagementSystem.DAL.Context;
using CompanyManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagementSystem.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
       
        public DepartmentRepository(CompanyManagementSystemDbContext context) : base(context) { }


    }
}
