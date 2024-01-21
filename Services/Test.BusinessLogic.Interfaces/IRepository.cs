using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Models.Data;

namespace Test.BusinessLogic.Interfaces
{
    public interface IRepository<T,TKey>
        where T : class, new()
        where TKey : IEquatable<TKey>
    {
        List<T> GetAll();

        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(TKey id);
  
        Task<T> AddAsync(T item);

        Task<T> UpdateAsync(T item);
        Task<Result> DeleteAsync(T item);
    }
}
