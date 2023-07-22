using StudandoApi.Properties.Enuns;

namespace SudyApi.ViewModels
{
    public class RegisterImportanteDateViewModel
    {
        public ImportantDateType ImportantDateType { get; set; }

        public DateOnly Date { get; set; }

        public string? Observation { get; set; }
    }
}
