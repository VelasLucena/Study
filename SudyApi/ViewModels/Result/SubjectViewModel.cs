using StudandoApi.Models.User;
using SudyApi.Models.Subject;
using System.ComponentModel.DataAnnotations;

namespace SudyApi.ViewModels.Result
{
    public class SubjectViewModel
    {
        public int SubjectId { get; set; }

        public int? UserId { get; set; }

        public string? Name { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public List<ChapterViewModel>? Chapters { get; set; }

        public SubjectViewModel() { }

        public SubjectViewModel(SubjectModel subject) 
        {
            SubjectId = subject.SubjectId;
            UserId = subject.UserId;
            Name = subject.Name;
            CreationDate = subject.CreationDate;
            UpdateDate = subject.UpdateDate;

            if(subject.Chapters != null)
            {
                Chapters = new List<ChapterViewModel>();
                foreach (ChapterModel chapter in subject.Chapters)
                {
                    Chapters.Add(new ChapterViewModel(chapter));
                }
            }
        }
    }
}
