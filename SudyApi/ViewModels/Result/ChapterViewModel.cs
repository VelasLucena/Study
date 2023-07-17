using SudyApi.Models.Subject;
using System.ComponentModel.DataAnnotations;

namespace SudyApi.ViewModels.Result
{
    public class ChapterViewModel
    {
        public int ChapterId { get; set; }

        public int? SubjectId { get; set; }

        public string? Name { get; set; }

        public int? ModulesCount { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ChapterViewModel() { }

        public ChapterViewModel(ChapterModel chapter)
        {
            ChapterId = chapter.ChapterId;
            SubjectId = chapter.SubjectId;
            Name = chapter.Name;
            ModulesCount = chapter.ModulesCount;
            CreationDate = chapter.CreationDate;
            UpdateDate = chapter.UpdateDate;
        }
    }
}
