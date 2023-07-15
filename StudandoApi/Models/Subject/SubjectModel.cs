using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models.Subject
{
    public class SubjectModel
    {
        [Key]
        public int SubjectId { get; set; }

        public string? Name { get; set; }

        public List<ChapterModel>? Chapters { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? CreationUserId { get; set; }

        public int? UpdateUserId { get; set; }
    }
}
