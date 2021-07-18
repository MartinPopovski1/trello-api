using Projector.Core.Logic.Exceptions;
using Projector.Core.Logic.Exceptions.ExtensionMethods;
using Projector.Core.Logic.Handlers.TicketHandler;
using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.Ticket;
using Projector.Core.Models;
using Projector.Repositories.SqlClientRepositories.RowRecordParsers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Projector.Repositories.SqlClientRepositories
{
    public class TicketRepository : ITicketRepository
    {
        string _connString;
        public TicketRepository(string connectionString)
        {
            _connString = connectionString;
        }
        public async Task<Ticket> GetById(BaseIdentifyRequest<Guid> request)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();
                string sqlCommand = $@"SELECT Id AS TicketId, Name AS TicketName, CreatedAt AS TicketCreatedAt, Deleted AS TicketDeleted, TicketListId FROM Ticket
                                       WHERE Id = '{request.Id}'";

                var query = connection.CreateCommand();
                query.CommandText = sqlCommand;

                var rowRecords = await query.ExecuteReaderAsync();
                Ticket ticket = null;
                while (rowRecords.Read())
                {
                    ticket = RowRecordParser.ParseTicket(rowRecords);
                    ticket.Deleted = rowRecords["TicketDeleted"].ToString() == "1" ? true : false;
                }
                connection.Close();
                return ticket;
            }
        }
        public async Task<Ticket> CreateTicket(Ticket ticket)
        {
            using(SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();
                string sqlCommand = $@"INSERT INTO Ticket (Id,Name,CreatedAt,TicketListId)
                                        VALUES ('{ticket.Id}','{ticket.Name}','{ticket.CreatedAt}','{ticket.TicketListId}')";

                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;

                await command.ExecuteNonQueryAsync();
                connection.Close();
                return ticket;
            }
        }
        public async Task Delete(BaseIdentifyRequest<Guid> request)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();
                string sqlCommand = $@"UPDATE Ticket SET Deleted = 1 WHERE Id = '{request.Id}'";

                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;

                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }
        public async Task EditTicket(EditTicketRequest request)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();
                string sqlCommand = $@"UPDATE Ticket SET Name = '{request.Name}' , Description = '{request.Description}' WHERE Id = '{request.Id}'";

                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;

                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }
        public async  Task SetDescription(Ticket ticket, Guid ticketId)
        {
            using(SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();

                string sqlCommand = $@"UPDATE Ticket SET Description = '{ticket.Description}' WHERE Id = '{ticketId}'";

                var command = connection.CreateCommand();
                
                command.CommandText = sqlCommand;

                await command.ExecuteNonQueryAsync();
                
                connection.Close();
            }
        }
        public async Task<bool> NameTaken(Guid ticketListId , string name)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();
                string sqlCommand = $@"SELECT * From Ticket t
                                        WHERE t.TicketListId = '{ticketListId}' AND t.Name = '{name}' AND t.Deleted != 1";

                var query = connection.CreateCommand();
                query.CommandText = sqlCommand;

                var rowRecords = await query.ExecuteReaderAsync();

                if (rowRecords.HasRows)
                {
                    connection.Close();
                    return true;
                }
                connection.Close();
                return false;
            }
        }
        public async Task AssignUsers(Guid ticketId, List<Guid> userIds)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {

                connection.Open();
                string sqlCommand = "";
                var command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction("Assigning Users to Ticket");
                command.Connection = connection;
                command.Transaction = transaction;
                if (userIds.Count > 0)
                {
                    try
                    {
                        foreach (var item in userIds)
                        {
                            sqlCommand += $@"INSERT INTO UserTickets ( Id , UserId , TicketId , Deleted )
                                                 VALUES ( NEWID() , '{item}' , '{ticketId}' , 0 ); ";
                            
                        }

                        command.CommandText = sqlCommand;
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


                connection.Close();
            }
        }
        public async Task RemoveUsersFromTicket(Guid ticketId, List<Guid> userId)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {

                connection.Open();

                var command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction("Assigning Users to Ticket");
                command.Connection = connection;
                command.Transaction = transaction;
                if (userId.Count > 0)
                {
                    try
                    {
                        foreach (var item in userId)
                        {
                            command.CommandText = $@"DELETE FROM UserTickets
                                                     WHERE UserId = '{item}' AND TicketId = '{ticketId}'";
                            await command.ExecuteNonQueryAsync();
                        }
                        transaction.Commit();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        var validationResult = ex.BuildValidationResult(System.Net.HttpStatusCode.InternalServerError);
                        throw new TechnicalException(validationResult);
                    }
                }


                connection.Close();
            }
        }
        public async Task ChangeParrentTicketList(Guid ticketListId,Guid ticketId)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("Ticket switch");
                string sqlCommand = $@"UPDATE Ticket SET TicketListId = '{ticketListId}' WHERE Id = '{ticketId}' ;
                                       UPDATE UserTickets SET Deleted = 1 WHERE TicketId = '{ticketId}' ;
                                       INSERT INTO UserTickets ( Id, UserId, TicketId, Deleted )
                                         VALUES ( NEWID(), '(SELECT DefaultAssignedUser FROM TicketList WHERE Id = '{ticketListId}')' , '{ticketId}' , 0 )";

                var command = connection.CreateCommand();
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
}