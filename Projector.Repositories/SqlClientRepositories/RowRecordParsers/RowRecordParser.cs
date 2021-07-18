using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Repositories.SqlClientRepositories.RowRecordParsers
{
    public static class RowRecordParser
    {
        public static User ParseUser(SqlDataReader rowRecord)
        {
            return new User
            {
                Id = Guid.Parse(rowRecord["Id"].ToString()),
                UserName = rowRecord["UserName"].ToString(),
                FirstName = rowRecord["FirstName"].ToString(),
                LastName = rowRecord["LastName"].ToString(),
                IsActive = rowRecord["IsActive"].ToString() == "1" ? true : false,
                PasswordHash = rowRecord["PasswordHash"] as byte[],
                PasswordSalt = rowRecord["PasswordSalt"] as byte[],
            };
        }
        public static Project ParseProject(SqlDataReader rowRecord)
        {
            return new Project
            {
                Id = Guid.Parse(rowRecord["ProjectId"].ToString()),
                Name = rowRecord["ProjectName"].ToString(),
                Deleted = false,
                CreatedAt = DateTime.Parse(rowRecord["ProjectCreatedAt"].ToString())
            };
        }
        public static Board ParseBoard(SqlDataReader rowRecord)
        {
            return new Board
            {

                Id = Guid.Parse(rowRecord["BoardId"].ToString()),
                Name = rowRecord["BoardName"].ToString(),
                Deleted = false,
                CreatedAt = DateTime.Parse(rowRecord["BoardCreatedAt"].ToString()),
                ProjectId = Guid.Parse(rowRecord["ProjectId"].ToString())

            };
        }
        public static TicketList ParseTicketList(SqlDataReader rowRecord)
        {
            return new TicketList
            {
                Id = Guid.Parse(rowRecord["TicketListId"].ToString()),
                Name = rowRecord["TicketListName"].ToString(),
                CreatedAt = DateTime.Parse(rowRecord["TicketListCreatedAt"].ToString()),
                Deleted = false,
                BoardId = Guid.Parse(rowRecord["BoardId"].ToString())
            };
        }
        public static Ticket ParseTicket(SqlDataReader rowRecord)
        {
            return new Ticket
            {
                Id = Guid.Parse(rowRecord["TicketId"].ToString()),
                Name = rowRecord["TicketName"].ToString(),
                CreatedAt = DateTime.Parse(rowRecord["TicketCreatedAt"].ToString()),
                Deleted = false,
                TicketListId = Guid.Parse(rowRecord["TicketListId"].ToString())
            };
        }
    }
}
