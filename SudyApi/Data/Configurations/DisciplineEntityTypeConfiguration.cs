using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SudyApi.Models;
using System.Reflection.Emit;

namespace SudyApi.Data.Configurations
{
    public class DisciplineEntityTypeConfiguration : IEntityTypeConfiguration<DisciplineModel>
    {
        public void Configure(EntityTypeBuilder<DisciplineModel> modelBuilder)
        {
            modelBuilder
                .HasOne(x => x.DisciplineName)
                .WithMany(x => x.Disciplines)
                .HasForeignKey(x => x.DisciplineNameId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder
                .HasOne(x => x.Semester)
                .WithMany(x => x.Disciplines)
                .HasForeignKey(x => x.SemesterId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
