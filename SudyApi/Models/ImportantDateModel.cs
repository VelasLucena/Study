using StudandoApi.Properties.Enuns;
using SudyApi.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models
{
    public class ImportantDateModel
    {
        [Key]
        public int ImportantDateId { get; set; }

        public ImportantDateType ImportantDateType { get; set; }

        public DateOnly Date { get; set; }

        public string? Observation { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public ImportantDateModel() { }

        public ImportantDateModel(RegisterImportanteDateViewModel viewModel) 
        {
            ImportantDateType = viewModel.ImportantDateType;
            Date = viewModel.Date;

            if(!string.IsNullOrEmpty(viewModel.Observation))
                Observation = viewModel.Observation;
        }

    }
}
