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

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public DayOfWeekModel(){ }

        public DayOfWeekModel(KeyValuePair<string, DisciplineModel> studyPlan, int hourBeginStudy)
        {
            Discipline = studyPlan.Value;
            DisciplineId = studyPlan.Value.DisciplineId;
            DayOfWeekType = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), studyPlan.Key);
            Hour = TimeOnly.Parse(hourBeginStudy.ToString());
        }

        //public DayOfWeekModel CreateDayToStudy(string day)
        //{

        //}
    }
}
