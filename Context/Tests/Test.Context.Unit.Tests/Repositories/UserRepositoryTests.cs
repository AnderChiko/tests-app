using Moq;
using Test.Context.Infrastructure;
using Test.Context.Unit.Tests.Extensions;
using Test.Context.Repositories.Implementations;

namespace Test.Context.Unit.Tests.Repositories
{

    public class UserRepositoryTests 
    {
        IDbContextGenerator contextGenerator;
        List<User> usersList;

        public UserRepositoryTests()
        {
            usersList = new List<User>();
            usersList.AddRange(getInitialDbEntities());

            var myDbMoq = new Mock<ITestContext>();
            myDbMoq.Setup(p => p.User).Returns(DbContextMock.GetQueryableMockDbSet<User>(usersList));
            myDbMoq.Setup(p => p.SaveChanges()).Returns(3);

            var moq = new Mock<IDbContextGenerator>();
            moq.Setup(p => p.GenerateMyDbContext()).Returns(myDbMoq.Object);

            contextGenerator = moq.Object;
        }

        private List<User> getInitialDbEntities()
        {
            return new List<User>()
             {
                new User {Id = 1, Username="Test1", Password="Test1"},
                new User {Id = 2, Username="Test2", Password="Test2"},
                new User {Id = 3, Username="Test3", Password="Test3"},
            };
        }

        [Fact]
        public async void GetAllUsersAsync_WhenCalled_ReturnsTasksAsync()
        {

            var manager = new UserRepository(contextGenerator);

            var entities = await manager.GetAllAsync();

            Assert.NotNull(entities);
            Assert.Equal(3, entities.Count());
        }

        [Fact]
        public async void Task_GetUsersById_Return_TaskResult()
        {

            var manager = new UserRepository(contextGenerator);

            var entity = await manager.GetByIdAsync(1);

            Assert.NotNull(entity);
            Assert.Equal(1, entity?.Id);
        }

        [Fact]
        public async void Task_Create_New_User_Return_SaveUser()
        {
            var manager = new UserRepository(contextGenerator);

            var entity =  new User() { 
                Id = 4,
                Username = "NewUser",
                Password = "newlyadded"
            };

            var addedEntity = await manager.AddAsync(entity);

            Assert.NotNull(entity);
            Assert.Equal(4, entity?.Id);
        }
    }
}
