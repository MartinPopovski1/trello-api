using Projector.Core.Logic.Exceptions;
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
    public class BoardHandler : IBoardHandler
    {
        IBoardRepository _repository;
        IBoardValidator _validator;
        public BoardHandler(IBoardRepository repository, IBoardValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<List<Board>> GetAll(Guid projectId)
        {
            return await _repository.GetAll(projectId);
        }
        public async Task<Board> Get(BaseIdentifyRequest<Guid> request)
        {
            return await _repository.Get(request.Id);
        }
        public async Task<bool> Edit(EditBoardRequest request, Guid projectId)
        {
            var validationResult = await _validator.ValidateCreate(projectId, request.Name);
            if (!validationResult.IsSuccessful)
                throw new BusinessException(validationResult);

            await _repository.EditBoard(request.Id, request.Name);
            return true;
        }
        public async Task<bool> Delete(BaseIdentifyRequest<Guid> request)
        {
            var validationResult = await _validator.ValidateDelete(request.Id);
            if (!validationResult.IsSuccessful)
                throw new BusinessException(validationResult);

            await _repository.DeleteBoard(request.Id);
            return true;
        }
        public async Task<Board> Create(CreateBoardRequest request , Guid projectId)
        {
            var validationResult = await _validator.ValidateCreate(projectId, request.Name);
            if (!validationResult.IsSuccessful)
                throw new BusinessException(validationResult);

            var board = new Board(request, projectId);
            return await _repository.AddBoard(board);
        }
        public async Task<List<Board>> Search(Guid projectId, string name)
        {
            return await _repository.Search(projectId, name);
        }
    }
}
