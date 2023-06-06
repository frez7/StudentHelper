using MediatR;
using StudentHelper.BL.Services.SellerServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Queries.SellerApplicationQueries;

namespace StudentHelper.BL.Handlers.SellerHandlers.SellerApplicationHandling
{
    public class ApproveHandler : IRequestHandler<ApproveQuery, Response>
    {
        private readonly SellerApplicationService _service;
        public ApproveHandler(SellerApplicationService service)
        {
            _service = service;
        }

        public async Task<Response> Handle(ApproveQuery query, CancellationToken cancellationToken)
        {
            return await _service.Approve(query.Id);
        }
    }
}
