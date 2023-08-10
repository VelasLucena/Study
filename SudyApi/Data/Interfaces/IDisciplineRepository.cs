using SudyApi.Models;

namespace SudyApi.Data.Interfaces
{
    public interface IDisciplineRepository
    {
        #region GetAllDisciplines

        Task<List<DisciplineModel>> GetAllDisciplines(int semesterId, int take = 10, int skip = 0);

        Task<List<DisciplineModel>> GetAllDisciplinesNoTracking(int semesterId, int take = 10, int skip = 0);

        #endregion

        #region GetDisciplineByName

        Task<DisciplineModel> GetDisciplineByName(int disciplineNameId, int semesterId);

        Task<DisciplineModel> GetDisciplineByNameNoTracking(int disciplineNameId, int semesterId);

        #endregion

        #region GetDisciplineById

        Task<DisciplineModel> GetDisciplineById(int disciplineId);

        Task<DisciplineModel> GetDisciplineByIdNoTracking(int disciplineId);

        #endregion

        #region GetDisciplinesBySemesterId

        Task<List<DisciplineModel>> GetDisciplinesBySemesterId(int semesterId, int take = 10, int skip = 0);

        Task<List<DisciplineModel>> GetDisciplinesBySemesterIdNoTracking(int semesterId, int take = 10, int skip = 0);

        #endregion
    }
}
