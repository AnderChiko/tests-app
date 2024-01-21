
using AutoMapper;
using Test.BusinessLogic.Interfaces;
using Test.BusinessLogic.Models;
using Test.Context.Repositories.Interfaces;
using Test.Core.Models.Data;

namespace Test.BusinessLogic
{
    public class TasksManager : ITasksManager
    {
        private readonly ITasksRepository _tasksRepository;
        private readonly IMapper _mapper;
        public TasksManager(ITasksRepository tasksRepository, IMapper mapper)
        {
            _tasksRepository = tasksRepository;
            _mapper = mapper;
        }

        public async Task<DataResult<List<Tasks>>> Get()
        {
            var allItems = await _tasksRepository.GetAllAsync();

            return new DataResult<List<Tasks>>(_mapper.Map<List<Tasks>>(allItems));
        }

        public async Task<DataResult<Tasks>> Get(int id)
        {
            var allItems = await _tasksRepository.GetByIdAsync(id);

            return new DataResult<Tasks>(_mapper.Map<Tasks>(allItems));
        }


        public Task<DataResult<Tasks>> Create(Tasks entry)
        {

            throw new NotImplementedException();
        }

        public Task<DataResult<Result>> Delete(int id)
        {
            throw new NotImplementedException();
        }

      
       

        public async Task<DataResult<Tasks>> Update(Tasks entry)
        {
            var item = await _tasksRepository.UpdateAsync(_mapper.Map<Context.Tasks>(entry));

            return new DataResult<Tasks>(_mapper.Map<Models.Tasks>(item));
        }

    }
}