using MediatR;
using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Queries.SellerApplicationQueries
{
    public class ApproveQuery : IRequest<Response>
    {
        public int Id { get; set; }
    }
}
