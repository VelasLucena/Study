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

        #endregion

        #region Constructor

        public CourseRepository(SudyContext sudyContext, ICacheService cacheService)
        {
            _sudyContext = sudyContext;
            _cachingService = cacheService;
        }

        #endregion

        #region Methods

        #region GetAllCourses

        async Task<List<CourseModel>> ICourseRepository.GetAllCourses(Ordering ordering = Ordering.Asc, string attributeName = nameof(CourseModel.Name))
        {
            switch (ordering)
            {
                case Ordering.Asc:
                    return await _sudyContext.Courses.OrderBy(x => EF.Property<object>(x, attributeName)).ToListAsync();
                case Ordering.Desc:
                    return await _sudyContext.Courses.OrderByDescending(x => EF.Property<object>(x, attributeName)).ToListAsync();
            }

            return null;
        }

        async Task<List<CourseModel>> ICourseRepository.GetAllCoursesNoTracking(Ordering ordering = Ordering.Asc, string attributeName = nameof(CourseModel.Name))
        {
            switch (ordering)
            {
                case Ordering.Asc:
                    return await _sudyContext.Courses.AsNoTracking().OrderBy(x => EF.Property<object>(x, attributeName)).ToListAsync();
                case Ordering.Desc:
                    return await _sudyContext.Courses.AsNoTracking().OrderByDescending(x => EF.Property<object>(x, attributeName)).ToListAsync();
            }

            return null;
        }

        #region GetCourseByName

        async Task<List<CourseModel>> ICourseRepository.GetCourseByName(string courseName)
        {
            return await _sudyContext.Courses.Where(x => x.Name.Contains(courseName)).ToListAsync();
        }

        async Task<List<CourseModel>> ICourseRepository.GetCourseByNameNoTracking(string courseName)
        {
            return await _sudyContext.Courses.AsNoTracking().Where(x => x.Name.Contains(courseName)).ToListAsync();
        }

        #endregion

        #region GetCourseById

        async Task<CourseModel> ICourseRepository.GetCourseById(int courseId)
        {
            return await _sudyContext.Courses.SingleOrDefaultAsync(x => x.CourseId == courseId);
        }

        async Task<CourseModel> ICourseRepository.GetCourseByIdNoTracking(int courseId)
        {
            if (!bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache)))
                return await _sudyContext.Courses.AsNoTracking().SingleOrDefaultAsync(x => x.CourseId == courseId);

            string resultCache = await _cachingService.Get(nameof(CourseModel) + courseId);

            if (!string.IsNullOrEmpty(resultCache))
                return JsonConvert.DeserializeObject<CourseModel>(resultCache);

            CourseModel course = await _sudyContext.Courses.AsNoTracking().SingleOrDefaultAsync(x => x.CourseId == courseId);
            if (course != null)
                await _cachingService.Set(nameof(CourseModel) + courseId, JsonConvert.SerializeObject(course));

            return course;
        }

        #endregion

        #endregion

        #endregion
    }
}
