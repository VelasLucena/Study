using StudandoApi.Properties.Enuns;
using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models
{
    public class DayOfWeekModel
    {
        [Key]
        public int DayOfWeekId { get; set; }

        public int DisciplineId { get; set; }

        public DisciplineModel Discipline { get; set; }

        public DayOfWeek DayOfWeekType { get; set; }

        public TimeOnly Hour { get; set; }

        public int TotalDaysToStudy { get; set; }
        
        public int ModulesCount { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public DayOfWeekModel(){ }

        public DayOfWeekModel(DisciplineModel discipline, DayOfWeek day, int hourBeginStudy, int totalModulesCount, int totalDaysToStudy)
        {
            Discipline = discipline;
            DisciplineId = discipline.DisciplineId;
            DayOfWeekType = day;
            ModulesCount = totalModulesCount;
            TotalDaysToStudy = totalDaysToStudy;
            Hour = TimeOnly.Parse(hourBeginStudy.ToString());
        }

        //public DayOfWeekModel CreateDayToStudy(string day)
        //{

        //}
    }
}
