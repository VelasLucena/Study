using SudyApi.Models;
using SudyApi.Properties.Enuns;

namespace SudyApi.Data.Interfaces
{
    public interface IImportantDateRepository
    {
        Task<List<ImportantDateModel>> GetAllImportantDateBySemesterId(int semesterId);

        Task<ImportantDateModel> GetImportantDateById(int importantDateId);

        Task<List<ImportantDateModel>> GetImportantDateByDate(DateOnly date);

    }
}
