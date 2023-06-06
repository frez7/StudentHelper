using MediatR;
using StudentHelper.BL.Services.SellerServices;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Queries.SellerApplicationQueries;

namespace StudentHelper.BL.Handlers.SellerHandlers.SellerApplicationHandling
{
    public class GetByIdHandler : IRequestHandler<GetByIdQuery, SellerApplication>
    {
        private readonly SellerApplicationService _service;
        public GetByIdHandler(SellerApplicationService service)
        {
            _service = service;
        }

        public async Task<SellerApplication> Handle(GetByIdQuery query, CancellationToken cancellationToken)
        {
            return await _service.GetById(query.Id);
        }
    }
}
