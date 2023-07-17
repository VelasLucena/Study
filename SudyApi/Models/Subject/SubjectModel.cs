using StudandoApi.Models.User;
using SudyApi.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models.Subject
{
    public class SubjectModel
    {
        [Key]
        public int SubjectId { get; set; }

        public int? UserId { get; set; }

        public UserModel? User { get; set; }

        public ICollection<ChapterModel>? Chapters { get; set; }

        public string? Name { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public SubjectModel() { }

        public SubjectModel(RegisterSubjectViewModel newSubject, UserModel user) 
        {
            Name = newSubject.Name;
            CreationDate = DateTime.Now;
            UserId = newSubject.UserId;
            User = user;
        }

        public SubjectModel(EditSubjectViewModel newSubject, UserModel user)
        {
            SubjectId = newSubject.SubjectId;
            Name = newSubject.Name;
            UpdateDate = DateTime.Now;
            UserId = newSubject.UserId;
            User = user;
        }
    }
}
