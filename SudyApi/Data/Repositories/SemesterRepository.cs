using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudandoApi.Data.Contexts;
using SudyApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.Utility;

namespace SudyApi.Data.Repositories
{
    public class SemesterRepository : ISemesterRepository
    {
        #region Field

        private readonly SudyContext _sudyContext;
        private readonly ICacheService _cachingService;

        #endregion

        #region Constructor

        public SemesterRepository(SudyContext sudyContext, ICacheService cacheService)
        {
            _sudyContext = sudyContext;
            _cachingService = cacheService;
        }

        #endregion

        #region Methods
        
        #region GetAllSemestersByUserId

        async Task<List<SemesterModel>> ISemesterRepository.GetAllSemestersByUserId(int userId)
        {
            return await _sudyContext.Semesters.Where(x => x.User.UserId == userId).ToListAsync();
        }

        async Task<List<SemesterModel>> ISemesterRepository.GetAllSemestersByUserIdNoTracking(int userId)
        {
            if (!bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache)))
                return await _sudyContext.Semesters.AsNoTracking().Where(x => x.User.UserId == userId).ToListAsync();

            string resultCache = await _cachingService.Get(nameof(UserModel) + userId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<List<SemesterModel>>(resultCache);

            List<SemesterModel> user = await _sudyContext.Semesters.AsNoTracking().Where(x => x.User.UserId == userId).ToListAsync();

            if (user != null)
                await _cachingService.Set(nameof(SemesterModel) + userId, JsonConvert.SerializeObject(user));

            return user;
        }

        #endregion

        #endregion
    }
}
