using StudandoApi.Models.User;
using SudyApi.Properties.Enuns;

namespace SudyApi.Data.Interfaces
{
    public interface IUserRepository
    {
        #region GetUsers

        Task<List<UserModel>> GetAllUsers(int limit, Ordering ordering, string order);

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
