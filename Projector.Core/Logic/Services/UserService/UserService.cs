using Microsoft.IdentityModel.Tokens;
using Projector.Core.Logic.Handlers.UserHandler;
using Projector.Core.Logic.Interfaces;
using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.Users;
using Projector.Core.Logic.Utilities;
using Projector.Core.Models;
using Projector.Core.Models.DTOs;
using Projector.Core.Models.Enums;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private User User;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> DeleteUser(Guid deleteUser, Guid user)
        {
            var removeUser = await _userRepository.FindUser(deleteUser);

            if (removeUser.UserRole == UserRoleEnum.Admin)
                throw new Exception("You cannot delete the Admin");
            if (removeUser.Id == user)
                throw new Exception("You can remove your own User");

            await _userRepository.RemoveUser(deleteUser);

            return true;
        }

        public AuthenticateResponse GenerateToken(User user, Guid? counterId = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler
            {

            };
            var key = Encoding.ASCII.GetBytes("secret");
            var tokenId = Guid.NewGuid().ToString();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "secret",
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Role, user.UserRole.ToString()),
                    new Claim("TokenId",tokenId),
                }),
                Expires = DateTime.Now.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                IssuedAt = DateTime.Now
            };


            if (counterId != null)
                tokenDescriptor.Subject.AddClaim(new Claim("LogedOnCounter", counterId.Value.ToString(), nameof(Guid)));

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AuthenticateResponse
            {
                AccessToken = tokenHandler.WriteToken(token),
                TokenId = tokenId
            };
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await _userRepository.FindByName(username);

            if (user == null)
                throw new Exception("No such User!");

            if (!PasswordHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Wrong Password!");
                
            return user;
        }

        public User Create(CreateUserRequest request, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required");

            if (_userRepository.FindByName(request.UserName) == null) 
                throw new Exception("Username \"" + request.UserName + "\" is already taken");

            PasswordHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            User user = new User(request);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return user;
        }

        public async Task<User> Update(User userParam, string password = null)
        {
            return null;
        }

        public async Task<string> ChangePassword(Guid userId, string newPassword, string oldPassword)
        {
            var user = await _userRepository.FindUser(userId);
            
            if (user == null)
                throw new Exception("The user does not exist!");

            if (!PasswordHelper.VerifyPasswordHash(oldPassword, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Wrong password!");

            PasswordHelper.CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var tokenResponse = GenerateToken(user);
            user.Token = tokenResponse.TokenId;

            _userRepository.Update(user);

            return tokenResponse.AccessToken;

        }

        public User GetLogedInUser()
        {
            if (User == null)
                return new User() { Id = Guid.Parse("D5C7E9B2-6CFF-44CE-936C-419DBD48B0BE") };
            return User;
        }

        public void SetUser(User user)
        {
            User = user;
        }

        public AuthenticateResponse GenerateToken(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUser(BaseIdentifyRequest<Guid> request)
        {
            return await _userRepository.GetById(request.Id);
        }
    }
}