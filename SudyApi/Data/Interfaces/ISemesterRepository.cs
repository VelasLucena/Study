using SudyApi.Models;

namespace SudyApi.Data.Interfaces
{
    public interface ISemesterRepository
    {
        Task<List<SemesterModel>> GetAllSemestersByUserId(int userId);

        Task<SemesterModel> GetSemesterById(int semesterId);
    }
}
