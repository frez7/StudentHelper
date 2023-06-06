using MediatR;
using StudentHelper.Model.Models.Entities.SellerEntities;

namespace StudentHelper.Model.Models.Queries.SellerApplicationQueries
{
    public class GetByIdQuery : IRequest<SellerApplication>
    {
        public int Id { get; set; }
    }
}
