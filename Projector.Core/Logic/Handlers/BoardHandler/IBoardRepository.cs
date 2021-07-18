using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Projector.Core.Logic.Handlers.BoardHandler
{
    public interface IBoardRepository
    {
        Task<Board> AddBoard(Board board);
        Task DeleteBoard(Guid boardId);
        Task EditBoard(Guid boardId, string name);
        Task<Board> Get(Guid boardId);
        Task<List<Board>> GetAll(Guid projectId);
        Task<bool> NameTaken(Guid projectId, string name);
        Task<List<Board>> Search(Guid projectId, string name);
        Task<bool> HasActiveTicketLists(Guid boardId);
        
    }
}