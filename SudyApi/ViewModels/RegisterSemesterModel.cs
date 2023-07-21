
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
        public string CurrentSemester { get; set; }
    }
}
