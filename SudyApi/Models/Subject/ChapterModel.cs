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

        public string? Name { get; set; }

        public int? ModulesCount { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ChapterModel() { }

        public ChapterModel(RegisterChapterViewModel viewModel)
        {
            Name = viewModel.Name;
            ModulesCount = viewModel.ModulesCount;
            CreationDate = DateTime.Now;
        }
    }
}
