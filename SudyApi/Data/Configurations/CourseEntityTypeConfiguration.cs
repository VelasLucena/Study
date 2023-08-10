using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SudyApi.Models;
using SudyApi.Utility;
using System.Reflection.Emit;

namespace SudyApi.Data.Configurations
{
    public class CourseEntityTypeConfiguration : IEntityTypeConfiguration<CourseModel>
    {
        public void Configure(EntityTypeBuilder<CourseModel> builder)
        {
            List<CourseModel> courses = DefaultValues.GenerateCourses();

            builder.HasData(courses);
            builder.HasIndex(a => new { a.Name, a.Level })
                .IsUnique()
                .IsDescending();
        }
    }
}
