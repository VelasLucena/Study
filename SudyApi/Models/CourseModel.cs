using StudandoApi.Properties.Enuns;
using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models
{
    public class CourseModel
    {
        [Key]
        public int CourseId { get; set; }

        public string? Name { get; set; }

        public GraduationType Level { get; set; }

        public int SemestersCount { get; set; }

        public CourseModel() { }

        public CourseModel(string name, GraduationType level, int courseId)
        {
            Name = name;
            Level = level;
            CourseId = courseId;
        }
    }
}
