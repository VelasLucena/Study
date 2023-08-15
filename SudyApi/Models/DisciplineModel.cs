using SudyApi.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models
{
    public class DisciplineModel
    {
        [Key]
        public int DisciplineId { get; set; }

        public int SemesterId { get; set; }

        public SemesterModel? Semester { get; set; }

        public int DisciplineNameId { get; set; }

        public DisciplineNameModel? DisciplineName { get; set; }

        public DateOnly DisciplineStart { get; set; }

        public DateOnly DisciplineEnd { get; set; }

        public int TotalDaysToStudy { get; set; }

        public int TotalModulesCount { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ICollection<SubjectModel>? Subjects { get; set; }

        public ICollection<DayOfWeekModel>? DaysOfWeeks { get; set; }

        public DisciplineModel() { }

        public DisciplineModel(SemesterModel semester, DisciplineNameModel disciplineName, RegisterDisciplineViewModel viewModel)
        {
            SemesterId = semester.SemesterId;
            DisciplineName = disciplineName;
            DisciplineEnd = viewModel.DisciplineEnd != null ? viewModel.DisciplineEnd.Value : semester.SemesterEnd;
            DisciplineStart = viewModel.DisciplineStart != null ? viewModel.DisciplineStart.Value : semester.SemesterStart;
            CreationDate = DateTime.Now;
        }

        public void Update(SemesterModel semester, DisciplineNameModel disciplineName, EditDisciplineViewModel viewModel)
        {
            Semester = semester != null ? semester : Semester;
            DisciplineName = disciplineName != null ? disciplineName : DisciplineName;
            DisciplineEnd = viewModel.DisciplineEnd != null ? viewModel.DisciplineEnd.Value : DisciplineEnd;
            DisciplineStart = viewModel.DisciplineStart != null ? viewModel.DisciplineStart.Value : DisciplineStart;
            UpdateDate = DateTime.Now;
        }
    }
}
