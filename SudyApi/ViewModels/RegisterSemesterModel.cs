namespace SudyApi.ViewModels
{
    public class RegisterSemesterModel
    {
        public int UserId { get; set; }

        public int CourseId { get; set; }

        public int InstitutionId { get; set; }

        public string CurrentSemester { get; set; }
    }
}
