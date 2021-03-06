using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected DataContext _context;
        public GenericRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual async Task Add(T entity)
            => await _context.Set<T>().AddAsync(entity);

        public void Delete(T entity)
           => _context.Set<T>().Remove(entity);

        public virtual async Task<T> GetByIdAsync(Guid id)
            => await _context.Set<T>().FindAsync(id);

        public async Task<IReadOnlyList<T>> ListAllAsync()
            => await _context.Set<T>().ToListAsync();

        public bool IsExist(Func<T, bool> filter)
            => _context.Set<T>().Any(filter);

        public void Update(T entity)
        {
            // no code to implement
        }


    }
}
