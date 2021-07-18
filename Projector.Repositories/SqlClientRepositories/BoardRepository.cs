using Projector.Core.Logic.Exceptions;
using Projector.Core.Logic.Handlers.BoardHandler;
using Projector.Core.Models;
using Projector.Repositories.SqlClientRepositories.RowRecordParsers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Projector.Repositories.SqlClientRepositories
{
    public class BoardRepository : IBoardRepository
    {
        private string _connString;
        public BoardRepository(string connectionString)
        {
            _connString = connectionString;
        }

        public async Task<List<Board>> GetAll(Guid projectId)
        {
            List<Board> boards = new List<Board>();

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();

                string sqlCommand = $"SELECT * FROM Board b Where b.Deleted <> 1 AND b.ProjectId = '{projectId}' ORDER BY b.CreatedAt ASC";

                var query = connection.CreateCommand();
                query.CommandText = sqlCommand;
                var rowRecords = await query.ExecuteReaderAsync();

                while (rowRecords.Read())
                {
                    boards.Add(RowRecordParser.ParseBoard(rowRecords));
                }
                connection.Close();
                return boards;
            }
        }

     

        public async Task<Board> Get(Guid boardId)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();
               
                string sqlCommand = $@"SELECT b.Id AS BoardId, b.Name AS BoardName, b.CreatedAt AS BoardCreatedAt, b.ProjectId AS ProjectId,
                                                t.Id AS TicketListId, t.Name AS TicketListName, t.CreatedAt AS TicketListCreatedAt, 
                                                tic.Id AS TicketId, tic.Name AS TicketName, tic.CreatedAt AS TicketCreatedAt
                                        FROM  Board b
                                        LEFT JOIN (SELECT * FROM TicketList WHERE Deleted != 1 ) AS t ON b.id = t.BoardId
                                        LEFT JOIN (SELECT * FROM Ticket WHERE Deleted != 1 ) AS tic ON t.Id = tic.TicketListId
                                        WHERE b.Id = '{boardId}' AND b.Deleted != 1
                                        ORDER BY b.CreatedAt DESC";

                var query = connection.CreateCommand();

                query.CommandText = sqlCommand;

                var rowRecords = await query.ExecuteReaderAsync();

                var board = new Board();
                var tempId = Guid.NewGuid();
                while (rowRecords.Read())
                {
                    if(tempId != Guid.Parse(rowRecords["BoardId"].ToString()))
                        board = RowRecordParser.ParseBoard(rowRecords);
                    // fati slucaj ako nema ticket listi
                    if (!string.IsNullOrEmpty(rowRecords["TicketListId"].ToString())) 
                    {
                        var ticketList = RowRecordParser.ParseTicketList(rowRecords);

                        if(!string.IsNullOrEmpty(rowRecords["TicketId"].ToString()))
                        {
                            ticketList.Tickets.Add(RowRecordParser.ParseTicket(rowRecords));
                        }

                        board.TicketLists.Add(ticketList);
                        tempId = board.Id;
                    }
                }

                if (board.TicketLists.Any())
                {
                    var groupedTicketLists = board.TicketLists.GroupBy(x => x.Id);
                    board.TicketLists = groupedTicketLists.Select(group => new TicketList
                    {
                        Id = group.First().Id,
                        Name = group.First().Name,
                        CreatedAt = group.First().CreatedAt,
                        Tickets = group.SelectMany(x => x.Tickets).OrderBy(t => t.CreatedAt).ToList()
                    }).OrderBy(g => g.CreatedAt).ToList();
                }

                connection.Close();

                return board;
            }
        }

        public async Task<Board> AddBoard(Board board)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();

                string sqlCommand = $"INSERT INTO Board(Id,Name,CreatedAt,ProjectId) VALUES ('{board.Id}','{board.Name}','{board.CreatedAt}','{board.ProjectId}')";

                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;

                await command.ExecuteNonQueryAsync();

                connection.Close();

                return board;
            }
        }

        public async Task EditBoard(Guid boardId, string name)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();

                string sqlCommand = $"UPDATE Board SET Name = '{name}' WHERE Id = '{boardId}'";

                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;

                await command.ExecuteNonQueryAsync();



                connection.Close();

                

            }
        }

        public async Task DeleteBoard(Guid boardId)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {

                connection.Open();

                string sqlCommand = $"UPDATE Board SET Deleted = 1 Where Id = '{boardId}'";

                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;
                await command.ExecuteNonQueryAsync();

                connection.Close();
            }
        }

        public async Task<bool> NameTaken(Guid projectId, string name)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();

                var sqlCommand = $"SELECT Name FROM Board b WHERE b.Name = '{name}' AND b.ProjectId = '{projectId}' AND b.Deleted != 1";

                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;

                var rowRecords = await command.ExecuteReaderAsync();

                if(rowRecords.HasRows)
                {
                    connection.Close();
                    return true;
                }

                connection.Close();
                return false;
            }
        }

        public async Task<List<Board>> Search(Guid projectId, string name)
        {
            using(SqlConnection connection = new SqlConnection(_connString))
            {
                connection.Open();

                string sqlCommand = $@"SELECT b.Id AS BoardId, b.Name AS BoardName, b.CreatedAt AS BoardCreatedAt , b.ProjectId AS ProjectId FROM Board b 
                                        WHERE b.ProjectId = '{projectId}' AND b.Name LIKE '%{name}%' AND b.Deleted != 1";

                var query = connection.CreateCommand();
                query.CommandText = sqlCommand;

                var rowRecords = await query.ExecuteReaderAsync();
                List<Board> boards = new List<Board>();

                while(rowRecords.Read())
                {
                    boards.Add(RowRecordParser.ParseBoard(rowRecords));

                }
                connection.Close();
                return boards;
            }
        }

        public async Task<bool> HasActiveTicketLists(Guid boardId)
        {
            using (SqlConnection connection = new SqlConnection(_connString))
            {

                connection.Open();

                string sqlCommand = $@"SELECT * FROM TicketList t
                                        WHERE t.BoardId = '{boardId}' AND t.Deleted != 1";

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
    }
}