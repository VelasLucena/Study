using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SudyApi.Models;

namespace SudyApi.Data.Configurations
{
    public class ChapterEntityTypeConfiguration : IEntityTypeConfiguration<ChapterModel>
    {
        public void Configure(EntityTypeBuilder<ChapterModel> builder)
        {
            builder
                .HasOne(x => x.Subject)
                .WithMany(x => x.Chapters)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
