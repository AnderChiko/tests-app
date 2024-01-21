using System.Xml;
using Test.Context;
using Test.Context.Extensions;
using Test.Context.Infrastructure;
using Test.Context.Repositories.Interfaces;

namespace Test.Context.Repositories.Implementations
{
    public class TasksRepository : ITasksRepository
    {
        private readonly TestContext _dbContext;
        private readonly IDbContextGenerator _contextGenerator;

        public TasksRepository(IDbContextGenerator contextGenerator)
        {
            _contextGenerator = contextGenerator;
        }

        public async Task<List<Tasks>> GetAllAsync()
        {
            var configuration = await _contextGenerator.GenerateMyDbContext().Tasks.ToListAsyncSafe<Tasks>();

            return configuration;
        }

        public async Task<Tasks?> GetByIdAsync(int id)
        {
            var configuration = await Task.Run(() =>
            {
                return _contextGenerator.GenerateMyDbContext()
                .Tasks
                .Where(x => x.Id == id)
                .FirstOrDefault();
            });
            return configuration;
        }

        public async Task<Tasks> AddAsync(Tasks item)
        {
            using (var context = _contextGenerator.GenerateMyDbContext())
            {
                var entityToUpdate = await GetByIdAsync(item.Id);
                if (entityToUpdate == null)
                {
                    entityToUpdate = new Tasks();
                    context.Tasks.Add(entityToUpdate);
                    context.SaveChanges();
                }
            }
            return item;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var context = _contextGenerator.GenerateMyDbContext())
            {
                var entityToUpdate = await GetByIdAsync(id);
                if (entityToUpdate == null)
                {
                    entityToUpdate = new Tasks();
                    context.Tasks.Remove(entityToUpdate);
                    context.SaveChanges();
                }
            }
            return true;
        }

        public List<Tasks> GetAll()
        {
            var configuration = _contextGenerator.GenerateMyDbContext().Tasks.ToList();

            return configuration;
        }



        public async Task<Tasks> UpdateAsync(Tasks item)
        {
            var dbSet = _dbContext.Set<Tasks>();
            dbSet.UpdateRange(item);

            var result = await _dbContext.SaveChangesAsync();

            return item;
        }

    }
}
