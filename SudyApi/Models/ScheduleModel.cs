using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models
{
    public class ScheduleModel
    {
        [Key]
        public int ScheduleId { get; set; }

        public UserModel User { get; set; }

        public SemesterModel Semester { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ICollection<ImportantDateModel>? ImportantDates { get; set; }
    }
}
