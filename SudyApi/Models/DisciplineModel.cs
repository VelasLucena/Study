using SudyApi.ViewModels;
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

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ICollection<SubjectModel> Subjects { get; set; }

        public DisciplineModel() { }

        public DisciplineModel(SemesterModel semester, DisciplineNameModel disciplineName)
        {
            SemesterId = semester.SemesterId;
            Name = disciplineName;
            CreationDate = DateTime.Now;
        }

        public void Update(SemesterModel semester, DisciplineNameModel disciplineName)
        {
            Semester = semester;
            Name = disciplineName;
            UpdateDate = DateTime.Now;
        }
    }
}
