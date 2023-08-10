using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudandoApi.Data.Contexts;
using SudyApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.Utility;

namespace SudyApi.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Field

        private readonly SudyContext _sudyContext;
        private readonly ICacheService _cachingService;

        #endregion

        #region Constructor

        public UserRepository(SudyContext sudyContext, ICacheService cacheService)
        {
            _sudyContext = sudyContext;
            _cachingService = cacheService;
        }

        #endregion

        #region Methods

        #region GetUsers

        public async Task<List<UserModel>> GetAllUsers(int limit, Ordering ordering, string attributeName)
        {
            switch (ordering)
            {
                case Ordering.Asc:
                    return await _sudyContext.Users.Include(x => x.UserInformation).OrderBy(x => EF.Property<object>(x, attributeName)).Take(limit).ToListAsync();
                case Ordering.Desc:
                    return await _sudyContext.Users.Include(x => x.UserInformation).OrderByDescending(x => EF.Property<object>(x, attributeName)).Take(limit).ToListAsync();
            }

            return null;
        }

        public async Task<List<UserModel>> GetAllUsersNoTracking(int limit, Ordering ordering, string attributeName)
        {
            switch (ordering)
            {
                case Ordering.Asc:
                    return await _sudyContext.Users.Include(x => x.UserInformation).AsNoTracking().OrderBy(x => EF.Property<object>(x, attributeName)).Take(limit).ToListAsync();
                case Ordering.Desc:
                    return await _sudyContext.Users.Include(x => x.UserInformation).AsNoTracking().OrderByDescending(x => EF.Property<object>(x, attributeName)).Take(limit).ToListAsync();
            }

            return null;
        }

        #endregion

        #region GetUserById

        public async Task<UserModel> GetUserById(int userId)
        {
            return await _sudyContext.Users.Include(x => x.UserInformation).SingleOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<UserModel> GetUserByIdNoTracking(int userId)
        {
            if (!bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache)))
                return await _sudyContext.Users.Include(x => x.UserInformation).AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId);

            string resultCache = await _cachingService.Get(nameof(UserModel) + userId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<UserModel>(resultCache);

            UserModel user = await _sudyContext.Users.Include(x => x.UserInformation).AsNoTracking().SingleOrDefaultAsync(x => x.UserId == userId);

            if (user != null)
                await _cachingService.Set(nameof(UserModel) + userId, JsonConvert.SerializeObject(user));

            return user;
        }

        public async Task<UserModel> GetUserByIdSql(int userId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region GetUserByEmail

        public async Task<UserModel> GetUserByEmail(string email)
        {
            return await _sudyContext.Users.Where(x => x.Email.Contains(email)).Include(x => x.UserInformation).SingleOrDefaultAsync();
        }

        public async Task<UserModel> GetUserByEmailNotracking(string email)
        {
            return await _sudyContext.Users.Where(x => x.Email.Contains(email)).Include(x => x.UserInformation).AsNoTracking().SingleOrDefaultAsync();
        }

        public async Task<UserModel> GetUserByEmailSql(string email)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region GetUserByName

        public async Task<UserModel> GetUserByNameFirst(string name)
        {
            return await _sudyContext.Users.Where(x => x.Name.Contains(name)).Include(x => x.UserInformation).SingleOrDefaultAsync();
        }

        public async Task<UserModel> GetUserByNameFirstNoTracking(string name)
        {
            return await _sudyContext.Users.Where(x => x.Name.Contains(name)).Include(x => x.UserInformation).AsNoTracking().SingleOrDefaultAsync();
        }

        #endregion

        #endregion
    }
}
