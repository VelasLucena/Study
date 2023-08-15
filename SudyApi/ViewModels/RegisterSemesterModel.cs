
using System.ComponentModel.DataAnnotations;

namespace SudyApi.ViewModels
{
    public class RegisterSemesterModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int InstitutionId { get; set; }

        [Required]
        public string? CurrentSemester { get; set; }

        [Required]
        public DateOnly SemesterStart { get; set; }

        [Required]
        public DateOnly SemesterEnd { get; set; }

        [Required]
        public int HoursForStudy { get; set; }

        [Required]
        public int HourForStudy { get; set; }

        [Required]
        public string? DaysForStudy { get; set; }
    }
}
