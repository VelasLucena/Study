using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models
{
    public class ScheduleDaysModel
    {
        [Key]
        public int ScheduleDaysId { get; set; }

        public DayOfWeekType DayOfWeekType { get; set; }
    }
}
