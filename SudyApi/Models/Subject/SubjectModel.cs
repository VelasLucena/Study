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

        public string? Name { get; set; }

        public ICollection<ChapterModel>? Chapters { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public SubjectModel() { }

        public SubjectModel(RegisterSubjectViewModel viewModel, UserModel user) 
        {
            Name = viewModel.Name;
            CreationDate = DateTime.Now;
            UserId = viewModel.UserId;
            User = user;
        }

        public SubjectModel(EditSubjectViewModel viewModel, UserModel user)
        {
            SubjectId = viewModel.SubjectId;
            Name = viewModel.Name;
            UpdateDate = DateTime.Now;
            UserId = viewModel.UserId;
            User = user;
        }

        public void Update(EditSubjectViewModel viewModel)
        {
            Name = viewModel.Name;
            UpdateDate = DateTime.Now;
        }
    }
}
