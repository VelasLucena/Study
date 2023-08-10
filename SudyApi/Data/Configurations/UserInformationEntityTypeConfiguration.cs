using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SudyApi.Models;

namespace SudyApi.Data.Configurations
{
    public class UserInformationEntityTypeConfiguration : IEntityTypeConfiguration<UserInformation>
    {
        public void Configure(EntityTypeBuilder<UserInformation> builder)
        {
            builder.HasIndex(x => x.Cpf).IsUnique();
            builder.ToTable("Users_Information");
        }
    }
}
