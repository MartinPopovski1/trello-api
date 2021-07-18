using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Projector.Core.Logic.Handlers.UserHandler
{
    public interface IUserRepository
    {
        
        Task<User> FindByName(string firstName);
        Task<User> FindUser(Guid deleteUser);
        Task<User> CreateUser(User user);
        Task<List<User>> GetAll();
        Task RemoveUser(Guid deleteUser);
        Task<User> GetById(Guid id);
        Task SetPrivileges(User user);
        void Update(User user);
        Task<List<Privilege>> GetUserPrivileges(Guid userId);
        Task<bool> UserNameTaken(string userName);
    }
}