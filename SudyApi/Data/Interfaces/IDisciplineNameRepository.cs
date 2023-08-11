using SudyApi.Models;

namespace SudyApi.Data.Interfaces
{
    public interface IDisciplineNameRepository
    {
        Task<List<DisciplineNameModel>> GetDisciplineNameByName(string name);


        Task<DisciplineNameModel> GetDisciplineNameById(int disciplineNameId);
    }
}
