using Projector.Core.Logic.Requests.Users;
using Projector.Core.Models.Enums;
using System;
using System.Collections.Generic;

namespace Projector.Core.Models
{
    public class User : BaseModel
    {        
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRoleEnum UserRole { get; set; }
        public string Token { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsActive { get; set; }

        public List<Privilege> Privileges { get; set; }
        public List<UserTickets> UserTickets { get; set; }

        public User(CreateUserRequest request)
        {
            Id = Guid.NewGuid();
            UserName = request.UserName;
            FirstName = request.FirstName;
            LastName = request.LastName;
            Privileges = request.Privileges;
            UserRole = UserRoleEnum.User;
            IsActive = true;
        }
        public User()
        {
            Privileges = new List<Privilege>();
            UserTickets = new List<UserTickets>();
        }
    }
}