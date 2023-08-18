
using SudyApi.Models;

namespace SudyApi.ViewModels
{
    public class RegisterSubjectViewModel
    {
        public int DisciplineId { get; set; }

        public string? Name { get; set;}

        public static implicit operator SubjectModel(RegisterSubjectViewModel viewModel)
        {
            return new SubjectModel(viewModel);
        }
    }
}
