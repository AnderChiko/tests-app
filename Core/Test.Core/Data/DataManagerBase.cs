using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Test.Core.Interfaces.Data;
using Test.Core.Models.Data;

namespace Test.Core.Data
{
    /// <summary>
    /// Entity framework base class that handles create, read, update, delete operations against a SQL database using Entity Framework
    /// </summary>
    /// <typeparam name="TModel">Model passed in</typeparam>
    /// <typeparam name="TKey">Type of database primary key</typeparam>
    /// <typeparam name="TDbContext">Database context passed in</typeparam>
    public abstract class DataManagerBase<TSourceModel, TModel, TKey, TDbContext> : ICrudManager<TModel, TKey>
        where TDbContext : DbContext
        where TModel : class, new()
        where TSourceModel : class, new()
    {
        protected readonly IServiceProvider _serviceProvider;
        protected MapperConfiguration _mapperConfiguration;
        protected IMapper _mapper;
        protected virtual TDbContext GetDbContext()
        {
            return _serviceProvider.GetRequiredService<TDbContext>();
        }

        protected DataManagerBase(IServiceProvider serviceProvider, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }

        /// <summary>
        /// Create an entry.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public virtual async Task<DataResult<TModel>> Create(TModel entry)
        {
            // map to db entries
            var dbEntry = _mapper.Map<TSourceModel>(entry);

            using (var dbContext = GetDbContext())
            {
                dbContext.Entry(dbEntry).State = EntityState.Added;
                var contextResult = dbContext.Add(dbEntry);
                await dbContext.SaveChangesAsync();

                return new DataResult<TModel>(_mapper.Map<TModel>(contextResult.Entity));
            }
        }

        /// <summary>
        /// Update an entry.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public virtual async Task<DataResult<TModel>> Update(TModel entry)
        {
            // map to db entries
            var dbEntry = _mapper.Map<TSourceModel>(entry);

            using (var dbContext = GetDbContext())
            {
                dbContext.Entry(dbEntry).State = EntityState.Modified;
                var contextResult = dbContext.Update(dbEntry);
                await dbContext.SaveChangesAsync();

                return new DataResult<TModel>(_mapper.Map<TModel>(contextResult.Entity));
            }
        }

        /// <summary>
        /// Delete an entry. Hard delete.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Result> Delete(TKey id)
        {

            using (var dbContext = GetDbContext())
            {
                var dbSet = dbContext.Set<TSourceModel>();
                var entry = await dbSet.FindAsync(id);
                dbContext.Entry(entry).State = EntityState.Deleted;

                await dbContext.SaveChangesAsync();

                return new Result(Status.Success, "Successfully Deleted");
            }
        }

        /// <summary>
        /// Create range: caters for creating a list of entries.
        /// </summary>
        /// <param name="entry">Entry to update which is a List of TModel</param>
        /// <returns>Returns a data result of the entry</returns>
        public virtual async Task<DataResult<List<TModel>>> Create(List<TModel> entries)
        {
            // map to db entries
            var dbEntry = _mapper.Map<List<TSourceModel>>(entries);

            using (var dbContext = GetDbContext())
            {
                var dbSet = dbContext.Set<TSourceModel>();
                dbSet.AddRange(dbEntry);

                await dbContext.SaveChangesAsync();

                var result = new DataResult<List<TModel>>(entries);

                return result;
            }
        }

        /// <summary>
        /// Update range: caters for updating a list of entries.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public virtual async Task<DataResult<List<TModel>>> Update(List<TModel> entries)
        {
            // map to db entries
            var dbEntry = _mapper.Map<List<TSourceModel>>(entries);

            using (var dbContext = GetDbContext())
            {
                var dbSet = dbContext.Set<TSourceModel>();
                dbSet.UpdateRange(dbEntry);

                var result = await dbContext.SaveChangesAsync();

                return new DataResult<List<TModel>>( entries);
            }
        }

        /// <summary>
        /// Delete a range. Caters for deleting a list of entry.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public virtual async Task<Result> Delete(List<TModel> entry)
        {
            // map to db entries
            var dbEntry = _mapper.Map<TSourceModel>(entry);

            using (var dbContext = GetDbContext())
            {
                var dbSet = dbContext.Set<TSourceModel>();
                dbSet.RemoveRange(dbEntry);

                await dbContext.SaveChangesAsync();

                return new Result(Status.Success, "Successfully Deleted");
            }
        }

        /// <summary>
        /// Get by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<DataResult<TModel>> Get(TKey id)
        {
            using (var dbContext = GetDbContext())
            {
                var dbSet = dbContext.Set<TSourceModel>();
                var contextResult = await dbSet.FindAsync(id);

                return new DataResult<TModel>(_mapper.Map<TModel>(contextResult));
            }
        }

        /// <summary>
        /// Get by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<DataResult<List<TModel>>> Get()
        {
            using (var dbContext = GetDbContext())
            {
                var dbSet = dbContext.Set<TSourceModel>();
                var contextResult = await dbSet.ToListAsync();

                return new DataResult<List<TModel>>(_mapper.Map<List<TModel>>(contextResult));
            }
        }

        private TModel SetValue(TModel entry, string propertyName, object value)
        {
            var propertyInfo = entry.GetType().GetProperty(propertyName);

            if (propertyInfo is null)
                return entry;

            var type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;

            if (propertyInfo.PropertyType.IsEnum)
            {
                propertyInfo.SetValue(entry, Enum.Parse(propertyInfo.PropertyType, value.ToString()!));
            }
            else
            {
                var safeValue = (value == null) ? null : Convert.ChangeType(value, type);
                propertyInfo.SetValue(entry, safeValue, null);
            }

            return entry;
        }

    }
}
