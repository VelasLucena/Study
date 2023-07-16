using StudandoApi.Models.User;
using SudyApi.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models.Subject
{
    public class SubjectModel
    {
        [Key]
        public int SubjectId { get; set; }

        public UserModel? User { get; set; }

        public string? Name { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public SubjectModel() { }

        public SubjectModel(RegisterSubjectViewModel newSubject, UserModel user) 
        {
            User = user;
            Name = newSubject.Name;
            CreationDate = DateTime.Now;
        }

        public SubjectModel(EditSubjectViewModel newSubject, UserModel user)
        {
            User = user;
            SubjectId = newSubject.SubjectId;
            Name = newSubject.Name;
            UpdateDate = DateTime.Now;
        }
    }
}
