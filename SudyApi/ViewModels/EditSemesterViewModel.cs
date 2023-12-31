﻿namespace SudyApi.ViewModels
{
    public class EditSemesterViewModel
    {
        public int SemesterId { get; set; }

        public int? CourseId { get; set; }

        public int? InstitutionId { get; set; }

        public string? CurrentSemester { get; set; }

        public DateOnly? SemesterStart { get; set; }

        public DateOnly? SemesterEnd { get; set; }

        public int? HoursForStudy { get; set; }

        public string? DaysForStudy { get; set; }
    }
}
