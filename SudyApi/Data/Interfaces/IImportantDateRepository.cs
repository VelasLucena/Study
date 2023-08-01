using SudyApi.Models;

namespace SudyApi.Data.Interfaces
{
    public interface IImportantDateRepository
    {
        Task<ImportantDateModel> GetAllImportantDateById(int importantDateId);

        Task<ImportantDateModel> GetAllImportantDateById(int importantDateId);

        Task<ImportantDateModel> GetImportantDateById(int importantDateId);

        Task<ImportantDateModel> GetImportantDateByIdNoTracking(int importantDateId);

        Task<ImportantDateModel> GetImportantDateByDate(DateOnly date);

        Task<ImportantDateModel> GetImportantDateByDateNoTracking(DateOnly date);

    }
}
