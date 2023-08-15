using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SudyApi.Models;
using System.Reflection.Emit;

namespace SudyApi.Data.Configurations
{
    public class UserInformationEntityTypeConfiguration : IEntityTypeConfiguration<UserInformation>
    {
        public void Configure(EntityTypeBuilder<UserInformation> modelBuilder)
        {
            modelBuilder.HasIndex(x => x.Cpf).IsUnique();
            modelBuilder.ToTable("Users_Information");
        }
    }
}
