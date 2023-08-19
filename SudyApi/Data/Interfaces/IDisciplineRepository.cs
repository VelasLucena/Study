using SudyApi.Models;

namespace SudyApi.Data.Interfaces
{
    public interface IDisciplineRepository
    {
        Task<List<DisciplineModel>> GetAllDisciplines(int semesterId);

        Task<DisciplineModel> GetDisciplineByName(int disciplineNameId, int semesterId);

        Task<DisciplineModel> GetDisciplineById(int disciplineId);

        Task<List<DisciplineModel>> GetDisciplinesBySemesterId(int semesterId);

        Task<DisciplineModel> GetDisciplineByChapterId(int chapterId);
    }
}
