using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudandoApi.Data.Contexts;
using SudyApi.Data.Helper;
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
        private readonly DataOptionsModel _dataOptions;

        #endregion

        #region Constructor

        public SemesterRepository(SudyContext sudyContext, ICacheService cacheService, DataOptionsModel dataOptions)
        {
            _sudyContext = sudyContext;
            _cachingService = cacheService;
            _dataOptions = dataOptions;
        }

        #endregion

        #region Methods
        
        public async Task<List<SemesterModel>> GetAllSemestersByUserId(int userId)
        {
            return await _sudyContext.Semesters
                .Include(x => x.User)
                .Include(x => x.Course)
                .Include(x => x.Institution)
                .Include(x => x.User.UserInformation)
                .Where(x => x.User.UserId == userId)
                .Take(_dataOptions.Take)
                .Skip(_dataOptions.Skip)
                .ApplyOrderBy(_dataOptions.KeyOrder, _dataOptions.Ordering)
                .ApplyTracking(_dataOptions.IsTracking)
                .ToListAsync();
        }
        
        async Task<SemesterModel> ISemesterRepository.GetSemesterById(int semesterId)
        {
            bool cache = bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache));

            if (_dataOptions.IsTracking == true)
                cache = false;

            if (!cache) return await _sudyContext.Semesters
                    .Include(x => x.User)
                    .Include(x => x.User.UserInformation)
                    .Include(x => x.Course)
                    .Include(x => x.Institution)
                    .SingleOrDefaultAsync(x => x.SemesterId == semesterId);

            string resultCache = await _cachingService.Get(nameof(SemesterModel) + semesterId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<SemesterModel>(resultCache);

            SemesterModel? semester = await _sudyContext.Semesters
                    .Include(x => x.User)
                    .Include(x => x.User.UserInformation)
                    .Include(x => x.Course)
                    .Include(x => x.Institution)
                    .ApplyTracking(_dataOptions.IsTracking)
                    .SingleOrDefaultAsync(x => x.SemesterId == semesterId);

            if (semester != null)
                await _cachingService.Set(nameof(SemesterModel) + semesterId, JsonConvert.SerializeObject(semester));

            return semester;
        }      

        #endregion
    }
}
