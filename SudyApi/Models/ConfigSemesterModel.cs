using SudyApi.ViewModels;

namespace SudyApi.Models
{
    public class ConfigSemesterModel
    {
        public int ConfigSemesterId { get; set; }

        public int HoursForStudy { get; set; }

        public string? DaysForStudy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public ConfigSemesterModel() { }

        public ConfigSemesterModel(RegisterSemesterModel viewModel)
        {
            HoursForStudy = viewModel.HoursForStudy;
            DaysForStudy = viewModel.DaysForStudy;
            CreationDate = DateTime.Now;
        }

        public void Update(EditSemesterViewModel viewModel)
        {
            HoursForStudy = viewModel.HoursForStudy != null ? viewModel.HoursForStudy.Value : HoursForStudy;
            DaysForStudy = viewModel.DaysForStudy != null ? viewModel.DaysForStudy : DaysForStudy;
            UpdateDate = DateTime.Now;
        }
    }
}
