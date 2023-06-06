using MediatR;
using StudentHelper.BL.Services.SellerServices;
using StudentHelper.Model.Models.Entities.SellerEntities;
using StudentHelper.Model.Models.Queries.SellerQueries;

namespace StudentHelper.BL.Handlers.SellerHandlers.SellerHandling
{
    public class GetSellerByIdHandler : IRequestHandler<GetSellerByIdQuery, SellerDTO>
    {
        private readonly SellerService _service;
        public GetSellerByIdHandler(SellerService service)
        {
            _service = service;
        }

        public async Task<SellerDTO> Handle(GetSellerByIdQuery query, CancellationToken cancellationToken)
        {
            return await _service.GetSellerById(query.Id);
        }
    }
}
