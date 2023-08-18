using StudandoApi.Properties.Enuns;
using SudyApi.Models;

namespace SudyApi.ViewModels
{
    public class RegisterImportanteDateViewModel
    {
        public ImportantDateType ImportantDateType { get; set; }

        public DateOnly Date { get; set; }

        public string? Observation { get; set; }

        public static implicit operator ImportantDateModel(RegisterImportanteDateViewModel viewModel)
        {
            return new ImportantDateModel(viewModel);
        }
    }
}
