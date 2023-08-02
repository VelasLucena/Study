using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models
{
    public class InstitutionModel
    {
        [Key]
        public int? institutionId { get; set; }

        public string Name { get; set; }

        public string AbbreviationName { get; set; }
    }
}
