using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SudyApi.Models;
using SudyApi.Utility;
using System.Reflection.Emit;

namespace SudyApi.Data.Configurations
{
    public class InstitutionEntityTypeConfiguration : IEntityTypeConfiguration<InstitutionModel>
    {
        public void Configure(EntityTypeBuilder<InstitutionModel> builder)
        {

            List<InstitutionModel> institutions = DefaultValues.GenerateInstitutions();

            builder.HasData(institutions);
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
