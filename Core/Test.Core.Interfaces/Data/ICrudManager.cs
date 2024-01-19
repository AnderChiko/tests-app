using Test.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core.Interfaces.Data
{
    public interface ICrudManager<T, TKey>
    {
        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DataResult<T>> Get(TKey id);

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DataResult<List<T>>> Get();

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<DataResult<T>> Update(T entry);

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<DataResult<T>> Create(T entry);

        /// <summary>
        /// Delete by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result> Delete(TKey id);
    }
}
