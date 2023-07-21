using SudyApi.Models;

namespace SudyApi.Data.Interfaces
{
    public interface IDisciplineRepository
    {
        #region GetAllDisciplines

        Task<List<DisciplineModel>> GetAllDisciplines(int semesterId);

        Task<List<DisciplineModel>> GetAllDisciplinesNoTracking(int semesterId);

        #endregion

        #region GetDisciplineByName

        Task<List<DisciplineModel>> GetDisciplinesByName(string name);

        Task<List<DisciplineModel>> GetDisciplinesByNameNoTracking(string name);

        #endregion

        #region GetDisciplineById

        Task<DisciplineModel> GetDisciplineById(int disciplineId);

        Task<DisciplineModel> GetDisciplineByIdNoTracking(int disciplineId);

        #endregion
    }
}
