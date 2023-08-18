using SudyApi.Models;

namespace SudyApi.ViewModels
{
    public class RegisterChapterViewModel
    {
        public int SubjectId { get; set; }

        public string? Name { get; set; }

        public int ModulesCount { get; set; }

        public static implicit operator ChapterModel(RegisterChapterViewModel viewModel)
        {
            return new ChapterModel(viewModel);
        }
    }
}
