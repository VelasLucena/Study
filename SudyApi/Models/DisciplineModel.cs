using SudyApi.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models
{
    public class DisciplineModel
    {
        [Key]
        public int DisciplineId { get; set; }

        public int? SemesterId { get; set; }

        public SemesterModel? Semester { get; set; }

        public DisciplineNameModel? Name { get; set; }

        public DateOnly DisciplineStart { get; set; }

        public DateOnly DisciplineEnd { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ICollection<SubjectModel> Subjects { get; set; }

        public DisciplineModel() { }

        public DisciplineModel(SemesterModel semester, DisciplineNameModel disciplineName, RegisterDisciplineViewModel viewModel)
        {
            SemesterId = semester.SemesterId;
            Name = disciplineName;
            DisciplineEnd = viewModel.DisciplineEnd.Value;
            DisciplineStart = viewModel.DisciplineStart.Value;
            CreationDate = DateTime.Now;
        }

        public void Update(SemesterModel semester, DisciplineNameModel disciplineName, EditDisciplineViewModel viewModel)
        {
            Semester = semester;
            Name = disciplineName;
            DisciplineEnd = viewModel.DisciplineEnd.Value;
            DisciplineStart = viewModel.DisciplineStart.Value;
            UpdateDate = DateTime.Now;
        }
    }
}
