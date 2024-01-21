using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Context.Repositories.Interfaces
{
    public interface IRepository<T, TKey>
        where T : class, new()
        where TKey : IEquatable<TKey>
    {

        Task<List<T>> GetAllAsync();

        Task<T?> GetByIdAsync(TKey id);

        Task<T> AddAsync(T item);

        Task<T> UpdateAsync(T item);

        Task<bool> DeleteAsync(TKey id);
    }
}
