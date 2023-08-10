using SudyApi.Models;
using SudyApi.Properties.Enuns;

namespace SudyApi.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<List<UserModel>> GetAllUsers(Ordering ordering = Ordering.Asc, string keySelector = nameof(UserModel.UserId));

        Task<UserModel> GetUserById(int user);

        Task<UserModel> GetUserByEmail(string email);

        Task<UserModel> GetUserByNameFirst(string name);

    }
}
