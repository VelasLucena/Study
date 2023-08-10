using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SudyApi.Models;
using SudyApi.Utility;
using System.Reflection.Emit;

namespace SudyApi.Data.Configurations
{
    public class DisciplineNameEntityTypeConfiguration : IEntityTypeConfiguration<DisciplineModel>
    {
        public void Configure(EntityTypeBuilder<DisciplineModel> builder)
        {
            List<DisciplineNameModel> names = DefaultValues.GenerateDisciplineNames();

            builder.HasData(names);
            builder.HasIndex(x => x.Name).IsUnique();
            builder.ToTable("Discipline_Name");
        }
    }
}
