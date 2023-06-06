using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace StudentHelper.Model.Models.Queries.CourseQueries
{
    public class GetImageQuery : IRequest<IActionResult>
    {
        public int CourseId { get; set; }
    }
}
