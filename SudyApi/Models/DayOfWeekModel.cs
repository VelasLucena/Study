using StudandoApi.Properties.Enuns;
using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models
{
    public class DayOfWeekModel
    {
        [Key]
        public int DayOfWeekId { get; set; }

        public int DisciplineId { get; set; }

        public DisciplineModel? Discipline { get; set; }

        public DayOfWeek DayOfWeekType { get; set; }

        public TimeOnly Hour { get; set; }
        
        public int ModulesCount { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public DayOfWeekModel(DisciplineModel discipline, string day, int hourBeginStudy)
        {
            Discipline = discipline;
            DisciplineId = discipline.DisciplineId;
            DayOfWeekType = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), day, true);
            Hour = TimeOnly.Parse(hourBeginStudy.ToString());
        }
    }
}
