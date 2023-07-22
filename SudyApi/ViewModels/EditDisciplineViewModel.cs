namespace SudyApi.ViewModels
{
    public class EditDisciplineViewModel
    {
        public int DisciplineId { get; set; }

        public int? SemesterId { get; set; }

        public int? DisciplineNameId { get; set; }

        public string? DisciplineName { get; set; }

        public DateOnly? DisciplineStart { get; set; }

        public DateOnly? DisciplineEnd { get; set; }
    }
}
