using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models.Subject
{
    public class ChapterModel
    {
        [Key]
        public int ChapterId { get; set; }

        public string? Name { get; set; }

        public int? ModulesCount { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
