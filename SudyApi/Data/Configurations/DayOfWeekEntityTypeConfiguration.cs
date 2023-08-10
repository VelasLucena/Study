using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SudyApi.Models;

namespace SudyApi.Data.Configurations
{
    public class DayOfWeekEntityTypeConfiguration : IEntityTypeConfiguration<DayOfWeekModel>
    {
        public void Configure(EntityTypeBuilder<DayOfWeekModel> builder)
        {

        }
    }
}
