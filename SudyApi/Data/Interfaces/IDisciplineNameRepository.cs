using SudyApi.Models;

namespace SudyApi.Data.Interfaces
{
    public interface IDisciplineNameRepository
    {
        #region GetDisciplineNameByName

        Task<List<string>> GetDisciplineNameByName(string name);

        Task<List<string>> GetDisciplineNameByNameNoTracking(string name);

        #endregion

        #region GetDisciplineNameById

        Task<DisciplineNameModel> GetDisciplineNameById(int disciplineNameId);

        Task<DisciplineNameModel> GetDisciplineNameByIdNoTracking(int disciplineNameId);

        #endregion
    }
}
