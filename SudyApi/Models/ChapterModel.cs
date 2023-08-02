using Newtonsoft.Json;
using SudyApi.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SudyApi.Models
{
    public class ChapterModel
    {
        [Key]
        public int ChapterId { get; set; }

        public int SubjectId { get; set; }

        public SubjectModel Subject { get; set; }

        public string Name { get; set; }

        public int? ModulesCount { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ChapterModel() { }

        public ChapterModel(RegisterChapterViewModel viewModel)
        {
            Name = viewModel.Name;
            ModulesCount = viewModel.ModulesCount;
            CreationDate = DateTime.Now;
            SubjectId = viewModel.SubjectId;
        }


        public void Update(EditChapterViewModel viewModel)
        {
            Name = viewModel.Name != null ? viewModel.Name : Name;
            ModulesCount = viewModel.ModulesCount != null ? viewModel.ModulesCount : ModulesCount;
            UpdateDate = DateTime.Now;
        }
    }
}
