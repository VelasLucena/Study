using SudyApi.Models;

namespace SudyApi.Data.Interfaces
{
    public interface IDisciplineNameRepository
    {
        #region GetDisciplineNameByName

        Task<List<DisciplineNameModel>> GetDisciplineNameByName(string name, int take = 10, int skip = 0);

        Task<List<DisciplineNameModel>> GetDisciplineNameByNameNoTracking(string name, int take = 10, int skip = 0);

        #endregion

        #region GetDisciplineNameById

        Task<DisciplineNameModel> GetDisciplineNameById(int disciplineNameId);

        Task<DisciplineNameModel> GetDisciplineNameByIdNoTracking(int disciplineNameId);

        #endregion
    }
}
