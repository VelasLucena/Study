using System.ComponentModel.DataAnnotations;

namespace SudyApi.Models
{
    public class DisciplineNameModel
    {
        [Key]
        public int DisciplineNameId { get; set; }

        public string? Name { get; set; }

        public ICollection<DisciplineModel>? Disciplines { get; set; }
        
        public DisciplineNameModel() { }

        public DisciplineNameModel(string name)
        {
            Name = name;
        }

        public void Update(string disciplineName)
        {
            Name = disciplineName != null ? disciplineName : Name;
        }
    }
}
