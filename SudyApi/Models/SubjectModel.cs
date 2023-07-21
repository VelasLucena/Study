using SudyApi.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models
{
    public class SubjectModel
    {
        [Key]
        public int SubjectId { get; set; }

        public string? Name { get; set; }

        public int? DisciplineId { get; set; }

        public DisciplineModel? Discipline { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ICollection<ChapterModel>? Chapters { get; set; }


        public SubjectModel() { }

        public SubjectModel(RegisterSubjectViewModel viewModel)
        {
            Name = viewModel.Name;
            DisciplineId = viewModel.DisciplineId;
            CreationDate = DateTime.Now;
        }

        public void Update(EditSubjectViewModel viewModel)
        {
            Name = viewModel.Name;
            UpdateDate = DateTime.Now;
        }
    }
}
