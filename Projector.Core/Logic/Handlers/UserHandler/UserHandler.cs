using Projector.Core.Logic.Exceptions;
using Projector.Core.Logic.Interfaces;
using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.Users;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Handlers.UserHandler
{
    public class UserHandler : IUserHandler
    {
        IUserRepository _repository;
        IUserService _userService;
        IUserValidator _validator;
        public UserHandler(IUserRepository repository, IUserService userService , IUserValidator validator)
        {
            _repository = repository;
            _userService = userService;
            _validator = validator;
        }

        public async Task<string> ChangePassword(BaseIdentifyRequest<Guid> request, string oldPassword, string newPassword)
        {
            var result = await _userService.ChangePassword(request.Id, newPassword, oldPassword);
            return result;
        }

        public async Task CreateUser(CreateUserRequest request, string password)
        {
            var userNameValid = await _validator.ValidateUserName(request.UserName);
            var passwordValid = _validator.ValidatePassword(password);

            if (!userNameValid.IsSuccessful)
                throw new BusinessException(userNameValid);
            if (!passwordValid.IsSuccessful)
                throw new BusinessException(passwordValid);

            var newUser = _userService.Create(request, password);
            await _repository.CreateUser(newUser);
        }

        public async Task<User> FindByName(string userName)
        {
            return await _repository.FindByName(userName);
        }

        public async Task<User> FindUser(BaseIdentifyRequest<Guid> request)
        {
            return await _repository.FindUser(request.Id);
        }

        public async  Task<List<User>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<List<Privilege>> GetUserPrivileges(BaseIdentifyRequest<Guid> request)
        {
            return await _repository.GetUserPrivileges(request.Id);
        }

        public async Task RemoveUser(BaseIdentifyRequest<Guid> request)
        {
            await _repository.RemoveUser(request.Id);
        }
        public async Task SetPrivileges(User user)
        {
            await _repository.SetPrivileges(user);
        }

        public void Update(User user)
        {
            _repository.Update(user);
        }

        public async Task<User> UpdateInfo(EditUserRequest request)
        {
            var validationResult = await _validator.ValidateUserName(request.UserName);

            if (!validationResult.IsSuccessful)
                throw new BusinessException(validationResult);

            var user =  await _repository.GetById(request.Id);

            if(request.UserName != null)
                user.UserName = request.UserName;
            if (user.FirstName != null)
                user.FirstName = request.FirstName;
            if (user.LastName != null)
                user.LastName = request.LastName;
            if (user.Privileges != null)
                user.Privileges = request.Privileges;

            _repository.Update(user);
            return user;
        }
    }
}
