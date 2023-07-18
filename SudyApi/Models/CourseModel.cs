using StudandoApi.Properties.Enuns;

namespace SudyApi.Models
{
    public class CourseModel
    {
        public int CourseId { get; set; }

        public string? CourseName { get; set; }

        public GraduationLevel? Level { get; set; }

        public int? SemestersCount { get; set; }
    }
}
