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
            return await _sudyContext.Semesters.Include(x => x.CollegeSubjects).Include(x => x.Course).Include(x => x.Institution).Where(x => x.User.UserId == userId).ToListAsync();
        }

        async Task<List<SemesterModel>> ISemesterRepository.GetAllSemestersByUserIdNoTracking(int userId)
        {
            return await _sudyContext.Semesters.AsNoTracking().Include(x => x.CollegeSubjects).Include(x => x.Course).Include(x => x.Institution).Where(x => x.User.UserId == userId).ToListAsync();
        }

        #endregion

        #region GetSemesterById

        async Task<SemesterModel> ISemesterRepository.GetSemesterById(int semesterId)
        {
            return await _sudyContext.Semesters.Include(x => x.CollegeSubjects).Include(x => x.Course).Include(x => x.Institution).FirstOrDefaultAsync(x => x.SemesterId == semesterId);
        }

        async Task<SemesterModel> ISemesterRepository.GetSemesterByIdNoTracking(int semesterId)
        {
            if (!bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache)))
                return await _sudyContext.Semesters.Include(x => x.CollegeSubjects).Include(x => x.Course).Include(x => x.Institution).FirstOrDefaultAsync(x => x.SemesterId == semesterId);

            string resultCache = await _cachingService.Get(nameof(SemesterModel) + semesterId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<SemesterModel>(resultCache);

            SemesterModel semester = await _sudyContext.Semesters.Include(x => x.CollegeSubjects).Include(x => x.Course).Include(x => x.Institution).FirstOrDefaultAsync(x => x.SemesterId == semesterId);

            if (semester != null)
                await _cachingService.Set(nameof(SemesterModel) + semesterId, JsonConvert.SerializeObject(semester));

            return semester;
        }

        #endregion

        #endregion
    }
}
