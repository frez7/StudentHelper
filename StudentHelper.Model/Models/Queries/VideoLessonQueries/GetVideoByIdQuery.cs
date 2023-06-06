using MediatR;
using StudentHelper.Model.Models.Entities.CourseDTOs;

namespace StudentHelper.Model.Models.Queries.VideoLessonQueries
{
    public class GetVideoByIdQuery : IRequest<VideoLessonDTO>
    {
        public int VideoId { get; set; }
    }
}
