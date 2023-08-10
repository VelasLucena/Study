using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using SudyApi.Models;
using System.Reflection.Emit;

namespace SudyApi.Data.Configurations
{
    public class SemesterEntityTypeConfiguration : IEntityTypeConfiguration<SemesterModel>
    {
        public void Configure(EntityTypeBuilder<SemesterModel> builder)
        {
            builder.HasMany(x => x.Disciplines)
                .WithOne(x => x.Semester)
                .HasForeignKey(x => x.SemesterId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.HasMany(x => x.ImportantDates)
                .WithOne(x => x.Semester)
                .HasForeignKey(x => x.SemesterId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.Property(e => e.ConfigSemester)
                .HasConversion(x => JsonConvert.SerializeObject(x),
                x => JsonConvert.DeserializeObject<ConfigSemesterModel>(x));
        }
    }
}
