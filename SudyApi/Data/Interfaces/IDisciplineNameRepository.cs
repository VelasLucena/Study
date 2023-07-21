using SudyApi.Models;

namespace SudyApi.Data.Interfaces
{
    public interface IDisciplineNameRepository
    {
        #region GetDisciplineNameByName

        Task<List<DisciplineNameModel>> GetDisciplineNameByName(string name);

        Task<List<DisciplineNameModel>> GetDisciplineNameByNameNoTracking(string name);

        #endregion

        #region GetDisciplineNameById

        Task<DisciplineNameModel> GetDisciplineNameById(int disciplineNameId);

        Task<DisciplineNameModel> GetDisciplineNameByIdNoTracking(int disciplineNameId);

        #endregion
    }
}
