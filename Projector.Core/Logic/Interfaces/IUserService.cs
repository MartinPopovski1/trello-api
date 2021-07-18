using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.Users;
using Projector.Core.Models;
using Projector.Core.Models.DTOs;
using System;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Interfaces
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        User GetLogedInUser();

        Task<User> GetUser(BaseIdentifyRequest<Guid> id);
        void SetUser(User user);

        AuthenticateResponse GenerateToken(User user);

        User Create(CreateUserRequest user, string password);
        Task<bool> DeleteUser(Guid deleteUser, Guid user);
        Task<User> Update(User user, string password = null);

        Task<string> ChangePassword(Guid userId, string newPassword, string oldPassword);
    }
}