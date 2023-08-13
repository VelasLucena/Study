using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudandoApi.Data.Contexts;
using SudyApi.Data.Helper;
using SudyApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.Utility;
using System.Linq;

namespace SudyApi.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Field

        private readonly SudyContext _sudyContext;
        private readonly ICacheService _cachingService;
        private readonly DataOptionsModel _dataOptions;

        #endregion

        #region Constructor

        public UserRepository(SudyContext sudyContext, ICacheService cacheService, DataOptionsModel dataOptions)
        {
            _sudyContext = sudyContext;
            _cachingService = cacheService;
            _dataOptions = dataOptions;
        }

        #endregion

        #region Methods

        public async Task<List<UserModel>> GetAllUsers(Ordering ordering = Ordering.Asc, string keySelector = nameof(UserModel.UserId))
        {
            return await _sudyContext.Users
                .Include(x => x.UserInformation)
                .Take(_dataOptions.Take)
                .Skip(_dataOptions.Skip)
                .ApplyOrderBy(_dataOptions.KeyOrder, _dataOptions.Ordering)
                .ApplyTracking(_dataOptions.IsTracking)
                .ToListAsync();
        }  

        public async Task<UserModel> GetUserById(int userId)
        {
            bool cache = !bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache));

            if (_dataOptions.IsTracking == true)
                cache = false;

            if (!cache) 
                return await _sudyContext.Users
                .Include(x => x.UserInformation)
                .SingleOrDefaultAsync(x => x.UserId == userId);

            string resultCache = await _cachingService.Get(nameof(UserModel) + userId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<UserModel>(resultCache);

            UserModel? user = await _sudyContext.Users
                                               .Include(x => x.UserInformation)
                                               .ApplyTracking(_dataOptions.IsTracking)
                                               .SingleOrDefaultAsync(x => x.UserId == userId);

            if (user != null)
                await _cachingService.Set(nameof(UserModel) + userId, JsonConvert.SerializeObject(user));

            return user;

        }          

        public async Task<UserModel> GetUserByEmail(string email)
        {
            return await _sudyContext.Users
                .Where(x => x.Email.Contains(email))
                .Include(x => x.UserInformation)
                .ApplyTracking(_dataOptions.IsTracking)
                .SingleOrDefaultAsync();
        }

        public async Task<UserModel> GetUserByNameFirst(string name)
        {
            return await _sudyContext.Users
                .Where(x => x.Name.Contains(name))
                .Include(x => x.UserInformation)
                .ApplyTracking(_dataOptions.IsTracking)
                .SingleOrDefaultAsync();
        }

        #endregion
    }
}
