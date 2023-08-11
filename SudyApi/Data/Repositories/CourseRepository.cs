using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudandoApi.Data.Contexts;
using SudyApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.Utility;
using System.Collections.Generic;

namespace SudyApi.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        #region Field

        private readonly SudyContext _sudyContext;
        private readonly ICacheService _cachingService;
        private readonly DataOptionsModel _dataOptions;

        #endregion

        #region Constructor

        public CourseRepository(SudyContext sudyContext, ICacheService cacheService, DataOptionsModel dataOptions)
        {
            _sudyContext = sudyContext;
            _cachingService = cacheService;
            _dataOptions = dataOptions;
        }

        #endregion

        #region Methods

        public async Task<List<CourseModel>> GetAllCourses()
        {

            return await _sudyContext.Courses
                .Take(_dataOptions.Take)
                .Skip(_dataOptions.Skip)
                .ApplyOrderBy(_dataOptions.KeyOrder, _dataOptions.Ordering)
                .ApplyTracking(_dataOptions.IsTracking)
                .ToListAsync();

        }     

        public async Task<List<CourseModel>> GetCourseByName(string courseName)
        {
            return await _sudyContext.Courses
                .Where(x => x.Name.Contains(courseName))
                .Take(_dataOptions.Take)
                .Skip(_dataOptions.Skip)
                .ApplyOrderBy(_dataOptions.KeyOrder, _dataOptions.Ordering)
                .ApplyTracking(_dataOptions.IsTracking)
                .ToListAsync();
        }

        public async Task<CourseModel> GetCourseById(int courseId)
        {
            bool cache = !bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache));

            if (_dataOptions.IsTracking == true)
                cache = false;

            if (!cache)
                return await _sudyContext.Courses
                    .SingleOrDefaultAsync(x => x.CourseId == courseId);

            string resultCache = await _cachingService.Get(nameof(CourseModel) + courseId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<CourseModel>(resultCache);

            CourseModel? course = await _sudyContext.Courses
                .ApplyTracking(_dataOptions.IsTracking)
                .SingleOrDefaultAsync(x => x.CourseId == courseId);

            if (course != null)
                await _cachingService.Set(nameof(CourseModel) + courseId, JsonConvert.SerializeObject(course));

            return course;
        }       

        #endregion
    }
}
