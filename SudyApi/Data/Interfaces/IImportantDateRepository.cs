using SudyApi.Models;
using SudyApi.Properties.Enuns;

namespace SudyApi.Data.Interfaces
{
    public interface IImportantDateRepository
    {
        Task<List<ImportantDateModel>> GetAllImportantDateByScheduleId(int scheduleId);

        Task<List<ImportantDateModel>> GetAllImportantDateByScheduleIdNoTracking(int scheduleId);

        Task<ImportantDateModel> GetImportantDateById(int importantDateId);

        Task<ImportantDateModel> GetImportantDateByIdNoTracking(int importantDateId);

        Task<List<ImportantDateModel>> GetImportantDateByDate(DateOnly date);

        Task<List<ImportantDateModel>> GetImportantDateByDateNoTracking(DateOnly date);

    }
}
