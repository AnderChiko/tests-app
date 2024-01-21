using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Test.Core.Interfaces.Data;
using Test.Core.Models.Data;

namespace Test.Core.Data
{
    /// <summary>
    /// Entity framework base class that handles create, read, update, delete operations against a SQL database using Entity Framework
    /// </summary>
    /// <typeparam name="TModel">Model passed in</typeparam>
    /// <typeparam name="TKey">Type of database primary key</typeparam>
    public abstract class DataManagerBase<DtoModel, TModel, TKey> : ICrudManager<TModel, TKey>
        where TModel : class, new()
        where DtoModel : class, new()
        where TKey : IEquatable<TKey>
    {
        protected readonly IMapper _mapper;
        protected readonly DbContext _dbContext;

        protected DataManagerBase(DbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }


        public TModel CreateModel()
        {
            return new TModel();
        }

        /// <summary>
        /// Create an entry.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public virtual async Task<DataResult<TModel>> Create(TModel entry)
        {
            // map to db entries
            var dbEntry = _mapper.Map<DtoModel>(entry);

            _dbContext.Entry(dbEntry).State = EntityState.Added;
            var contextResult = _dbContext.Add(dbEntry);
            await _dbContext.SaveChangesAsync();

            return new DataResult<TModel>(_mapper.Map<TModel>(contextResult.Entity));

        }

        /// <summary>
        /// Update an entry.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public virtual async Task<DataResult<TModel>> Update(TModel entry)
        {
            // map to db entries
            var dbEntry = _mapper.Map<DtoModel>(entry);

            _dbContext.Entry(dbEntry).State = EntityState.Modified;
            var contextResult = _dbContext.Update(dbEntry);
            await _dbContext.SaveChangesAsync();

            return new DataResult<TModel>(_mapper.Map<TModel>(contextResult.Entity));
        }

        /// <summary>
        /// Delete an entry. Hard delete.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<DataResult<Result>> Delete(TKey id)
        {

                var dbSet = _dbContext.Set<DtoModel>();
                var entry = await dbSet.FindAsync(id);
               var dbResult = _dbContext.Entry(entry).State = EntityState.Deleted;

                await _dbContext.SaveChangesAsync();

                return new DataResult<Result>(new Result(Status.Success, "Successfully Deleted")) ;
        }

        /// <summary>
        /// Create range: caters for creating a list of entries.
        /// </summary>
        /// <param name="entry">Entry to update which is a List of TModel</param>
        /// <returns>Returns a data result of the entry</returns>
        public virtual async Task<DataResult<List<TModel>>> Create(List<TModel> entries)
        {
            // map to db entries
            var dbEntry = _mapper.Map<List<DtoModel>>(entries);

                var dbSet = _dbContext.Set<DtoModel>();
                dbSet.AddRange(dbEntry);

                await _dbContext.SaveChangesAsync();

                var result = new DataResult<List<TModel>>(entries);

                return result;
        }

        /// <summary>
        /// Update range: caters for updating a list of entries.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public virtual async Task<DataResult<List<TModel>>> Update(List<TModel> entries)
        {
            // map to db entries
            var dbEntry = _mapper.Map<List<DtoModel>>(entries);

                var dbSet = _dbContext.Set<DtoModel>();
                dbSet.UpdateRange(dbEntry);

                var result = await _dbContext.SaveChangesAsync();

                return new DataResult<List<TModel>>(entries);
        }

        /// <summary>
        /// Delete a range. Caters for deleting a list of entry.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public virtual async Task<Result> Delete(List<TModel> entry)
        {
            // map to db entries
            var dbEntry = _mapper.Map<DtoModel>(entry);

                var dbSet = _dbContext.Set<DtoModel>();
                dbSet.RemoveRange(dbEntry);

                await _dbContext.SaveChangesAsync();

                return new Result(Status.Success, "Successfully Deleted");
        }

        /// <summary>
        /// Get by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<DataResult<TModel>> Get(TKey id)
        {
            
                var dbSet = _dbContext.Set<DtoModel>();
                var contextResult = await dbSet.FindAsync(id);

                return new DataResult<TModel>(_mapper.Map<TModel>(contextResult));
        }

        /// <summary>
        /// Get by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<DataResult<List<TModel>>> Get()
        {
           
            var dbSet = _dbContext.Set<DtoModel>();
            var contextResult = await dbSet.ToListAsync();

            return new DataResult<List<TModel>>(_mapper.Map<List<TModel>>(contextResult));
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

        public DataResult<List<TModel>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
