using SudyApi.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SudyApi.Models
{
    public class SemesterModel
    {
        [Key]
        public int SemesterId { get; set; }

        public int UserId { get; set; }

        public UserModel? User { get; set; }

        public int CourseId { get; set; }

        public CourseModel? Course { get; set; }

        public int IntitutionId { get; set; }

        public InstitutionModel? Institution { get; set; }

        public ConfigSemesterModel? ConfigSemester { get; set; }

        public string? CurrentSemester { get; set; }

        public DateOnly SemesterStart { get; set; }

        public DateOnly SemesterEnd { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ICollection<DisciplineModel>? Disciplines { get; set; }

        public ICollection<ImportantDateModel>? ImportantDates { get; set; }

        public SemesterModel() { }

        public SemesterModel(RegisterSemesterModel viewModel, UserModel user, CourseModel course, InstitutionModel institution)
        {
            User = user;
            Course = course;
            Institution = institution;
            ConfigSemester = new ConfigSemesterModel(viewModel);
            CurrentSemester = viewModel.CurrentSemester;
            SemesterEnd = viewModel.SemesterEnd;
            SemesterStart = viewModel.SemesterStart;
            CreationDate = DateTime.Now;
        }

        public void Update(EditSemesterViewModel viewModel, UserModel user, CourseModel course, InstitutionModel institution)
        {
            User = user != null ? user : User;
            Course = course != null ? course : Course;
            Institution = institution != null ? institution : Institution;
            CurrentSemester = viewModel.CurrentSemester != null ? viewModel.CurrentSemester : CurrentSemester;
            SemesterEnd = viewModel.SemesterEnd != null ? viewModel.SemesterEnd.Value : SemesterEnd;
            SemesterStart = viewModel.SemesterStart != null ? viewModel.SemesterStart.Value : SemesterStart;
            UpdateDate = DateTime.Now;

            ConfigSemester.Update(viewModel);
        }

        public static bool ScheduleIsPossible(SemesterModel semester)
        {
            if (semester.ConfigSemester.HoursForStudy == null)
                return false;

            int daysForStudyCount = Convert.ToInt32((
                semester.SemesterStart
                .ToDateTime(TimeOnly.MinValue).Date 
                - 
                semester.SemesterEnd
                .ToDateTime(TimeOnly.MinValue).Date)
                .TotalDays);
            int totalModulesCount = 0;         

            foreach(DisciplineModel discipline in semester.Disciplines)
            {
                foreach(SubjectModel subject in discipline.Subjects)
                {
                    foreach(ChapterModel chapter in subject.Chapters)
                    {
                        totalModulesCount = totalModulesCount + chapter.ModulesCount;
                    }
                }
            }

            int modulesCountHasPossible = daysForStudyCount * semester.ConfigSemester.HoursForStudy.Value;

            if (modulesCountHasPossible < totalModulesCount)
                return false;
            else
                return true;
        }
    }
}
