using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Core.Configuration;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder, DbContextOptionsBuilder optionsBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserInformationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new InstitutionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CourseEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SemesterEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DisciplineEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DisciplineNameEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectEntityTypeConfiguration());

            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
