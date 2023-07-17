using Newtonsoft.Json;
using SudyApi.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SudyApi.Models.Subject
{
    public class ChapterModel
    {
        [Key]
        public int ChapterId { get; set; }

        public int? SubjectId { get; set; }

        public SubjectModel? Subject { get; set; }

        public string? Name { get; set; }

        public int? ModulesCount { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ChapterModel() { }

        public ChapterModel(RegisterChapterViewModel viewModel, SubjectModel subject)
        {
            Name = viewModel.Name;
            ModulesCount = viewModel.ModulesCount;
            CreationDate = DateTime.Now;
            Subject = subject;
        }

        public ChapterModel(EditChapterViewModel viewModel, SubjectModel subject)
        {
            ChapterId = viewModel.ChapterId;
            SubjectId = subject.SubjectId;
            Subject = subject;
            Name = viewModel.Name;
            ModulesCount = viewModel.ModulesCount;
            UpdateDate = DateTime.Now;
        }
    }
}
