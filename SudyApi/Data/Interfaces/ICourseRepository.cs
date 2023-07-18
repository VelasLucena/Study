using SudyApi.Models;
using SudyApi.Properties.Enuns;

namespace SudyApi.Data.Interfaces
{
    public interface ICourseRepository
    {
        #region GetAllCourses

        Task<List<CourseModel>> GetAllCourses(Ordering ordering, string attributeName);

        Task<List<CourseModel>> GetAllCoursesNoTracking(Ordering ordering, string attributeName);

        #endregion

        #region GetCourseByName

        Task<List<CourseModel>> GetCourseByName(string courseName);

        Task<List<CourseModel>> GetCourseByNameNoTracking(string courseName);

        #endregion
    }
}
