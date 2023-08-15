using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SudyApi.Models;
using System.Reflection.Emit;

namespace SudyApi.Data.Configurations
{
    public class UserInformationEntityTypeConfiguration : IEntityTypeConfiguration<UserInformationModel>
    {
        public void Configure(EntityTypeBuilder<UserInformationModel> modelBuilder)
        {
            modelBuilder
                .HasIndex(x => x.Cpf)
                .IsUnique();

            modelBuilder
                .ToTable("Users_Information");
        }
    }
}
