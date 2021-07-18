using Projector.Core.Logic.Requests;
using Projector.Core.Logic.Requests.Board;
using Projector.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projector.Core.Logic.Handlers.BoardHandler
{
    public interface IBoardHandler
    {
        Task<Board> Create(CreateBoardRequest request, Guid proejctId);
        Task<bool> Delete(BaseIdentifyRequest<Guid> request);
        Task<bool> Edit(EditBoardRequest request, Guid projectId);
        Task<Board> Get(BaseIdentifyRequest<Guid> request);
        Task<List<Board>> GetAll(Guid projectId);
        Task<List<Board>> Search(Guid projectId, string name);
    }
}
