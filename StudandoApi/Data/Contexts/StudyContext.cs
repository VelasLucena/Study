using Microsoft.EntityFrameworkCore;
using StudandoApi.Models.User;

namespace StudandoApi.Data.Contexts
{
    public class StudyContext : DbContext
    {

        public StudyContext(DbContextOptions<StudyContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }

        public DbSet<UserInformation> UsersInformation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<UserInformation>().HasIndex(x => x.Cpf).IsUnique();
        }

        public bool UserIsLogged(int user)
        {
            if (Users.FirstOrDefaultAsync(x => x.UserId == user) != null)
                return true;

            return false;
        }
    }
}
