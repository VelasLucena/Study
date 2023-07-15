using StudandoApi.Models.User;
using SudyApi.Properties.Enuns;

namespace SudyApi.Data.Interfaces
{
    public interface IUserRepository
    {
        #region GetUsers

        Task<List<UserModel>> GetUsers(int limit, Ordering ordering, string order);

        Task<List<UserModel>> GetUsersNoTracking(int limit, Ordering ordering, string order);

        Task<List<UserModel>> GetUsersSql(int limit, string order, string ordering);

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

        Task<UserModel> GetUserByName(string name);

        Task<UserModel> GetUserByNameNoTracking(string name);

        #endregion 
    }
}
