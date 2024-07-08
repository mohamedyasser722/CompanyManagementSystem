using CompanyManagementSystem.BLL.Interfaces;
using CompanyManagementSystem.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagementSystem.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly CompanyManagementSystemDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(CompanyManagementSystemDbContext context) 
        { 
            _context = context;
            _dbSet = context.Set<T>();
        }


        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
           
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id)!;
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            
        }

        public void Detach(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }   
    }
}
