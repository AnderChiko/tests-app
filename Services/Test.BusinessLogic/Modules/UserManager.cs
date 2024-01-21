
using AutoMapper;
using Test.BusinessLogic.Interfaces;
using Test.BusinessLogic.Models;
using Test.Context.Repositories.Interfaces;
using Test.Core.Models.Data;

namespace Test.BusinessLogic
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserManager(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<DataResult<List<User>>> Get()
        {
            var allItems = await _userRepository.GetAllAsync();

            return new DataResult<List<User>>(_mapper.Map<List<User>>(allItems));
        }

        public async Task<DataResult<User>> Get(int id)
        {
            var allItems = await _userRepository.GetByIdAsync(id);

            return new DataResult<User>(_mapper.Map<User>(allItems));
        }

        public async Task<DataResult<User>> Create(User entry)
        {
            var item = await _userRepository.AddAsync(_mapper.Map<Context.User>(entry));
            return new DataResult<User>(_mapper.Map<User>(item));
        }

        public async Task<DataResult<Result>> Delete(int id)
        {
            var item = await _userRepository.DeleteAsync(id);
            return new DataResult<Result>(new Result(Status.Success));
        }

        public async Task<DataResult<User>> Update(User entry)
        {
            var item = await _userRepository.UpdateAsync(_mapper.Map<Context.User>(entry));

            return new DataResult<User>(_mapper.Map<Models.User>(item));
        }
    }
}