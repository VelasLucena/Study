using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SudyApi.Models;
using System.Runtime.CompilerServices;

namespace SudyApi.Data.Configurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> modelBuilder)
        {
            modelBuilder
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder
                .HasOne(x => x.UserInformation)
                .WithOne(x => x.User)
                .HasForeignKey<UserModel>(x => x.UserInformationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
