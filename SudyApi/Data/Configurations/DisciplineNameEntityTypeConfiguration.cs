using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SudyApi.Models;
using SudyApi.Utility;
using System.Reflection.Emit;

namespace SudyApi.Data.Configurations
{
    public class DisciplineNameEntityTypeConfiguration : IEntityTypeConfiguration<DisciplineNameModel>
    {
        public void Configure(EntityTypeBuilder<DisciplineNameModel> builder)
        {
            List<DisciplineNameModel> names = DefaultValues.GenerateDisciplineNames();

            builder.HasData(names);
            builder.HasIndex(x => x.Name).IsUnique();
            builder.ToTable("Discipline_Name");
        }
    }
}
