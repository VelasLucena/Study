namespace SudyApi.Models
{
    public class SemesterModel
    {
        public int SemesterId { get; set; }

        public UserModel? User { get; set; }

        public CourseModel? Course { get; set; }

        public InstitutionModel? Institution { get; set; }

        public string? CurrentSemester { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
