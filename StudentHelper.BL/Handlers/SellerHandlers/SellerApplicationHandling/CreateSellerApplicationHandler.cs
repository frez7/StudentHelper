using MediatR;
using StudentHelper.BL.Services.SellerServices;
using StudentHelper.Model.Models.Common;
using StudentHelper.Model.Models.Requests.SellerRequests;

namespace StudentHelper.BL.Handlers.SellerHandlers.SellerApplicationHandling
{
    public class CreateSellerApplicationHandler : IRequestHandler<SellerApplicationRequest, Response>
    {
        private readonly SellerApplicationService _service;
        public CreateSellerApplicationHandler(SellerApplicationService service)
        {
            _service = service;
        }

        public async Task<Response> Handle(SellerApplicationRequest request, CancellationToken cancellationToken)
        {
            return await _service.CreateSellerApplication(request);
        }
    }
}
