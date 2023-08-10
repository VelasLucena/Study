using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SudyApi.Models;
using System.Reflection.Emit;

namespace SudyApi.Data.Configurations
{
    public class DisciplineEntityTypeConfiguration : IEntityTypeConfiguration<DisciplineModel>
    {
        public void Configure(EntityTypeBuilder<DisciplineModel> builder)
        {
            builder.HasMany(x => x.Subjects)
                .WithOne(x => x.Discipline)
                .HasForeignKey(x => x.DisciplineId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.DaysOfWeeks)
                .WithOne(x => x.Discipline)
                .HasForeignKey(x => x.DisciplineId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
