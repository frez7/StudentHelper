using MediatR;
using StudentHelper.Model.Models.Entities.SellerEntities;

namespace StudentHelper.Model.Models.Queries.SellerQueries
{
    public class GetSellerByIdQuery : IRequest<SellerDTO>
    {
        public int Id { get; set; }
    }
}
