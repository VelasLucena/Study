using SudyApi.Models;
using SudyApi.Properties.Enuns;

namespace SudyApi.Data.Interfaces
{
    public interface IUserRepository
    {
        #region GetUsers

        Task<List<UserModel>> GetAllUsers(Ordering ordering = Ordering.Asc, string keySelector = nameof(UserModel.UserId), bool isTracking = true, int take = 10, int skip = 0);

        Task<List<UserModel>> GetAllUsersNoTracking(int limit, Ordering ordering, string order);

        #endregion

        #region GetUserById

        Task<UserModel> GetUserById(int user);

        Task<UserModel> GetUserByIdNoTracking(int user);

        Task<UserModel> GetUserByIdSql(int user);

        #endregion

        #region GetUserByEmail

        Task<UserModel> GetUserByEmail(string email);

        Task<UserModel> GetUserByEmailNotracking(string email);

        Task<UserModel> GetUserByEmailSql(string email);

        #endregion

        #region GetUserByName

        Task<UserModel> GetUserByNameFirst(string name);

        Task<UserModel> GetUserByNameFirstNoTracking(string name);

        #endregion 
    }
}
