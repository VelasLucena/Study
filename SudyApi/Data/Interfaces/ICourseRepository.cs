using SudyApi.Models;
using SudyApi.Properties.Enuns;

namespace SudyApi.Data.Interfaces
{
    public interface ICourseRepository
    {
        #region GetAllCourses

        Task<List<CourseModel>> GetAllCourses(Ordering ordering, string attributeName, int take = 10, int skip = 0);

        Task<List<CourseModel>> GetAllCoursesNoTracking(Ordering ordering, string attributeName, int take = 10, int skip = 0);

        #endregion

        #region GetCourseByName

        Task<List<CourseModel>> GetCourseByName(string courseName, int take = 10, int skip = 0);

        Task<List<CourseModel>> GetCourseByNameNoTracking(string courseName, int take = 10, int skip = 0);

        #endregion

        #region GetCourseById

        Task<CourseModel> GetCourseById(int courseId);

        Task<CourseModel> GetCourseByIdNoTracking(int courseId);

        #endregion
    }
}
