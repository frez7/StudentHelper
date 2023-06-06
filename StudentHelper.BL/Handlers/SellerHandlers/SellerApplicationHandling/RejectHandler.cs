using MediatR;
using StudentHelper.BL.Services.SellerServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Queries.SellerApplicationQueries;

namespace StudentHelper.BL.Handlers.SellerHandlers.SellerApplicationHandling
{
    public class RejectHandler : IRequestHandler<RejectQuery, Response>
    {
        private readonly SellerApplicationService _service;
        public RejectHandler(SellerApplicationService service)
        {
            _service = service;
        }

        public async Task<Response> Handle(RejectQuery query, CancellationToken cancellationToken)
        {
            return await _service.Reject(query.Id);
        }
    }
}
