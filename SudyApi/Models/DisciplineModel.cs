using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models
{
    public class DisciplineModel
    {
        [Key]
        public int DisciplineId { get; set; }

        public int? SemesterId { get; set; }

        public SemesterModel? Semester { get; set; }

        public DisciplineNameModel? Name { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public ICollection<SubjectModel> Subjects { get; set; }
    }
}
