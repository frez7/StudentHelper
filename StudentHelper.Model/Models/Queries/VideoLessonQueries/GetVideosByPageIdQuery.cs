using MediatR;
using StudentHelper.Model.Models.Entities.CourseDTOs;

namespace StudentHelper.Model.Models.Queries.VideoLessonQueries
{
    public class GetVideosByPageIdQuery : IRequest<List<VideoLessonDTO>>
    {
        public int PageId { get; set; }
    }
}
