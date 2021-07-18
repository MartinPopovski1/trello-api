using Projector.Core.Logic.Exceptions;
using Projector.Core.Logic.Exceptions.ExtensionMethods;
using Projector.Core.Logic.Handlers.UserHandler;
using Projector.Core.Logic.Interfaces;
using Projector.Core.Models;
using Projector.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Projector.Repositories.SqlClientRepositories
{
    public class UserRepository : IUserRepository
    {
        private string _connString;
        public UserRepository(string connectionString)
        {
            _connString = connectionString;
        }
        public async Task<User> CreateUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                if (user.Privileges?.Count == 0)
                {
                    connection.Open();
                    string sqlCommand;
                    var command = connection.CreateCommand();
                    command.Connection = connection;
                    sqlCommand = $@"INSERT INTO [User] ([Id], [UserName], [FirstName], [LastName], [PasswordHash], [PasswordSalt],[UserRole],[IsActive]) 
                                            VALUES ('{user.Id}','{user.UserName}','{user.FirstName}','{user.LastName}',@passwordHash,@passwordSalt,0,1)";
                    command.CommandText = sqlCommand;

                    //if(user.Privileges?.Count > 0)

                    command.Parameters.AddWithValue("passwordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("passwordSalt", user.PasswordSalt);

                    await command.ExecuteNonQueryAsync();

                    connection.Close();
                    return user;
                }
                else
                {
                    connection.Open();
                    string sqlCommand;
                    SqlTransaction transaction = connection.BeginTransaction("Assigning User Privileges");
                    var command = connection.CreateCommand();
                    command.Connection = connection;
                    sqlCommand = $@"INSERT INTO [User] ([Id], [UserName], [FirstName], [LastName], [PasswordHash], [PasswordSalt],[UserRole],[IsActive]) 
                                            VALUES ('{user.Id}','{user.UserName}','{user.FirstName}','{user.LastName}',@passwordHash,@passwordSalt,0,1); ";
                    command.Parameters.AddWithValue("passwordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("passwordSalt", user.PasswordSalt);

                    foreach (var privilege in user.Privileges)
                    {
                        sqlCommand += $@"INSERT INTO UserPrivileges(UserId,PrivilegeId)
                                         VALUES ('{user.Id}', '(SELECT Id FROM Privilege WHERE PrivilegeType = {(int)privilege.PrivilegeType})');";
                    }
                    
                    command.CommandText = sqlCommand;
                    
                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        var validationResult = ex.BuildValidationResult(System.Net.HttpStatusCode.InternalServerError);
                        throw new TechnicalException(validationResult);
                    }


                    connection.Close();
                    return user;
                }
            }
        }
        public async Task<User> FindByName(string userName)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.Connection = connection;

                command.CommandText = $@"SELECT * FROM [User]
                                        WHERE UserName = '{userName}'";

                var rowRecords = await command.ExecuteReaderAsync();
                User user = null;
                
                while(rowRecords.Read())
                {
                    user = new User();
                    user.Id = Guid.Parse(rowRecords["Id"].ToString());
                    user.UserName = rowRecords["UserName"].ToString();
                    user.FirstName = rowRecords["FirstName"].ToString();
                    user.LastName = rowRecords["LastName"].ToString();
                    user.IsActive = rowRecords["IsActive"].ToString() == "1" ? true : false;
                    user.PasswordHash = rowRecords["PasswordHash"] as byte[];
                    user.PasswordSalt = rowRecords["PasswordSalt"] as byte[];
                    
                }

                connection.Close();
                return user;
            }
        }

        public async Task<User> FindUser(Guid userId)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.Connection = connection;

                command.CommandText = $@"SELECT * FROM [User]
                                        WHERE Id = '{userId}'";

                var rowRecords = await command.ExecuteReaderAsync();
                User user = null;

                while (rowRecords.Read())
                {
                    user = new User();
                    user.Id = Guid.Parse(rowRecords["Id"].ToString());
                    user.UserName = rowRecords["UserName"].ToString();
                    user.FirstName = rowRecords["FirstName"].ToString();
                    user.LastName = rowRecords["LastName"].ToString();
                    user.IsActive = rowRecords["IsActive"].ToString() == "1" ? true : false;
                    user.PasswordHash = rowRecords["PasswordHash"] as byte[];
                    user.PasswordSalt = rowRecords["PasswordSalt"] as byte[];

                }

                connection.Close();
                return user;
            }
        }

        public async Task<List<User>> GetAll()
        {

            using(SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.Connection = connection;

                command.CommandText = $@"SELECT * FROM [User]";

                var rowRecords = await command.ExecuteReaderAsync();
                
                var users = new List<User>();
                while (rowRecords.Read())
                {
                    users.Add(new User
                    {
                        Id = Guid.Parse(rowRecords["Id"].ToString()),
                        UserName = rowRecords["UserName"].ToString(),
                        FirstName = rowRecords["FirstName"].ToString(),
                        LastName = rowRecords["LastName"].ToString(),
                        IsActive = rowRecords["IsActive"].ToString() == "1" ? true : false,
                        PasswordHash = rowRecords["PasswordHash"] as byte[],
                        PasswordSalt = rowRecords["PasswordSalt"] as byte[]
                });

                }

                connection.Close();
                return users;
            }
        }

        public async Task<User> GetById(Guid id)
        {
            using(SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.Connection = connection;

                command.CommandText = $@"SELECT * FROM [User]
                                         WHERE Id = '{id}'";
                var rowRecords = await command.ExecuteReaderAsync();
                User user = null;
                while(rowRecords.Read())
                {
                    user = new User
                    { 
                        Id = Guid.Parse(rowRecords["Id"].ToString()),
                        UserName = rowRecords["UserName"].ToString(),
                        FirstName = rowRecords["FirstName"].ToString(),
                        LastName = rowRecords["LastName"].ToString(),
                        IsActive = rowRecords["IsActive"].ToString() == "1" ? true : false,
                        PasswordHash = rowRecords["PasswordHash"] as byte[],
                        PasswordSalt = rowRecords["PasswordSalt"] as byte[]

                    };

                }
                connection.Close();
                return user;
            }
        }

        public async Task RemoveUser(Guid deleteUser)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.Connection = connection;

                command.CommandText = $@"UPDATE [User]
                                         SET IsActive = 0
                                         WHERE Id = '{deleteUser}'";

                var rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    connection.Close();
                    var validationResult = new ValidationResult("There is no User with that ID !", 403);
                    throw new BusinessException(validationResult);
                }
                    
                
                connection.Close();

            }
        }

        public async Task SetPrivileges(User user)
        {
            using(SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();
                string sqlCommand = "";
                SqlTransaction transaction = connection.BeginTransaction("Edit User Privileges");
                var command = connection.CreateCommand();
                command.Connection = connection;
                sqlCommand = $@" DELETE FROM UserPrivileges WHERE UserID = '{user.Id}' ;";
                if(user.Privileges?.Count > 0)
                {
                    foreach (var privilege in user.Privileges)
                    {
                        sqlCommand += $@"INSERT INTO UserPrivileges ( UserId , PrivilegeId )
                                     VALUES ( '{user.Id}' , (SELECT Id FROM Privilege  WHERE PrivilegeType = {(int)privilege.PrivilegeType})) ; ";
                    }

                }
                try
                {
                    await command.ExecuteNonQueryAsync();
                    transaction.Commit();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    var validationResult = ex.BuildValidationResult(HttpStatusCode.InternalServerError);
                    throw new TechnicalException(validationResult);
                }
            }
        }

        public async void Update(User user)
        {
            using(SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction("Update User Info"))
                {
                    var command = connection.CreateCommand();
                    string sqlCommand = "";
                    command.Connection = connection;


                    sqlCommand = $@"UPDATE [User]
                                SET Id = '{user.Id}' , UserName = '{user.UserName}' , FirstName = '{user.FirstName}' , LastName = '{user.LastName}' , PasswordHash = @passwordHash , 
                                PasswordSalt = @passwordSalt , UserRole = {user.UserRole} , IsActive = {user.IsActive}
                                WHERE Id = '{user.Id}'";

                    command.Parameters.AddWithValue("passwordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("passwordSalt", user.PasswordSalt);
                    if (user.Privileges?.Count > 0)
                    {
                        sqlCommand += $@"; DELETE FROM UserPrivileges WHERE UserID = '{user.Id}'; ";
                        foreach (var privilege in user.Privileges)
                        {
                            sqlCommand += $@"INSERT INTO UserPrivileges ( UserId , PrivilegeId )
                                     VALUES ( '{user.Id}' , (SELECT Id FROM Privilege  WHERE PrivilegeType = {(int)privilege.PrivilegeType})) ; ";
                        }
                    }
                    else if(user.Privileges?.Count == 0)
                    {
                        sqlCommand += $@"; DELETE FROM UserPrivileges WHERE UserID = '{user.Id}'; ";
                    }
                    command.CommandText = sqlCommand;
                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        transaction.Commit();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        var validationResult = ex.BuildValidationResult(HttpStatusCode.InternalServerError);
                        throw new TechnicalException(validationResult);
                    }
                }
            }
        }

     

        public async Task<List<Privilege>> GetUserPrivileges(Guid userId)
        {
            using(SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();
                string sqlCommand;
                var query = connection.CreateCommand();
                query.Connection = connection;
                List<Privilege> privileges = new List<Privilege>();
                sqlCommand = $@"SELECT p.Id AS PrivilegeId , p.Name AS PrivilegeName, p.PrivilegeType AS Type FROM [User] u 
                                INNER JOIN
                                UserPrivileges up ON u.Id = up.UserId
                                INNER JOIN Privilege p ON up.PrivilegeId = p.Id
                                WHERE u.Id = '{userId}'";

                query.CommandText = sqlCommand;

                var rowRecords = await query.ExecuteReaderAsync();
                
                while(rowRecords.Read())
                {
                    
                    privileges.Add(new Privilege
                    {
                        Id = Guid.Parse(rowRecords["PrivilegeId"].ToString()),
                        Name = rowRecords["PrivilegeName"].ToString(),
                        PrivilegeType = (PrivilegeEnum)int.Parse(rowRecords["Type"].ToString())
                    });
                }

                connection.Close();
                return privileges;

            }
        }

        public async Task<bool> UserNameTaken(string userName)
        {
            using(SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();
                string sqlCommand = "";
                var query = connection.CreateCommand();
                query.Connection = connection;
                sqlCommand = $@"SELECT Id FROM [User]
                                WHERE UserName = {userName}";

                query.CommandText = sqlCommand;
                var rowRecords = await query.ExecuteReaderAsync();
                if(rowRecords.HasRows)
                {
                    connection.Close();
                    return true;
                }
                connection.Close();
                return false;
                    
            }
        }
    }
}