using Microsoft.EntityFrameworkCore;
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

        public DbSet<DayOfWeekModel> DaysOfWeek { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Index

            modelBuilder.Entity<UserModel>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<UserInformation>()
                .HasIndex(x => x.Cpf)
                .IsUnique();

            modelBuilder.Entity<DisciplineNameModel>()
                .HasIndex(x => x.Name)
                .IsUnique();

            modelBuilder.Entity<InstitutionModel>()
                .HasIndex(x => x.Name)
                .IsUnique();

            modelBuilder.Entity<CourseModel>()
                        .HasIndex(a => new { a.Name, a.Level })
                        .IsUnique();

            #endregion

            #region TableName

            modelBuilder.Entity<UserInformation>()
                .ToTable("Users_Information");

            modelBuilder.Entity<DisciplineNameModel>()
                .ToTable("Discipline_Name");

            #endregion

            #region ForeignKey

            modelBuilder.Entity<SemesterModel>()
                .HasMany(x => x.Disciplines)
                .WithOne(x => x.Semester)
                .HasForeignKey(x => x.SemesterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DisciplineModel>()
                .HasMany(x => x.Subjects)
                .WithOne(x => x.Discipline)
                .HasForeignKey(x => x.DisciplineId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SubjectModel>()
                .HasMany(x => x.Chapters)
                .WithOne(x => x.Subject)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Default Data

            List<CourseModel> courses = DefaultValues.GenerateCourses();

            modelBuilder.Entity<CourseModel>().HasData(courses);

            List<InstitutionModel> institutions = DefaultValues.GenerateInstitutions();

            modelBuilder.Entity<InstitutionModel>().HasData(institutions);

            List<DisciplineNameModel> names = DefaultValues.GenerateDisciplineNames();

            modelBuilder.Entity<DisciplineNameModel>()
                .HasData(names);

            #endregion
        }
    }
}
