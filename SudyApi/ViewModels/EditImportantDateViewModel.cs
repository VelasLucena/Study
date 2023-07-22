using StudandoApi.Properties.Enuns;

namespace SudyApi.ViewModels
{
    public class EditImportantDateViewModel
    {
        public int? ImportantDateId { get; set; }

        public ImportantDateType? ImportantDateType { get; set; }

        public string? Observation { get; set; }

        public DateOnly? Date { get; set; }
    }
}
