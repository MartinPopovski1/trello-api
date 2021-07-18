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
    public interface IUserHandler
    {
        Task CreateUser(CreateUserRequest request, string password);
        Task<User> FindByName(string firstName);
        Task<User> FindUser(BaseIdentifyRequest<Guid> request);
        Task<List<User>> GetAll();
        Task RemoveUser(BaseIdentifyRequest<Guid> request);
        Task<string> ChangePassword(BaseIdentifyRequest<Guid> request, string oldPassword, string newPassword);
        Task SetPrivileges(User user);
        void Update(User user);
        Task<List<Privilege>> GetUserPrivileges(BaseIdentifyRequest<Guid> request);
        Task<User> UpdateInfo( EditUserRequest userInfo);
    }
}
