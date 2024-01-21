using Test.Context.Extensions;
using Test.Context.Infrastructure;
using Test.Context.Repositories.Interfaces;

namespace Test.Context.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly TestContext _dbContext;
        private readonly IDbContextGenerator _contextGenerator;

        public UserRepository(IDbContextGenerator contextGenerator)
        {
            _contextGenerator = contextGenerator;
        }

        public async Task<List<User>> GetAllAsync()
        {
            var configuration = await _contextGenerator.GenerateMyDbContext().User.ToListAsyncSafe<User>();

            return configuration;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var configuration = await Task.Run(() =>
            {
                return _contextGenerator.GenerateMyDbContext()
                .User
                .Where(x => x.Id == id)
                .FirstOrDefault();
            });
            return configuration;
        }

        public async Task<User> AddAsync(User item)
        {
            using (var context = _contextGenerator.GenerateMyDbContext())
            {
                var entityToUpdate = await GetByIdAsync(item.Id);
                if (entityToUpdate == null)
                {
                    entityToUpdate = new User();
                    context.User.Add(entityToUpdate);
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
                    entityToUpdate = new User();
                    context.User.Remove(entityToUpdate);
                    context.SaveChanges();
                }
            }
            return true;
        }

        public async Task<User> UpdateAsync(User item)
        {
            using (var context = _contextGenerator.GenerateMyDbContext())
            {
                var entityToUpdate = await GetByIdAsync(item.Id);
                if (entityToUpdate == null)
                {
                    entityToUpdate = new User();
                    context.User.Update(entityToUpdate);
                    context.SaveChanges();
                }
            };
            return item;
        }

    }
}
