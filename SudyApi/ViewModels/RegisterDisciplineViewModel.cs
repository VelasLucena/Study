namespace SudyApi.ViewModels
{
    public class RegisterDisciplineViewModel
    {
        public int SemesterId { get; set; }

        public int DisciplineNameId { get; set; }

        public string? DisciplineName { get; set; }

        public DateOnly? DisciplineStart { get; set; }

        public DateOnly? DisciplineEnd { get; set; }
    }
}
