using SudyApi.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models
{
    public class SemesterModel
    {
        [Key]
        public int SemesterId { get; set; }

        public UserModel? User { get; set; }

        public CourseModel? Course { get; set; }

        public InstitutionModel? Institution { get; set; }

        public string? CurrentSemester { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ICollection<DisciplineModel> Disciplines { get; set; }

        public SemesterModel() { }

        public SemesterModel(RegisterSemesterModel viewModel, UserModel user, CourseModel course, InstitutionModel institution)
        {
            User = user;
            Course = course;
            Institution = institution;
            CurrentSemester = viewModel.CurrentSemester;
            CreationDate = DateTime.Now;
        }

        public void Update(EditSemesterViewModel viewModel, UserModel user, CourseModel course, InstitutionModel institution)
        {
            User = user;
            Course = course;
            Institution = institution;
            CurrentSemester = viewModel.CurrentSemester;
            UpdateDate = DateTime.Now;
        }
    }
}
