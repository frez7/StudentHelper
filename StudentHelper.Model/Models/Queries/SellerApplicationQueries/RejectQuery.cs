using MediatR;
using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Queries.SellerApplicationQueries
{
    public class RejectQuery : IRequest<Response>
    {
        public int Id { get; set; }
    }
}
