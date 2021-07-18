using Projector.Core.Logic.Handlers.TicketListHandler;
using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.TicketList;
using Projector.Core.Models;
using Projector.Repositories.SqlClientRepositories.RowRecordParsers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Projector.Repositories.SqlClientRepositories
{
    public class TicketListRepository : ITicketListRepository
    {
        private string _connString;
        public TicketListRepository(string connectionString) 
        {
            _connString = connectionString;
        }

        public async Task CreateTicketList(TicketList ticketList)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {

                connection.Open();

                string sqlCommand = $@"INSERT INTO TicketList (Id,Name,CreatedAt,BoardId,DefaultAssignedUser)
                                        VALUES ('{ticketList.Id}','{ticketList.Name}','{ticketList.CreatedAt}','{ticketList.BoardId}' , '{ticketList.DefaultAssignedUser}')";

                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;
                await command.ExecuteNonQueryAsync();

                connection.Close();
            }
        }

        public async Task Delete(BaseIdentifyRequest<Guid> request)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {

                connection.Open();

                string sqlCommand = $@"UPDATE TicketList SET Deleted = 1 WHERE Id = '{request.Id}'";

                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;
                await command.ExecuteNonQueryAsync();

                connection.Close();
            }
        }

        public async Task EditTicketList(EditTicketListRequest request)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {

                connection.Open();

                string sqlCommand = $@"UPDATE TicketList SET Name = '{request.Name}' , DefaultAssignedUser = '{request.DefaultAssignedUser}' 
                                        WHERE Id = '{request.Id}'";

                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;
                await command.ExecuteNonQueryAsync();

                connection.Close();
            }
        }

        public async Task<List<TicketList>> GetAll(BaseIdentifyRequest<Guid> request)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                

                string sqlCommand = $@"SELECT * FROM TicketList WHERE BoardId = '{request.Id}' AND Deleted != 1";
                
                connection.Open();

                var query = connection.CreateCommand();
                query.CommandText = sqlCommand;

                var rowRecords = await query.ExecuteReaderAsync();

                List<TicketList> ticketLists = new List<TicketList>();
                
                while(rowRecords.Read())
                {
                    ticketLists.Add(RowRecordParser.ParseTicketList(rowRecords));
                }
                return ticketLists;
            }
        }

        public async Task<TicketList> GetById(Guid ticketListId)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                
                  string  sqlCommand = $@"SELECT list.Id AS TicketListId, list.Name AS TicketListName, list.CreatedAt AS ListCreatedAt, list.BoardId AS BoardId,
                                            tic.Id AS TicketId, tic.Name AS TicketName, tic.CreatedAt AS TicketCreatedAt 
                                            FROM TicketList list 
                                            LEFT JOIN (SELECT * FROM Ticket WHERE Deleted != 1) AS tic
                                            ON list.Id = tic.TicketListId
                                            WHERE list.Id = '{ticketListId}' AND list.Deleted != 1
                                            ORDER BY tic.CreatedAt ASC";
                

                connection.Open();

                var query = connection.CreateCommand();
                query.CommandText = sqlCommand;

                var rowRecords = await query.ExecuteReaderAsync();

                var ticketList = new TicketList();
                var tempId = Guid.NewGuid();
                
                while (rowRecords.Read())
                {
                    if (tempId != Guid.Parse(rowRecords["TicketListId"].ToString()))
                        ticketList = RowRecordParser.ParseTicketList(rowRecords);
                    
                    ticketList.Tickets.Add(RowRecordParser.ParseTicket(rowRecords));

                    tempId = ticketList.Id;
                }
                connection.Close();
                return ticketList;
            }
        }

        public async Task<bool> HasActiveTickets(Guid ticketListId)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {

                connection.Open();

                string sqlCommand = $@"SELECT * FROM Ticket t
                                        WHERE t.TicketListId = '{ticketListId}' AND t.Deleted != 1";

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

        public async Task<bool> NameTaken(Guid boardId, string name)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {

                connection.Open();

                var sqlCommand = $"SELECT Name FROM TicketList t WHERE t.Name = '{name}' AND b.ProjectId = '{boardId}' AND t.Deleted != 1";

                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;

                var rowRecords = await command.ExecuteReaderAsync();

                if (rowRecords.HasRows)
                {
                    connection.Close();
                    return true;
                }

                connection.Close();
                return false;
            }
        }

        public async Task AssignDefaultUser(Guid ticketListId, Guid userId)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {

                connection.Open();

                var sqlCommand = $@"UPDATE TicketList
                                    SET DefaultAssignedUser = '{userId}'
                                    WHERE Id = '{ticketListId}'";

                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;

                await command.ExecuteNonQueryAsync();

                connection.Close();
            }
        }
    }
}