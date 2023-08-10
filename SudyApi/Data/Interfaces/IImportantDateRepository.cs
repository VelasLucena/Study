using SudyApi.Models;
using SudyApi.Properties.Enuns;

namespace SudyApi.Data.Interfaces
{
    public interface IImportantDateRepository
    {
        Task<List<ImportantDateModel>> GetAllImportantDateBySemesterId(int semesterId, int take = 10, int skip = 0);

        Task<List<ImportantDateModel>> GetAllImportantDateBySemesterIdNoTracking(int semesterId, int take = 10, int skip = 0);

        Task<ImportantDateModel> GetImportantDateById(int importantDateId);

        Task<ImportantDateModel> GetImportantDateByIdNoTracking(int importantDateId);

        Task<List<ImportantDateModel>> GetImportantDateByDate(DateOnly date, int take = 10, int skip = 0);

        Task<List<ImportantDateModel>> GetImportantDateByDateNoTracking(DateOnly date, int take = 10, int skip = 0);

    }
}
