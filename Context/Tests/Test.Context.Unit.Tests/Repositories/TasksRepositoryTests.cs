using Moq;
using Test.Context.Infrastructure;
using Test.Context.Unit.Tests.Extensions;
using Test.Context.Repositories.Implementations;

namespace Test.Context.Unit.Tests.Repositories
{

    public class TasksRepositoryTests 
    {
        IDbContextGenerator contextGenerator;
        List<Tasks> tasksList;

        public TasksRepositoryTests()
        {
            tasksList = new List<Tasks>();
            tasksList.AddRange(getInitialDbEntities());

            var myDbMoq = new Mock<ITestContext>();
            myDbMoq.Setup(p => p.Tasks).Returns(DbContextMock.GetQueryableMockDbSet<Tasks>(tasksList));
            myDbMoq.Setup(p => p.SaveChanges()).Returns(3);

            var moq = new Mock<IDbContextGenerator>();
            moq.Setup(p => p.GenerateMyDbContext()).Returns(myDbMoq.Object);

            contextGenerator = moq.Object;
        }

        private List<Tasks> getInitialDbEntities()
        {
            return new List<Tasks>()
             {
                new Tasks {Id = 1, Title="Test1", Description="Test1 Description"},
                new Tasks {Id = 2, Title="Test2", Description="Test2 Description"},
                new Tasks {Id = 3, Title="Test3", Description="Test3 Description"},
            };
        }

        [Fact]
        public async void GetAllTasksAsync_WhenCalled_ReturnsTasksAsync()
        {

            var manager = new TasksRepository(contextGenerator);

            var entities = await manager.GetAllAsync();

            Assert.NotNull(entities);
            Assert.Equal(3, entities.Count());
        }

        [Fact]
        public async void Task_GetTasksById_Return_TaskResult()
        {

            var manager = new TasksRepository(contextGenerator);

            var entity = await manager.GetByIdAsync(1);

            Assert.NotNull(entity);
            Assert.Equal(1, entity?.Id);
        }

        [Fact]
        public async void Task_Create_New_Task_Return_SaveTask()
        {
            var manager = new TasksRepository(contextGenerator);

            var entity =  new Tasks() { 
                Id = 4,
                Title = "New Task",
                Description = "newly added"
            };

            var addedEntity = await manager.AddAsync(entity);

            Assert.NotNull(entity);
            Assert.Equal(4, entity?.Id);
        }
    }
}
