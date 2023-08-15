using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SudyApi.Models;

namespace SudyApi.Data.Configurations
{
    public class DayOfWeekEntityTypeConfiguration : IEntityTypeConfiguration<DayOfWeekModel>
    {
        public void Configure(EntityTypeBuilder<DayOfWeekModel> builder)
        {
            builder
                .HasOne(x => x.Discipline)
                .WithMany(x => x.DaysOfWeeks)
                .HasForeignKey(x => x.DisciplineId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
