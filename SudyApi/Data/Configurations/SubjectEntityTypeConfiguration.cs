using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SudyApi.Models;
using System.Reflection.Emit;

namespace SudyApi.Data.Configurations
{
    public class SubjectEntityTypeConfiguration : IEntityTypeConfiguration<SubjectModel>
    {
        public void Configure(EntityTypeBuilder<SubjectModel> builder)
        {
            builder.HasMany(x => x.Chapters)
                .WithOne(x => x.Subject)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
