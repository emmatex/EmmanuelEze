using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> ListAllAsync();
        bool IsExist(Func<T, bool> filter);
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
