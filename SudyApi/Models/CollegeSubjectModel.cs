using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models
{
    public class CollegeSubjectModel
    {
        [Key]
        public int CollegeSubjectId { get; set; }

        public int? SemesterId { get; set; }

        public SemesterModel? Semester { get; set; }

        public string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public ICollection<SubjectModel> Subjects { get; set; }
    }
}
