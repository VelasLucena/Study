using Microsoft.EntityFrameworkCore;
using SudyApi.Data.Configurations;
using SudyApi.Models;
using SudyApi.Utility;

namespace StudandoApi.Data.Contexts
{
    public class SudyContext : DbContext
    {

        public SudyContext(DbContextOptions<SudyContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }

        public DbSet<UserInformation> UsersInformation { get; set; }

        public DbSet<SubjectModel> Subjects { get; set; }

        public DbSet<ChapterModel> Chapters { get; set; }

        public DbSet<CourseModel> Courses { get; set; }

        public DbSet<InstitutionModel> Institutions { get; set; }

        public DbSet<SemesterModel> Semesters { get; set; }

        public DbSet<DisciplineModel> Disciplines { get; set; }

        public DbSet<DisciplineNameModel> DisciplinesName { get; set; }

        public DbSet<ImportantDateModel> ImportantDates { get; set; }

        public DbSet<DayOfWeekModel> DaysOfWeeks { get; set; }

        public DbSet<ConfigSemesterModel> ConfigSemesters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new UserEntityTypeConfiguration();
            new UserInformationEntityTypeConfiguration();
            new InstitutionEntityTypeConfiguration();
            new CourseEntityTypeConfiguration();
            new SemesterEntityTypeConfiguration();
            new DisciplineEntityTypeConfiguration();
            new DisciplineNameEntityTypeConfiguration();
            new SubjectEntityTypeConfiguration();
        }
    }
}
