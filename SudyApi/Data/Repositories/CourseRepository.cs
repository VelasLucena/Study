using Microsoft.EntityFrameworkCore;
using StudandoApi.Data.Contexts;
using SudyApi.Data.Interfaces;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
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

        async Task<List<CourseModel>> ICourseRepository.GetAllCourses(Ordering ordering = Ordering.Asc, string attributeName = nameof(CourseModel.CourseName))
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

        async Task<List<CourseModel>> ICourseRepository.GetAllCoursesNoTracking(Ordering ordering = Ordering.Asc, string attributeName = nameof(CourseModel.CourseName))
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

        async Task<List<CourseModel>> ICourseRepository.GetCourseByName(string courseName)
        {
            return await _sudyContext.Courses.Where(x => x.CourseName.Contains(courseName)).ToListAsync();
        }

        async Task<List<CourseModel>> ICourseRepository.GetCourseByNameNoTracking(string courseName)
        {
            return await _sudyContext.Courses.AsNoTracking().Where(x => x.CourseName.Contains(courseName)).ToListAsync();
        }

        #endregion

        #endregion
    }
}
