
namespace SudyApi.ViewModels
{
    public class RegisterSubjectViewModel
    {
        public int UserId {get; set;}

        public string Name { get; set;}

        public List<RegisterChapterViewModel> Chapters { get; set;}
    }
}
