using SudyApi.Models;

namespace SudyApi.Data.Interfaces
{
    public interface ISemesterRepository
    {
        #region GetAllSemestersByUserId

        Task<List<SemesterModel>> GetAllSemestersByUserId(int userId);

        Task<List<SemesterModel>> GetAllSemestersByUserIdNoTracking(int userId);

        #endregion
    }
}
