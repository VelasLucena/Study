using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models.Subject
{
    public class SubjectChapterModel
    {
        [Key]
        public int SubjectChapterId { get; set; }

        public int? SubjectId { get; set; }

        public ChapterModel? ChapterId { get; set; }
    }
}
