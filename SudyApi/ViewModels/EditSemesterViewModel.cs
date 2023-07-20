namespace SudyApi.ViewModels
{
    public class EditSemesterViewModel
    {
        public int SemesterId { get; set; }

        public int? CourseId { get; set; }

        public int? InstitutionId { get; set; }

        public string? CurrentSemester { get; set; }
    }
}
