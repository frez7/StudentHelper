using MediatR;
using StudentHelper.Model.Models.Common;

namespace StudentHelper.Model.Models.Queries.VideoLessonQueries
{
    public class DeleteVideoQuery : IRequest<Response>
    {
        public int VideoId { get; set; }
    }
}
