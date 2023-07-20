using Microsoft.EntityFrameworkCore;
using SudyApi.Models;

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

        public DbSet<CollegeSubjectModel> CollegeSubjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Index

            modelBuilder.Entity<UserModel>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<UserInformation>()
                .HasIndex(x => x.Cpf).IsUnique();

            #endregion

            #region TableName

            modelBuilder.Entity<UserInformation>()
                .ToTable("Users_Information");

            modelBuilder.Entity<CollegeSubjectModel>()
                .ToTable("College_Subjects");

            #endregion

            #region ForeignKey

            modelBuilder.Entity<SemesterModel>()
                .HasMany(x => x.CollegeSubjects)
                .WithOne(x => x.Semester)
                .HasForeignKey(x => x.SemesterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CollegeSubjectModel>()
                .HasMany(x => x.Subjects)
                .WithOne(x => x.CollegeSubject)
                .HasForeignKey(x => x.CollegeSubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SubjectModel>()
                .HasMany(x => x.Chapters)
                .WithOne(x => x.Subject)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}
