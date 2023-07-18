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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<UserInformation>().HasIndex(x => x.Cpf).IsUnique();
            modelBuilder.Entity<UserInformation>().ToTable("Users_Information");

            modelBuilder.Entity<SubjectModel>().HasMany(x => x.Chapters).WithOne(x => x.Subject).HasForeignKey(x => x.SubjectId);
            modelBuilder.Entity<UserModel>().HasMany(x => x.Subjects).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        }
    }
}
