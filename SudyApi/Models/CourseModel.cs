using StudandoApi.Properties.Enuns;
using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models
{
    public class CourseModel
    {
        [Key]
        public int CourseId { get; set; }

        public string? CourseName { get; set; }

        public GraduationLevel? Level { get; set; }

        public int? SemestersCount { get; set; }
    }
}
