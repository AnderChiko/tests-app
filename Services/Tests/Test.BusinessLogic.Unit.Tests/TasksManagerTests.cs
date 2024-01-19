using AutoMapper;
using Moq;
using Test.BusinessLogic.Interfaces;
using Test.Context;
using Test.Core.Testing;

namespace Test.BusinessLogic.Unit.Tests
{

    public class TasksManagerTests : TestHarnessBase
    {
        private readonly ITasksManager  _mockTasksManager ;
        protected readonly Mock<IServiceProvider> _mockServiceProvider;
        protected Mock<IMapper> _mockMapper;

        public TasksManagerTests() : base()
        {
          
            ReBuildServices();

            _mockMapper = new Mock<IMapper>();
            _mockServiceProvider = new Mock<IServiceProvider>();    
            _mockTasksManager = new TasksManager(_mockServiceProvider.Object, _mockMapper.Object);
        }

        private Tasks[] getInitialDbEntities()
        {
            return new Tasks[]
             {
                new Tasks {Id = 1, Title="Test1", Description="Test1 Description"},
                new Tasks {Id = 2, Title="Test2", Description="Test2 Description"},
                new Tasks {Id = 3, Title="Test3", Description="Test3 Description"},
            };
        }

        [Fact]
        public async void Task_GetTasksById_Return_OkResult()
        {
           
        }

        [Fact]
        public async void Task_GetTasksById_Return_NotFoundResult()
        {
           
        }

        [Fact]
        public async void Task_GetTasksById_Return_BadRequestResult()
        {
          
        }

        [Fact]
        public async void Task_GetTasksById_MatchResult()
        {
        }

        [Fact]
        public async void Task_GetPosts_Return_OkResult()
        {
            
        }

        [Fact]
        public void Task_GetTasks_Return_BadRequestResult()
        {
           
        }

        [Fact]
        public async void Task_GetTasks_MatchResult()
        {
            
        }
    }
}
