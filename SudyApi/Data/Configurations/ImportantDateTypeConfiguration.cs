using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SudyApi.Models;

namespace SudyApi.Data.Configurations
{
    public class ImportantDateTypeConfiguration : IEntityTypeConfiguration<ImportantDateModel>
    {
        public void Configure(EntityTypeBuilder<ImportantDateModel> modelBuilder)
        {
            modelBuilder
                .HasOne(x => x.Semester)
                .WithMany(x => x.ImportantDates)
                .HasForeignKey(x => x.SemesterId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
